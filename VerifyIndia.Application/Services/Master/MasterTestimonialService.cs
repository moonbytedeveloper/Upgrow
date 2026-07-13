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
    public class MasterTestimonialService : MasterServiceBase<Master_Testimonial, MasterTestimonialDto, MasterTestimonialCommand>, IMasterTestimonialService
    {
        public MasterTestimonialService(IMasterRepository<Master_Testimonial> repository, IMapper mapper) : base(repository, mapper) { }
        protected override Expression<Func<Master_Testimonial, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
            (x.Comment != null && x.Comment.ToLower().Contains(searchTerm)) ||
            (x.CompanyName != null && x.CompanyName.ToLower().Contains(searchTerm)) ||
            (x.CustomerName != null && x.CustomerName.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterTestimonialCommand command)
        {
            return await _repository.ExistsAsync(x => x.CustomerName!.ToLower().Trim() == command.CustomerName.ToLower().Trim() &&
           x.UUID != command.UUID);
        }

        protected override Func<IQueryable<Master_Testimonial>, IOrderedQueryable<Master_Testimonial>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "comment" => q => isAsc ? q.OrderBy(x => x.Comment) : q.OrderByDescending(x => x.Comment),
                "star" => q => isAsc ? q.OrderBy(x => x.Star) : q.OrderByDescending(x => x.Star),
                "companyname" => q => isAsc ? q.OrderBy(x => x.CompanyName) : q.OrderByDescending(x => x.CompanyName),
                "customername" => q => isAsc ? q.OrderBy(x => x.CustomerName) : q.OrderByDescending(x => x.CustomerName),
                "Sequenseno" => q => isAsc ? q.OrderBy(x => x.SequenceNo) : q.OrderByDescending(x => x.SequenceNo),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}
