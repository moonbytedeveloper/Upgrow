using System.Threading.Tasks;

public interface IVerificationRequestLogService
{
    Task LogAsync(string payload);
}