using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Notification;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.IServices.Notification;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Notification
{
    public class CustomerNotificationPreferenceService : MasterServiceBase<
        CustomerNotificationPreference,
        CustomerNotificationPreferenceDto,
        CustomerNotificationPreferenceCommand>,
      ICustomerNotificationPreferenceService
    {
        public CustomerNotificationPreferenceService(IMasterRepository<CustomerNotificationPreference> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        protected override Expression<Func<CustomerNotificationPreference, bool>>? BuildSearchFilter(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task<bool> IsDuplicateAsync(CustomerNotificationPreferenceCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.CustomerUUID == command.CustomerUUID &&
                x.UUID != command.UUID);
        }
        protected override async Task CreateAsync(
           CustomerNotificationPreferenceCommand command,
           string userUuid,
           string ip,
           bool saveChanges = true)
        {
            command.UUID = Utils.GetUUID();
            var entity = _mapper.Map<CustomerNotificationPreference>(command);
            entity.CreatedOn = DateTimeOffset.UtcNow;
            await _repository.AddAsync(entity, saveChanges);
        }

        protected override async Task UpdateAsync(
            CustomerNotificationPreferenceCommand command,
            string userUuid,
            string ip,
            bool saveChanges = true)
        {
            var entity = await _repository.GetByUuidAsync(command.UUID!)
                ?? throw new Exception("Notification preference not found.");

            _mapper.Map(command, entity);
            entity.UpdatedOn = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(entity, saveChanges);
        }
        public async Task<CustomerNotificationPreferenceDto?> GetByCustomerUuidAsync(string customerUuid)
        {
            var result = await _repository.FindAllAsync(x => x.CustomerUUID == customerUuid);

            var entity = result.FirstOrDefault();

            if (entity == null)
                return null;

            return _mapper.Map<CustomerNotificationPreferenceDto>(entity);
        }
    }
}
