using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterSocialMediaService : MasterServiceBase<Master_SocialMedia, MasterSocialMediaDto, MasterSocialMediaCommand>, IMasterSocialMediaService
    {
        public MasterSocialMediaService(IMasterRepository<Master_SocialMedia> repository, IMapper mapper)
            : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override Expression<Func<Master_SocialMedia, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.PlatformName != null && x.PlatformName.ToLower().Contains(searchTerm)) ||
                (x.ProfileURL != null && x.ProfileURL.ToLower().Contains(searchTerm)) ||
                (x.IconURL != null && x.IconURL.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterSocialMediaCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.PlatformName.ToLower().Trim() == command.PlatformName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<Master_SocialMedia>, IOrderedQueryable<Master_SocialMedia>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "platformname" => q => isAsc ? q.OrderBy(x => x.PlatformName) : q.OrderByDescending(x => x.PlatformName),
                "profileurl" => q => isAsc ? q.OrderBy(x => x.ProfileURL) : q.OrderByDescending(x => x.ProfileURL),
                "sequenceno" => q => isAsc ? q.OrderBy(x => x.DisplayOrder) : q.OrderByDescending(x => x.DisplayOrder),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public override async Task DeleteAsync(string uuid, string userUuid, string ip)
        {
            var entity = await _repository.GetByUuidAsync(uuid)
                ?? throw new Exception("Social Media not found");

            entity.IsActive = false;
            await _repository.UpdateAsync(entity);
        }
    }
}