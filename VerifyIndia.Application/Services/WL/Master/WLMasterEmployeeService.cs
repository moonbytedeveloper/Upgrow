using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Application.Services.WL.Master
{
    public class WLMasterEmployeeService : MasterServiceBase<WL_MasterEmployee, WLMasterEmployeeDto, WLMasterEmployeeCommand>, IWLMasterEmployeeService
    {
        private readonly IPasswordHasher<WLMasterEmployeeCommand> _passwordHasher;
        public WLMasterEmployeeService(IWLMasterEmployeeRepository repository, IMapper mapper, IWLMasterDesignationService designationService, IPasswordHasher<WLMasterEmployeeCommand> passwordHasher) : base
            (repository, mapper)
        {
            _passwordHasher = passwordHasher;
        }
        // Define which fields to search
        protected override Expression<Func<WL_MasterEmployee, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x => (
            (x.FirstName != null && x.FirstName.ToLower().Contains(searchTerm)) ||
            (x.LastName != null && x.LastName.ToLower().Contains(searchTerm)) ||
            (x.FirstName != null && x.LastName != null &&
            (x.FirstName + " " + x.LastName).ToLower().Contains(searchTerm))) ||
           (x.HonorificUUID != null && x.FirstName != null && x.LastName != null &&
           (x.HonorificUUID + " " + x.FirstName + " " + x.LastName).ToLower().Contains(searchTerm)) ||
            (x.RoleUUID != null && x.RoleUUID.ToLower().Contains(searchTerm)) ||
            (x.DesignationUUID != null && x.DesignationUUID.ToLower().Contains(searchTerm)) ||
            (x.DepartmentUUID != null && x.DepartmentUUID.ToLower().Contains(searchTerm));
        }
        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterEmployeeCommand command)
        {
            var email = command.EmailId?.ToLower().Trim();
            var mobile = command.MobileNumber?.ToLower().Trim();

            return await _repository.ExistsAsync(x =>
                (
                    x.EmailId!.ToLower().Trim() == email ||
                    x.MobileNumber!.ToLower().Trim() == mobile
                )
                && x.UUID != command.UUID
            );
        }


        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_MasterEmployee>, IOrderedQueryable<WL_MasterEmployee>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "fullname" => q => isAsc ? q.OrderBy(x => x.HonorificUUID + x.FirstName + x.LastName) : q.OrderByDescending(x => x.HonorificUUID + x.FirstName + x.LastName),
                "employeeid" => q => isAsc ? q.OrderBy(x => x.EmployeeCode) : q.OrderByDescending(x => x.EmployeeCode),
                "role" => q => isAsc ? q.OrderBy(x => x.RoleUUID) : q.OrderByDescending(x => x.RoleUUID),
                "department" => q => isAsc ? q.OrderBy(x => x.DepartmentUUID) : q.OrderByDescending(x => x.DepartmentUUID),
                "designation" => q => isAsc ? q.OrderBy(x => x.DesignationUUID) : q.OrderByDescending(x => x.DesignationUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        public async Task ChangePasswordAsync(string uuid, string currentPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(uuid))
                throw new ArgumentException("Invalid employee UUID");

            var employee = await _repository.GetByUuidAsync(uuid);
            if (employee == null)
                throw new Exception("Employee not found");

            var employeeCommand = new WLMasterEmployeeCommand
            {
                UUID = employee.UUID,
                Password = employee.Password
            };

            var verification = _passwordHasher.VerifyHashedPassword(employeeCommand, employee.Password, currentPassword);
            if (verification == PasswordVerificationResult.Failed)
                throw new Exception("Current password is incorrect");

            employee.Password = _passwordHasher.HashPassword(employeeCommand, newPassword);

            await _repository.UpdateAsync(employee);
        }
    }
}

