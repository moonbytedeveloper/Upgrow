using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Api
{
 
    public class ApiXStatusCodeService : IApiXStatusCodeService
    {
        private readonly IMasterRepository<ApiXStatusCodes> _statusCodeRepository;
        private readonly IMapper _mapper;

        public ApiXStatusCodeService(
            IMasterRepository<ApiXStatusCodes> statusCodeRepository,
            IMapper mapper)
        {
            _statusCodeRepository = statusCodeRepository;
            _mapper = mapper;
        }
 
        public async Task<ResponseStatusDto?> GetByUuidAsync(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return null;

                var statusCode = await _statusCodeRepository.GetByUuidAsync(uuid);
                return statusCode == null ? null : _mapper.Map<ResponseStatusDto>(statusCode);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching status code by UUID: {ex.Message}", ex);
            }
        }
 
        public async Task<List<ResponseStatusDto>> GetAllActiveAsync()
        {
            try
            {
                var statusCodes = await _statusCodeRepository.GetAllActiveAsync();
                return _mapper.Map<List<ResponseStatusDto>>(
                    statusCodes.OrderBy(s => s.StatusCode).ToList());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching all active status codes: {ex.Message}", ex);
            }
        }
 
        public async Task<List<ResponseStatusDto>> GetByUuidsAsync(List<string> uuids)
        {
            try
            {
                if (uuids == null || !uuids.Any())
                    return new List<ResponseStatusDto>();

                var statusCodes = await _statusCodeRepository.FindAllAsync(
                    x => uuids.Contains(x.UUID) && x.IsActive);

                return _mapper.Map<List<ResponseStatusDto>>(
                    statusCodes.OrderBy(s => s.StatusCode).ToList());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching status codes by UUIDs: {ex.Message}", ex);
            }
        }
    }
}