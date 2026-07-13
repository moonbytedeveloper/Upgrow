using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class MasterNomenClatureRepository : MasterRepositoryBase<Master_Nomenclature>, IMasterNomenClatureRepository
    {
        public MasterNomenClatureRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<Master_Nomenclature>> GetPagedAsync(
        Expression<Func<Master_Nomenclature, bool>>? filter,
       PaginationParams pagination,
        Func<IQueryable<Master_Nomenclature>, IOrderedQueryable<Master_Nomenclature>>? orderBy = null,
        Func<IQueryable<Master_Nomenclature>, IQueryable<Master_Nomenclature>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                  filter,
                pagination,
                orderBy, query => from e in _context.Master_Nomenclature
                                  join r in _context.Master_FinancialYear
                                      on e.FinancialYearUUID equals r.UUID into yearGroup
                                  from r in yearGroup.DefaultIfEmpty()
                                  select new Master_Nomenclature
                                  {
                                      UUID = e.UUID,
                                      ModuleKey = e.ModuleKey,
                                      Prefix = e.Prefix,
                                      StartNo = e.StartNo,
                                      NumberOfDigits = e.NumberOfDigits,
                                      IsActive = e.IsActive,
                                      FinancialYearUUID = r != null ? r.Title : null,       // map title
                                      Id = e.Id // for ordering
                                  });


        }
        public async Task<int?> GetMaxNumberByModuleAsync(string moduleKey, int digits)
        {
            List<string> codes = new();

            moduleKey = moduleKey?.Trim();

            switch (moduleKey)
            {
                case Constants.NomenclatureModuleKeys.Employee:
                    codes = await _context.Master_Employee
                        .Where(x => !string.IsNullOrEmpty(x.EmployeeCode))
                        .Select(x => x.EmployeeCode)
                        .ToListAsync();
                    break;

                //case Constants.NomenclatureModuleKeys.Agent:
                //    codes = await _context.Master_Customer
                //        .Where(x => !string.IsNullOrEmpty(x.CustomerCode))
                //        .Select(x => x.CustomerCode)
                //        .ToListAsync();
                //    break;

                case Constants.NomenclatureModuleKeys.SupportTicket:
                    codes = await _context.Support_TicketHeader
                        .Where(x => !string.IsNullOrEmpty(x.TicketNumber))
                        .Select(x => x.TicketNumber)
                        .ToListAsync();
                    break;

                default:
                    throw new Exception($"Unsupported module '{moduleKey}'");
            }

            if (!codes.Any())
                return null;

            var numbers = codes.Select(code =>
            {
                if (code.Length >= digits)
                {
                    var lastPart = code.Substring(code.Length - digits);
                    return int.TryParse(lastPart, out var n) ? n : 0;
                }
                return 0;
            });

            var max = numbers.Max();

            return max > 0 ? max : (int?)null;
        }
    }
}
