using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class MasterOfferService : MasterServiceBase<Master_Offer, MasterOfferDto, MasterOfferCommand>, IMasterOfferService
    {
        public MasterOfferService(IMasterRepository<Master_Offer> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        protected override Expression<Func<Master_Offer, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            decimal.TryParse(searchTerm, out decimal offerValue);
            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                (x.OfferCode != null && x.OfferCode.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm)) ||
                (x.OfferType != null && x.OfferType.ToLower().Contains(searchTerm)) ||
                (x.OfferType != null && x.OfferType.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterOfferCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.OfferCode!.ToLower().Trim() == command.OfferCode.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<Master_Offer>, IOrderedQueryable<Master_Offer>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                "offerCode" => q => isAsc ? q.OrderBy(x => x.OfferCode) : q.OrderByDescending(x => x.OfferCode),
                "offerValue" => q => isAsc ? q.OrderBy(x => x.OfferValue) : q.OrderByDescending(x => x.OfferValue),
                "offerType" => q => isAsc ? q.OrderBy(x => x.OfferType) : q.OrderByDescending(x => x.OfferType),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                "validFrom" => q => isAsc ? q.OrderBy(x => x.ValidFrom) : q.OrderByDescending(x => x.ValidFrom),
                "validTo" => q => isAsc ? q.OrderBy(x => x.ValidTo) : q.OrderByDescending(x => x.ValidTo),
                "minOrderAmount" => q => isAsc ? q.OrderBy(x => x.MinOrderAmount) : q.OrderByDescending(x => x.MinOrderAmount),
                "maxDiscountAmount" => q => isAsc ? q.OrderBy(x => x.MaxDiscountAmount) : q.OrderByDescending(x => x.MaxDiscountAmount),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };


        }
    }

}
