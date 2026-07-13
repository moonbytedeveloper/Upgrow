using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.WL;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL
{
    public class WLSocialMediaService : WLBaseService<WL_MasterSocialMedia, WLSocialMediaDto, WLSocialMediaCommand>, IWLSocialMediaService
    {
        public WLSocialMediaService(
            IMasterRepository<WL_MasterSocialMedia> repository,
            IMasterRepository<Tenant> tenantRepository,
            IMapper mapper)
            : base(repository, tenantRepository, mapper)
        {

        }
        protected override string EntityDisplayName => "Record";

        protected override Expression<Func<WL_MasterSocialMedia, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.PlatformName != null && x.PlatformName.ToLower().Contains(searchTerm) ||
                 (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)) ||
                 (x.ProfileURL != null && x.ProfileURL.ToLower().Contains(searchTerm)));
        }

        protected override async Task<bool> IsDuplicateAsync(WLSocialMediaCommand command)
        {

            return await _repository.ExistsAsync(x =>
                x.PlatformName!.ToLower().Trim() == command.PlatformName.ToLower().Trim() &&
                x.TenantId == command.TenantId &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<WL_MasterSocialMedia>, IOrderedQueryable<WL_MasterSocialMedia>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "platformName" => q => isAsc ? q.OrderBy(x => x.PlatformName) : q.OrderByDescending(x => x.PlatformName),
                "profileURL" => q => isAsc ? q.OrderBy(x => x.ProfileURL) : q.OrderByDescending(x => x.ProfileURL),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}