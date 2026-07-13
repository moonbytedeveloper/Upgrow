using AutoMapper;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTOs.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Services.Master;

namespace Upgrow.Application.Services.Master
{
    public class MasterCMSService : MasterServiceBase<Master_CMS, MasterCMSDto, MasterCMSCommand>, IMasterCMSService
    {
        public MasterCMSService(
            IMasterRepository<Master_CMS> repository,
            IMapper mapper)
            : base(repository, mapper)
        {
        }

        public async Task<MasterCMSDto?> GetByCodeAsync(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return null;

                var list = await _repository.FindAllAsync(x => x.Code == code && x.IsActive);
                var match = list.FirstOrDefault();

                return match == null ? null : _mapper.Map<MasterCMSDto>(match);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<MasterCMSDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterCMSDto>>(entities);
        }
        protected override async Task<bool> IsDuplicateAsync(MasterCMSCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.PageTitle!.ToLower().Trim() == command.PageTitle.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Master_CMS, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.PageTitle != null && x.PageTitle.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<Master_CMS>, IOrderedQueryable<Master_CMS>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "pagetitle" => q => isAsc ? q.OrderBy(x => x.PageTitle) : q.OrderByDescending(x => x.PageTitle),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}