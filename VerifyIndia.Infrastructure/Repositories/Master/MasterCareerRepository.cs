using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class MasterCareerRepository : MasterRepositoryBase<Master_Career>
    {
        public MasterCareerRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<Master_Career>> GetPagedAsync(
        Expression<Func<Master_Career, bool>>? filter,
       PaginationParams pagination,
        Func<IQueryable<Master_Career>, IOrderedQueryable<Master_Career>>? orderBy = null,
        Func<IQueryable<Master_Career>, IQueryable<Master_Career>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                  filter,
                pagination,
                orderBy, query => from e in _context.Master_Career
                                  join r in _context.Master_Department
                                      on e.DepartmentUUID equals r.UUID into departmentGroup
                                  from r in departmentGroup.DefaultIfEmpty()
                                  select new Master_Career
                                  {
                                      UUID = e.UUID,
                                      Name = e.Name,
                                      IconImage = e.IconImage,
                                      Experience = e.Experience,
                                      NumberOfPosition = e.NumberOfPosition,
                                      ShortDescription = e.ShortDescription,
                                      IsActive = e.IsActive,
                                      DepartmentUUID = r != null ? r.Title : null,       // map title
                                      Id = e.Id // for ordering
                                  });


        }
    }
}
