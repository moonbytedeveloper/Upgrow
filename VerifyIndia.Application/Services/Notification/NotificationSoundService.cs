using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Notification;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Notification;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Notification
{
    public class NotificationSoundService : MasterServiceBase<NotificationSound, NotificationSoundDto, NotificationSoundCommand>, INotificationSoundService
    {
        public NotificationSoundService(IMasterRepository<NotificationSound> repository, IMapper mapper)
        : base(repository, mapper) { }

        public async Task<List<NotificationSoundDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<NotificationSoundDto>>(entities);
        }
        protected override Expression<Func<NotificationSound, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.SoundName != null && x.SoundName.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(NotificationSoundCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.SoundName!.ToLower().Trim() == command.SoundName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<NotificationSound>, IOrderedQueryable<NotificationSound>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "soundname" => q => isAsc ? q.OrderBy(x => x.SoundName) : q.OrderByDescending(x => x.SoundName),
                "displayorder" => q => isAsc ? q.OrderBy(x => x.DisplayOrder) : q.OrderByDescending(x => x.DisplayOrder),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

    }
}
