using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.IServices;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services
{
    public class AppSettingService
        : IAppSettingService
    {
        private readonly IAppSettingRepository
            _appSettingRepository;

        public AppSettingService(
            IAppSettingRepository appSettingRepository)
        {
            _appSettingRepository =
                appSettingRepository;
        }

        public async Task<int> GetIntValueAsync(
            string key,
            int defaultValue)
        {
            var value =
                await _appSettingRepository
                    .GetValueAsync(
                        key);

            if (string.IsNullOrWhiteSpace(
                    value))
            {
                return defaultValue;
            }

            return int.TryParse(
                value,
                out var result)
                    ? result
                    : defaultValue;
        }

        public async Task<string> GetValueAsync(
            string key,
            string defaultValue = "")
        {
            var value =
                await _appSettingRepository
                    .GetValueAsync(
                        key);

            return string.IsNullOrWhiteSpace(
                    value)
                ? defaultValue
                : value.Trim();
        }
    }
}
