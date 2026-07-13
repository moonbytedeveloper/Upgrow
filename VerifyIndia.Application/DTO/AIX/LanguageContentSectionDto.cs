namespace VerifyIndia.Application.DTO.AIX
{
    public class LanguageContentSectionDto
    {
        public List<ProgrammingLanguageDto> AvailableLanguages { get; set; } = new();
        public ApiXLanguageContentDto? SelectedLanguageContent { get; set; }
    }
}