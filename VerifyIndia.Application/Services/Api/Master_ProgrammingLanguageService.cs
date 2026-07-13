using AutoMapper;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Api
{
    public class Master_ProgrammingLanguageService : IMaster_ProgrammingLanguageService
    {
        private readonly IMasterRepository<Master_ProgrammingLanguage> _languageRepository;
        private readonly IMapper _mapper;

        public Master_ProgrammingLanguageService(
            IMasterRepository<Master_ProgrammingLanguage> languageRepository,
            IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public async Task<List<ProgrammingLanguageDto>> GetAllActiveLangunagesAsync()
        {
            try
            {
                var languages = await _languageRepository.FindAllAsync(
                    x => x.IsActive);

                return _mapper.Map<List<ProgrammingLanguageDto>>(languages.OrderBy(l => l.Title));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching programming languages: {ex.Message}", ex);
            }
        }

        public async Task<ProgrammingLanguageDto?> GetByUuidAsync(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return null;

                var language = await _languageRepository.GetByUuidAsync(uuid);
                if (language != null && !language.IsActive)
                    return null;

                return _mapper.Map<ProgrammingLanguageDto>(language);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching programming language: {ex.Message}", ex);
            }
        }
    }
}