using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL
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