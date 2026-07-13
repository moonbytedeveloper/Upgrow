using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterFaqService : MasterServiceBase<Master_FAQ, MasterFaqDto, MasterFaqCommand>, IMasterFaqService
    {
        private readonly IMasterRepository<Master_FAQSubCategory> _subCategoryRepo;
        private readonly IMasterRepository<Master_FAQCategory> _categoryRepo;

        public MasterFaqService(IMasterRepository<Master_FAQ> repository,
            IMasterRepository<Master_FAQSubCategory> subCategoryRepo,
            IMasterRepository<Master_FAQCategory> categoryRepo,
         IMapper mapper)
         : base(repository, mapper) 
        {
            _subCategoryRepo = subCategoryRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<MasterFaqDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterFaqDto>>(entities);
        }

        protected override Expression<Func<Master_FAQ, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                 (x.FAQSubCategoryUUID != null && x.FAQSubCategoryUUID.ToLower().Contains(searchTerm) ||
                 (x.FAQCategoryUUID != null && x.FAQCategoryUUID.ToLower().Contains(searchTerm)));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterFaqCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.FAQCategoryUUID!.ToLower().Trim() == command.FAQCategoryUUID.ToLower().Trim() &&
                x.FAQSubCategoryUUID!.ToLower().Trim() == command.FAQSubCategoryUUID.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<Master_FAQ>, IOrderedQueryable<Master_FAQ>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                "category" => q => isAsc ? q.OrderBy(x => x.FAQCategoryUUID) : q.OrderByDescending(x => x.FAQCategoryUUID),

                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }



        public async Task<List<SubcategoryFaqsDto>> GetFaqsGroupedByCategoryUuidAsync(string categoryUuid)
        {
            var result = new List<SubcategoryFaqsDto>();

            if (string.IsNullOrWhiteSpace(categoryUuid))
                return result;

            var trimmed = categoryUuid.Trim().ToLower();

            // Load all FAQs for this category
            var faqEntities = await _repository.FindAllAsync(x =>
                !string.IsNullOrWhiteSpace(x.FAQCategoryUUID) &&
                x.FAQCategoryUUID.ToLower() == trimmed);

            if (faqEntities == null || !faqEntities.Any())
                return result;

            // Load all active subcategories for this category
            var subCategories = await _subCategoryRepo.FindAllAsync(x =>
                !string.IsNullOrWhiteSpace(x.FAQCategoryUUID) &&
                x.FAQCategoryUUID.ToLower() == trimmed);

            if (subCategories == null || !subCategories.Any())
                return result;

            // Build groups for each subcategory
            foreach (var sub in subCategories)
            {
                var group = new SubcategoryFaqsDto
                {
                    UUID = sub.UUID,
                    Title = sub.Title,
                    Image = sub.Image
                };

                // Match FAQs for this subcategory
                group.Faqs = faqEntities
                    .Where(f =>
                        !string.IsNullOrWhiteSpace(f.FAQSubCategoryUUID) &&
                        (string.Equals(f.FAQSubCategoryUUID.Trim().ToLower(), sub.UUID.Trim().ToLower(), StringComparison.Ordinal) ||
                         string.Equals(f.FAQSubCategoryUUID.Trim().ToLower(), (sub.Title ?? string.Empty).Trim().ToLower(), StringComparison.Ordinal)))
                    .Select(f => new FaqSimpleDto
                    {
                        UUID = f.UUID,
                        Question = f.Title,
                        Answer = f.Description
                    })
                    .ToList();

                // Add group even if no FAQs (so subcategories show up)
                result.Add(group);
            }

            return result;
        }
    }
}
