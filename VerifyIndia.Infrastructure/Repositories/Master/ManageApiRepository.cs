using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.Interfaces;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class ManageApiRepository : IManageApiRepository
    {
        private readonly AppDbContext _context;

        public ManageApiRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveApiConfigurationAsync(
    SaveApiConfigurationCommand command,
    string userUuid,
    string ipAddress)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var api = await SaveApiAsync(command);

                await SaveEndpointsAsync(
                    api.UUID,
                    command);

                await SaveComponentsAsync(
                    api.UUID,
                    command);

                await SaveProviderApisAsync(
                    api.UUID,
                    command);

                await SaveProviderMappingsAsync(
                    api.UUID,
                    command);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<Master_Api> SaveApiAsync(
    SaveApiConfigurationCommand command)
        {
            Master_Api entity;

            if (string.IsNullOrWhiteSpace(command.Api.UUID))
            {
                entity = new Master_Api
                {
                    UUID = Utils.GetUUID(),
                    ApiName = command.Api.ApiName,
                    Code = command.Api.ApiName
                        .ToUpper()
                        .Replace(" ", "_"),
                    ApiCategoryUUID = command.Api.ApiCategoryUUID,
                    ShortDescription = command.Api.ShortDescription,
                    VerificationDocument = command.Api.VerificationDocument,
                    DisplayOrder = command.Api.DisplayOrder,
                    IsConsentBased = command.Api.IsConsentBased,
                    IsReminderRequired = command.Api.IsReminderRequired,
                    IsProviderSwitchable = command.Api.IsProviderSwitchable,
                    IsMultipleEndPoint = command.Api.IsMultipleEndPoint,
                    IsActive = true
                };

                await _context.Master_Api.AddAsync(entity);
            }
            else
            {
                entity = await _context.Master_Api
                    .FirstAsync(x => x.UUID == command.Api.UUID);

                entity.ApiName = command.Api.ApiName;
                entity.ApiCategoryUUID = command.Api.ApiCategoryUUID;
                entity.ShortDescription = command.Api.ShortDescription;
                entity.VerificationDocument = command.Api.VerificationDocument;
                entity.DisplayOrder = command.Api.DisplayOrder;
                entity.IsConsentBased = command.Api.IsConsentBased;
                entity.IsReminderRequired = command.Api.IsReminderRequired;
                entity.IsProviderSwitchable = command.Api.IsProviderSwitchable;
                entity.IsMultipleEndPoint = command.Api.IsMultipleEndPoint;
            }

            return entity;
        }

        private async Task SaveEndpointsAsync(
    string apiUuid,
    SaveApiConfigurationCommand command)
        {
            var endpoints = await _context.Api_Endpoint
                .Where(x => x.ApiUUID == apiUuid)
                .ToListAsync();

            var endpoint1 =
                endpoints.FirstOrDefault(x => x.Sequence == 1);

            if (endpoint1 == null)
            {
                endpoint1 = new Api_Endpoint
                {
                    UUID = Utils.GetUUID(),
                    ApiUUID = apiUuid,
                    Sequence = 1,
                    IsActive = true
                };

                await _context.Api_Endpoint.AddAsync(endpoint1);
            }

            endpoint1.EndpointUrl =
                command.ApiEndpoint.EndpointUrl;

            endpoint1.HttpMethod =
                command.ApiEndpoint.HttpMethod;

            endpoint1.IsActive = true;

            if (command.Api.IsMultipleEndPoint)
            {
                var endpoint2 =
                    endpoints.FirstOrDefault(x => x.Sequence == 2);

                if (endpoint2 == null)
                {
                    endpoint2 = new Api_Endpoint
                    {
                        UUID = Utils.GetUUID(),
                        ApiUUID = apiUuid,
                        Sequence = 2,
                        IsActive = true
                    };

                    await _context.Api_Endpoint.AddAsync(endpoint2);
                }

                endpoint2.EndpointUrl =
                    command.SecondApiEndpoint.EndpointUrl;

                endpoint2.HttpMethod =
                    command.SecondApiEndpoint.HttpMethod;

                endpoint2.IsActive = true;
            }
            else
            {
                var endpoint2 =
                    endpoints.FirstOrDefault(x => x.Sequence == 2);

                if (endpoint2 != null)
                {
                    endpoint2.IsActive = false;
                }
            }
        }

        private async Task SaveComponentsAsync(
    string apiUuid,
    SaveApiConfigurationCommand command)
        {
            var mappings = await _context.Api_ProviderComponentMapping
                .Where(x => x.ApiUUID == apiUuid)
                .ToListAsync();

            var first =
                mappings.FirstOrDefault(x => x.Sequence == 1);

            if (first == null)
            {
                first = new Api_ProviderComponentMapping
                {
                    UUID = Utils.GetUUID(),
                    ApiUUID = apiUuid,
                    Sequence = 1,
                    IsActive = true
                };

                await _context.Api_ProviderComponentMapping
                    .AddAsync(first);
            }

            first.ComponentUUID =
                command.SelectedComponentUUID;

            first.IsActive = true;

            if (command.Api.IsMultipleEndPoint)
            {
                var second =
                    mappings.FirstOrDefault(x => x.Sequence == 2);

                if (second == null)
                {
                    second = new Api_ProviderComponentMapping
                    {
                        UUID = Utils.GetUUID(),
                        ApiUUID = apiUuid,
                        Sequence = 2,
                        IsActive = true
                    };

                    await _context.Api_ProviderComponentMapping
                        .AddAsync(second);
                }

                second.ComponentUUID =
                    command.SelectedSecondComponentUUID;

                second.IsActive = true;
            }
            else
            {
                var second =
                    mappings.FirstOrDefault(x => x.Sequence == 2);

                if (second != null)
                {
                    second.IsActive = false;
                }
            }
        }

        private async Task SaveProviderApisAsync(
    string apiUuid,
    SaveApiConfigurationCommand command)
        {
            var existingProviderApis = await _context.ProviderApis
                .Where(x => x.ApiUUID == apiUuid)
                .ToListAsync();

            var lookup = existingProviderApis
                .ToDictionary(x => x.ProviderUUID);

            var selectedProviders = command.ProviderMappings
                .Where(x => x.SupportsApi)
                .Select(x => x.ProviderUUID!)
                .ToHashSet();

            foreach (var provider in command.ProviderMappings
                         .Where(x => x.SupportsApi))
            {
                if (lookup.TryGetValue(
                        provider.ProviderUUID!,
                        out var existing))
                {
                    existing.IsActive = true;
                }
                else
                {
                    await _context.ProviderApis.AddAsync(
                        new Provider_Apis
                        {
                            UUID = Utils.GetUUID(),
                            ApiUUID = apiUuid,
                            ProviderUUID = provider.ProviderUUID!,
                            IsActive = true
                        });
                }
            }

            foreach (var existing in existingProviderApis)
            {
                if (!selectedProviders.Contains(
                        existing.ProviderUUID))
                {
                    existing.IsActive = false;
                }
            }
        }

        private async Task SaveProviderMappingsAsync(
    string apiUuid,
    SaveApiConfigurationCommand command)
        {
            var existingMappings = await _context.Api_ProviderMapping
                .Where(x => x.ApiUUID == apiUuid)
                .ToListAsync();

            var lookup = existingMappings
                .ToDictionary(x => x.ProviderUUID);

            var activeProviders = command.ProviderMappings
                .Where(x => x.IsActive)
                .Select(x => x.ProviderUUID!)
                .ToHashSet();

            foreach (var provider in command.ProviderMappings
                         .Where(x => x.IsActive))
            {
                if (lookup.TryGetValue(
                        provider.ProviderUUID!,
                        out var existing))
                {
                    existing.IsActive = true;

                    // If Priority column exists in entity
                    existing.Priority = provider.Priority ?? 0;
                }
                else
                {
                    var entity = new Api_ProviderMapping
                    {
                        UUID = Utils.GetUUID(),
                        ApiUUID = apiUuid,
                        ProviderUUID = provider.ProviderUUID!,
                        IsActive = true,

                        // If Priority column exists
                        Priority = provider.Priority ?? 0
                    };

                    await _context.Api_ProviderMapping
                        .AddAsync(entity);
                }
            }

            foreach (var existing in existingMappings)
            {
                if (!activeProviders.Contains(
                        existing.ProviderUUID))
                {
                    existing.IsActive = false;
                }
            }
        }
    }
}
