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
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterNomenclatureService : MasterServiceBase<Master_Nomenclature, MasterNomenclatureDto, MasterNomenclatureCommand>, IMasterNomenclatureService
    {
        private readonly IMasterNomenClatureRepository _nomenClatureRepository;
        public MasterNomenclatureService(IMasterRepository<Master_Nomenclature> repository, IMapper mapper,IMasterNomenClatureRepository nomenClatureRepository)
            : base(repository, mapper)
        {
            _nomenClatureRepository = nomenClatureRepository;
        }

        protected override Expression<Func<Master_Nomenclature, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.ModuleKey != null && x.ModuleKey.ToLower().Contains(searchTerm)) ||
                (x.NumberOfDigits != null && x.NumberOfDigits.ToString().ToLower().Contains(searchTerm)) ||
                (x.FinancialYearUUID != null && x.FinancialYearUUID.ToLower().Contains(searchTerm)) ||
                (x.Prefix != null && x.Prefix.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterNomenclatureCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ModuleKey.ToLower().Trim() == command.ModuleKey.ToLower().Trim() &&
                x.Prefix.ToLower().Trim() == command.Prefix.ToLower().Trim() &&
                x.StartNo == command.StartNo &&
                x.UUID != command.UUID);
        }


        protected override Func<IQueryable<Master_Nomenclature>, IOrderedQueryable<Master_Nomenclature>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "key" => q => isAsc ? q.OrderBy(x => x.ModuleKey) : q.OrderByDescending(x => x.ModuleKey),
                "prefix" => q => isAsc ? q.OrderBy(x => x.Prefix) : q.OrderByDescending(x => x.Prefix),
                "startno" => q => isAsc ? q.OrderBy(x => x.StartNo) : q.OrderByDescending(x => x.StartNo),
                "numberofdigit" => q => isAsc ? q.OrderBy(x => x.NumberOfDigits) : q.OrderByDescending(x => x.NumberOfDigits),
                "finicialyear" => q => isAsc ? q.OrderBy(x => x.FinancialYearUUID) : q.OrderByDescending(x => x.FinancialYearUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<string> GenerateCodeAsync(string moduleKey)
        {
            var data = (await _repository.GetAllActiveAsync())
                .FirstOrDefault(x => x.ModuleKey.ToLower().Trim() == moduleKey.ToLower().Trim());

            if (data == null)
                throw new Exception($"Nomenclature not configured for '{moduleKey}'");

            int digits = data.NumberOfDigits ?? 2;

            // 🔹 Get max number dynamically
            var maxNo = await _nomenClatureRepository.GetMaxNumberByModuleAsync(moduleKey, digits);

            decimal nextNo = maxNo.HasValue
                ? maxNo.Value + 1
                : (data.StartNo ?? 1);

            string yearPart = data.IsIncludeYear == true
                ? DateTime.Now.Year.ToString()
                : "";

            string numberPart = nextNo.ToString().PadLeft(digits, '0');

            return $"{data.Prefix}{yearPart}{numberPart}";
        }
    }

}
