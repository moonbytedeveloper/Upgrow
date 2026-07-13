namespace Upgrow.Application.IServices.Auth
{
    public interface IHmacService
    {
        string GenerateSignature(long unixTimestampSeconds, string body, string clientSecret);
        bool ValidateSignature(string providedSignature, long unixTimestampSeconds, string body, string clientSecret);

        string GenerateHash(string input);
    }
}
