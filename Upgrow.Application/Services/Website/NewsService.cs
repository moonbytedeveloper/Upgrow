using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTOs;
using Upgrow.Application.DTOs.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class NewsService : MasterServiceBase<News, NewsDto, NewsCommand>, INewsService
    {
        private readonly IMasterRepository<SavedNews> _savedRepository;
        private readonly IMasterRepository<News> _newsRepository;
        private readonly IMasterRepository<News_Category> _newscategoryRepository;

        public NewsService(
            IMasterRepository<News> repository,
            IMasterRepository<News_Category> newscategoryRepository,
            IMasterRepository<SavedNews> savednewsRepository,
            IMasterRepository<News> newsRepository,
            IMapper mapper)
            : base(repository, mapper)
        {
            _newscategoryRepository = newscategoryRepository;
            _savedRepository = savednewsRepository;
            _newsRepository = newsRepository;
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(NewsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<News, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.NewsCategoryUUID != null && x.NewsCategoryUUID.ToLower().Contains(searchTerm)) ||
                (x.ShortDescription != null && x.ShortDescription.ToLower().Contains(searchTerm));
        }

        // Define sorting
        protected override Func<IQueryable<News>, IOrderedQueryable<News>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "shortdescription" => q => isAsc ? q.OrderBy(x => x.ShortDescription) : q.OrderByDescending(x => x.ShortDescription),

                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<NewsApiDto>> GetByCategoryAsync(string? categoryUuid)
        {
            // Normalize once, here, in C#
            var normalizedUuid = categoryUuid?.Trim();

            Expression<Func<News, bool>> filter = string.IsNullOrWhiteSpace(normalizedUuid)
                ? n => n.IsActive == true
                : n => n.IsActive == true && n.NewsCategoryUUID == normalizedUuid;

            var candidates = await _repository.FindAllAsync(filter);

            return _mapper.Map<List<NewsApiDto>>(
                candidates.OrderByDescending(n => n.PublishDate ?? DateTime.MinValue));
        }

        

        public async Task SaveAsync(string newsUuid, string customerUuid)
        {
            if (string.IsNullOrWhiteSpace(newsUuid)) throw new ArgumentException("newsUuid required", nameof(newsUuid));
            if (string.IsNullOrWhiteSpace(customerUuid)) throw new ArgumentException("customerUuid required", nameof(customerUuid));

            // Normalize inputs
            newsUuid = newsUuid.Trim();
            customerUuid = customerUuid.Trim();

            // Find any existing saved record for this customer + news
            var exists = await _savedRepository.FindAllAsync(s =>
                s.NewsUUID == newsUuid && s.CustomerUUID == customerUuid);

            var existingEntity = exists?.FirstOrDefault();

            if (existingEntity != null)
            {
                // Toggle IsActive when record exists (true -> false, false -> true)
                existingEntity.IsActive = !existingEntity.IsActive;
                await _savedRepository.UpdateAsync(existingEntity);
                return;
            }

            // If no existing record, create as active (original behaviour)
            var entity = new SavedNews
            {
                UUID = Utils.GetUUID(),
                NewsUUID = newsUuid,
                CustomerUUID = customerUuid,
                IsActive = true
            };

            await _savedRepository.AddAsync(entity);
        }


        public async Task<List<NewsDetailDto>> GetSavedNewsAsync(string customerUuid, string? categoryUuid = null)
        {
            if (string.IsNullOrWhiteSpace(customerUuid))
                return new List<NewsDetailDto>();

            var saved = await _savedRepository.FindAllAsync(s => s.CustomerUUID == customerUuid && s.IsActive == true);
            var newsUuids = saved?
                .Select(s => s.NewsUUID)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Select(u => u!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList()
                ?? new List<string>();

            if (!newsUuids.Any())
                return new List<NewsDetailDto>();

            // normalize category once
            string? normalizedCategory = string.IsNullOrWhiteSpace(categoryUuid) ? null : categoryUuid!.Trim().ToLowerInvariant();

            // use configured news repository if available, otherwise fallback to base repository
            var repo = _newsRepository ?? _repository;

            Expression<Func<News, bool>> predicate;
            if (normalizedCategory == null)
            {
                predicate = n => n.IsActive == true && newsUuids.Contains(n.UUID);
            }
            else
            {
                var nc = normalizedCategory; // capture for EF translation
                predicate = n => n.IsActive == true
                                 && newsUuids.Contains(n.UUID)
                                 && !string.IsNullOrWhiteSpace(n.NewsCategoryUUID)
                                 && n.NewsCategoryUUID!.Trim().ToLower() == nc;
            }

            var newsEntities = await repo.FindAllAsync(predicate);
            var ordered = newsEntities.OrderByDescending(n => n.PublishDate ?? DateTime.MinValue).ToList();

            // resolve category titles in bulk
            var categoryUuids = ordered
                .Select(n => n.NewsCategoryUUID)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Select(u => u!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            Dictionary<string, string> categoryMap = new();

            if (categoryUuids.Any() && _newscategoryRepository != null)
            {
                var categories = await _newscategoryRepository.FindAllAsync(c => categoryUuids.Contains(c.UUID));
                if (categories != null)
                {
                    categoryMap = categories
                        .Where(c => !string.IsNullOrWhiteSpace(c.UUID))
                        .ToDictionary(c => c.UUID!, c => c.Title ?? string.Empty, StringComparer.OrdinalIgnoreCase);
                }
            }

            var dtos = ordered.Select(n =>
            {
                var catUuid = n.NewsCategoryUUID;
                var catTitle = !string.IsNullOrWhiteSpace(catUuid) && categoryMap.TryGetValue(catUuid!.Trim(), out var title)
                    ? title
                    : catUuid;

                return new NewsDetailDto
                {
                    UUID = n.UUID ?? string.Empty,
                    Title = n.Title ?? string.Empty,
                    ShortDescription = n.ShortDescription,
                    NewsCategoryUUID = n.NewsCategoryUUID,
                    NewsCategoryTitle = catTitle,
                    PublishDate = n.PublishDate,
                    Source = n.Source,
                    Location = n.Location,
                    FullDescription = n.FullDescription,
                    Image = n.Image,
                    CardImage = n.CardImage,
                    IsActive = n.IsActive
                };
            }).ToList();

            return dtos;
        }

        public async Task<PagedResult<NewsDto>> GetByCategoryPagedAsync(string? categoryUuid, DataTableRequest request)
        {
            request ??= new DataTableRequest();
            var normalizedUuid = categoryUuid?.Trim();

            Func<IQueryable<News>, IQueryable<News>> queryModifier = q =>
            {
                q = q.Where(x => x.IsActive == true);

                if (!string.IsNullOrWhiteSpace(normalizedUuid))
                    q = q.Where(x => x.NewsCategoryUUID == normalizedUuid);

                return q;
            };

            return await base.GetPagedAsync(request, queryModifier);
        }

        public async Task<PagedResult<NewsDto>> GetByTopStoriesPagedAsync(string? categoryUuid, DataTableRequest request)
        {
            request ??= new DataTableRequest();
            var normalizedUuid = categoryUuid?.Trim();

            Func<IQueryable<News>, IQueryable<News>> queryModifier = q =>
            {
                q = q.Where(x => x.IsActive == true && x.IsTopStory == true);

                if (!string.IsNullOrWhiteSpace(normalizedUuid))
                    q = q.Where(x => x.NewsCategoryUUID == normalizedUuid);

                return q;
            };

            return await base.GetPagedAsync(request, queryModifier);
        }

        public async Task<PagedResult<NewsDto>> GetSavedNewsPagedAsync(
    string customerUuid,
    string? categoryUuid,
    DataTableRequest request)
        {
            request ??= new DataTableRequest();

            if (string.IsNullOrWhiteSpace(customerUuid))
            {
                return new PagedResult<NewsDto>
                {
                    Items = [],
                    TotalCount = 0,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }

            var saved = await _savedRepository.FindAllAsync(s =>
                s.CustomerUUID == customerUuid && s.IsActive == true);

            var newsUuids = saved?
                .Select(s => s.NewsUUID)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Select(u => u!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList()
                ?? [];

            if (!newsUuids.Any())
            {
                return new PagedResult<NewsDto>
                {
                    Items = [],
                    TotalCount = 0,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }

            var normalizedCategory = string.IsNullOrWhiteSpace(categoryUuid) ? null : categoryUuid.Trim();

            Func<IQueryable<News>, IQueryable<News>> queryModifier = q =>
            {
                q = q.Where(x => x.IsActive == true && x.UUID != null && newsUuids.Contains(x.UUID));

                if (!string.IsNullOrWhiteSpace(normalizedCategory))
                    q = q.Where(x => x.NewsCategoryUUID == normalizedCategory);

                return q;
            };

            return await base.GetPagedAsync(request, queryModifier);
        }

    }
}

    
