using Upgrow.Application.DTO.AIX;

namespace Upgrow.Application.IServices.Api
{
    public interface IMaster_ProgrammingLanguageService
    {
        Task<List<ProgrammingLanguageDto>> GetAllActiveLangunagesAsync();
        Task<ProgrammingLanguageDto?> GetByUuidAsync(string uuid);
    }
}