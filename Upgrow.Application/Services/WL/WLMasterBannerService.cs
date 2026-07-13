using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.WL;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL
{
    public class WLMasterBannerService
     : WLBaseService<WL_MasterBanner, WLMasterBannerDto, WLMasterBannerCommand>,
       IWLMasterBannerService
    {
        public WLMasterBannerService(
            IMasterRepository<WL_MasterBanner> repository,
            IMasterRepository<Tenant> tenantRepository,
            IMapper mapper)
            : base(repository, tenantRepository, mapper)
        {

        }
        protected override string EntityDisplayName => "Banner";
        protected override Expression<Func<WL_MasterBanner, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.MainTitle != null && x.MainTitle.ToLower().Contains(searchTerm)) ||
                (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)) ||
                (x.SubTitle != null && x.SubTitle.ToLower().Contains(searchTerm)) ||
                (x.ButtonText != null && x.ButtonText.ToLower().Contains(searchTerm)) ||
                (x.SequenceNo != null && Convert.ToString(x.SequenceNo).ToLower().Contains(searchTerm)) ||
                (x.ButtonURL != null && x.ButtonURL.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(WLMasterBannerCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.MainTitle!.ToLower().Trim() == command.MainTitle.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<WL_MasterBanner>, IOrderedQueryable<WL_MasterBanner>>? BuildSortExpression(
          string? sortColumn,
          string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "maintitle" => q => isAsc ? q.OrderBy(x => x.MainTitle) : q.OrderByDescending(x => x.MainTitle),
                "tenant" => q => isAsc ? q.OrderBy(x => x.TenantName) : q.OrderByDescending(x => x.TenantName),
                "subtitle" => q => isAsc ? q.OrderBy(x => x.SubTitle) : q.OrderByDescending(x => x.SubTitle),
                "buttontext" => q => isAsc ? q.OrderBy(x => x.ButtonText) : q.OrderByDescending(x => x.ButtonText),
                "buttonurl" => q => isAsc ? q.OrderBy(x => x.ButtonURL) : q.OrderByDescending(x => x.ButtonURL),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}
