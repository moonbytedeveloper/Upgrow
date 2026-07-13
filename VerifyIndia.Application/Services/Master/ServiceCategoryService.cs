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
using Upgrow.Domain.Entities.Auth;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class ServiceCategoryService : MasterServiceBase<Service_Category, ServiceCategoryDto, ServiceCategoryCommand>, IServiceCategoryService
    {
        public ServiceCategoryService(IMasterRepository<Service_Category> repository, IMapper mapper)
            : base(repository, mapper) { }
        public async Task<List<ServiceCategoryDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ServiceCategoryDto>>(entities);
        }

        // Define which fields to search
        protected override Expression<Func<Service_Category, bool>>? BuildSearchFilter(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return null;

            searchTerm = searchTerm.Trim().ToLowerInvariant();

            return e =>
                (e.CategoryName != null && e.CategoryName.ToLower().Contains(searchTerm))
                || (e.IconImage != null && e.IconImage.ToLower().Contains(searchTerm))
                || (e.Description != null && e.Description.ToLower().Contains(searchTerm));
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ServiceCategoryCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.CategoryName))
                return false;

            var candidate = command.CategoryName.Trim().ToLowerInvariant();
            return await _repository.ExistsAsync(x =>
                x.CategoryName != null &&
                x.CategoryName.ToLower().Trim() == candidate &&
                x.UUID != command.UUID);
        }
    }
}