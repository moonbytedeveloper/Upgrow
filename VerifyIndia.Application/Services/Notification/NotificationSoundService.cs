using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Notification;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Notification;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Notification;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Notification
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
