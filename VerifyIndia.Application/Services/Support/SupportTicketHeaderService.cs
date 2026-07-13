using AutoMapper;
using System.Linq.Expressions;
using VerifyIndia.Application.Commands.Support;
using VerifyIndia.Application.DTO.Support;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Support;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Support
{
    public class SupportTicketHeaderService
        : MasterServiceBase<Support_TicketHeader, SupportTicketDto, SupportTicketHeaderCommand>,
          ISupportTicketHeaderService
    {
        private readonly IMasterRepository<Master_Employee> _employeeRepository;
        private readonly IMasterRepository<Support_TicketCategory> _ticketCategoryRepository;
        private readonly IMasterRepository<Support_TicketLine> _ticketLineRepository;
        private readonly IMasterRepository<Support_TicketLineAttachment> _attachmentRepository;
        private readonly IMasterRepository<Tenant> _tenantrepository;
        private readonly IMasterRepository<Master_Customer> _customerRepository;

        public SupportTicketHeaderService(
            IMapper mapper,
            IMasterRepository<Support_TicketHeader> repository,
            IMasterRepository<Master_Employee> employeeRepository,
            IMasterRepository<Support_TicketCategory> ticketCategoryRepository,
            IMasterRepository<Support_TicketLine> ticketLineRepository,
            IMasterRepository<Support_TicketLineAttachment> attachmentRepository,
            IMasterRepository<Master_Customer> customerRepository,
            IMasterRepository<Tenant> tenantRepository)
            : base(repository, mapper)
        {
            _employeeRepository = employeeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _ticketLineRepository = ticketLineRepository;
            _attachmentRepository = attachmentRepository;
            _tenantrepository = tenantRepository;
            _customerRepository = customerRepository;
        }

        protected override async Task<bool> IsDuplicateAsync(SupportTicketHeaderCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.TicketNumber!.ToLower().Trim() == command.TicketNumber!.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Support_TicketHeader, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.TicketNumber != null && x.TicketNumber.ToLower().Contains(searchTerm)) ||
                (x.TicketCategoryUUID != null && x.TicketCategoryUUID.ToLower().Contains(searchTerm)) ||
                (x.UserType != null && x.UserType.ToLower().Contains(searchTerm)) ||
                (x.UserUUID != null && x.UserUUID.ToLower().Contains(searchTerm)) ||
                (x.AssigneeUUID != null && x.AssigneeUUID.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<Support_TicketHeader>, IOrderedQueryable<Support_TicketHeader>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "ticketnumber" => q => isAsc ? q.OrderBy(x => x.TicketNumber) : q.OrderByDescending(x => x.TicketNumber),
                "category" => q => isAsc ? q.OrderBy(x => x.TicketCategoryUUID) : q.OrderByDescending(x => x.TicketCategoryUUID),
                "usertype" => q => isAsc ? q.OrderBy(x => x.UserType) : q.OrderByDescending(x => x.UserType),
                "createdat" => q => isAsc ? q.OrderBy(x => x.CreatedAt) : q.OrderByDescending(x => x.CreatedAt),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<PagedResult<SupportTicketDto>> GetPagedAsync(
            DataTableRequest request,
            string? userType = null,
            string? categoryUuid = null,
            string? assigneeUuid = null, bool? isActive = null, string? currentUserUuid = null)
        {
            Func<IQueryable<Support_TicketHeader>, IQueryable<Support_TicketHeader>>? queryModifier = q =>
            {
                if (!string.IsNullOrWhiteSpace(userType))
                {
                    var value = userType.Trim().ToLower();
                    q = q.Where(x => x.UserType != null && x.UserType.ToLower() == value);
                }

                if (!string.IsNullOrWhiteSpace(categoryUuid))
                {
                    var value = categoryUuid.Trim().ToLower();
                    q = q.Where(x => x.TicketCategoryUUID != null && x.TicketCategoryUUID.ToLower() == value);
                }

                if (!string.IsNullOrWhiteSpace(assigneeUuid))
                {
                    var value = assigneeUuid.Trim().ToLower();
                    q = q.Where(x => x.AssigneeUUID != null && x.AssigneeUUID.ToLower() == value);
                }

                if (isActive.HasValue)
                {
                    q = q.Where(x => x.IsActive == isActive.Value);
                }
                if (!string.IsNullOrWhiteSpace(currentUserUuid))
                {
                    var value = currentUserUuid.Trim().ToLower();

                    q = q.Where(x =>
                        (x.UserUUID != null && x.UserUUID.ToLower() == value) ||
                        (x.AssigneeUUID != null && x.AssigneeUUID.ToLower() == value)
                    );
                }
                return q;
            };

            return await base.GetPagedAsync(request, queryModifier);
        }

        public async Task<List<SupportTicketAssignableEmployeeDto>> GetAssignableEmployeesAsync(string ticketCategoryUuid)
        {
            if (string.IsNullOrWhiteSpace(ticketCategoryUuid))
                return new List<SupportTicketAssignableEmployeeDto>();

            var category = await _ticketCategoryRepository.GetByUuidAsync(ticketCategoryUuid);
            var designationUuid = category?.DesignationUUID;

            if (string.IsNullOrWhiteSpace(designationUuid))
                return new List<SupportTicketAssignableEmployeeDto>();

            var employees = await _employeeRepository.GetAllActiveAsync();

            return employees
                .Where(x =>
                    x.IsLoginAllowed &&
                    !string.IsNullOrWhiteSpace(x.DepartmentUUID) &&
                    !string.IsNullOrWhiteSpace(x.DesignationUUID) &&
                    x.DesignationUUID.Trim().Equals(designationUuid.Trim(), StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x => new SupportTicketAssignableEmployeeDto
                {
                    UUID = x.UUID,
                    EmployeeCode = x.EmployeeCode ?? string.Empty,
                    EmployeeName = $"{x.FirstName} {x.LastName}".Trim()
                })
                .ToList();
        }

        public async Task AssignTicketAsync(string ticketUuid, string assigneeUuid, string userUuid, string ip)
        {
            if (string.IsNullOrWhiteSpace(ticketUuid))
                throw new Exception("Invalid ticket.");

            if (string.IsNullOrWhiteSpace(assigneeUuid))
                throw new Exception("Please select employee to assign.");

            var ticket = await _repository.GetByUuidAsync(ticketUuid);
            if (ticket == null)
                throw new Exception("Ticket not found.");

            ticket.AssigneeUUID = assigneeUuid;
            await _repository.UpdateAsync(ticket);
        }
        public async Task<List<TicketConversationDto>> GetTicketConversationAsync(string ticketUuid)
        {
            if (string.IsNullOrWhiteSpace(ticketUuid))
                return new List<TicketConversationDto>();
            var ticket = await _repository.GetByUuidAsync(ticketUuid);
            var lines = await _ticketLineRepository.FindAllAsync(x =>
                x.HeaderUUID == ticketUuid && x.IsActive);

            var orderedLines = lines.OrderByDescending(x => x.CreatedAt).ToList();

            if (!orderedLines.Any())
                return new List<TicketConversationDto>();

            var lineUuids = orderedLines.Select(x => x.UUID).ToList();
            var userUuids = orderedLines.Select(x => x.UserUUID).Distinct().ToList();

            var attachments = await _attachmentRepository.FindAllAsync(x =>
                lineUuids.Contains(x.LineUUID) && x.IsActive);
            var employees = await _employeeRepository.FindAllAsync(x => userUuids.Contains(x.UUID));
            var customers = await _customerRepository.FindAllAsync(x => userUuids.Contains(x.UUID));
            var whiteLabels = await _tenantrepository.FindAllAsync(x => userUuids.Contains(x.UUID));
            var customerDict = customers.ToDictionary(x => x.UUID);
            var tenantDict = whiteLabels.ToDictionary(x => x.UUID);
            var employeeDict = employees.ToDictionary(x => x.UUID);

            var result = orderedLines.Select(line =>
            {
                string name = "Unknown";
                string userType = "Unknown";

                if (customerDict.TryGetValue(line.UserUUID, out var cust))
                {
                    name = $"{cust.FName} {cust.LName}";
                    userType = "Customer";
                }
                else if (tenantDict.TryGetValue(line.UserUUID, out var wl))
                {
                    name = wl.TenantName;
                    userType = "WhiteLabel";
                }
                else if (employeeDict.TryGetValue(line.UserUUID, out var emp))
                {
                    name = $"{emp.FirstName} {emp.LastName}";
                    userType = "Employee";
                }

                return new TicketConversationDto
                {
                    LineUUID = line.UUID,
                    UserUUID = line.UserUUID,
                    UserName = name,
                    CreatedAt = line.CreatedAt,
                    Message = line.Message,
                    UserType = userType,

                    Url = attachments
                        .Where(a => a.LineUUID == line.UUID)
                        .Select(a => a.URL)
                        .ToList()
                };
            }).ToList();

            return result;
        }
        public async Task AddReplyAsync(
    string ticketUuid,
    string message,
    List<string> attachments,
    string userUuid,
    string ip)
        {
            if (string.IsNullOrWhiteSpace(ticketUuid))
                throw new Exception("Invalid ticket.");

            var ticket = await _repository.GetByUuidAsync(ticketUuid);

            if (ticket == null)
                throw new Exception("Ticket not found.");

            var wasClosed = !ticket.IsActive;
            ticket.UpdatedOn = Utils.GetCurrentUtcTime();
            ticket.IsActive = true;
            if (wasClosed)
            {
                ticket.Otp = GenerateOTP();
            }

            await _repository.UpdateAsync(ticket);

            var lineUuid = Utils.GetUUID();

            var line = new Support_TicketLine
            {
                UUID = lineUuid,
                HeaderUUID = ticketUuid,
                UserUUID = userUuid,
                Message = message,
                CreatedAt = Utils.GetCurrentUtcTime(),
                IsActive = true
            };

            await _ticketLineRepository.AddAsync(line);
            if (attachments != null && attachments.Any())
            {
                foreach (var file in attachments)
                {
                    await _attachmentRepository.AddAsync(new Support_TicketLineAttachment
                    {
                        UUID = Utils.GetUUID(),
                        LineUUID = lineUuid,
                        URL = file,
                        IsActive = true
                    });
                }
            }
        }

        public async Task SaveAsync(
    SupportTicketHeaderCommand header,
    string message,
    List<string>? attachments,
    string userUuid,
    string ip)
        {

            header.UUID ??= Utils.GetUUID();
            header.Otp = GenerateOTP();
            header.CreatedAt = Utils.GetCurrentUtcTime();
            header.IsActive = true;

            await _repository.AddAsync(_mapper.Map<Support_TicketHeader>(header));

            if (!string.IsNullOrWhiteSpace(message))
            {
                var lineUuid = Utils.GetUUID();

                var line = new Support_TicketLine
                {
                    UUID = lineUuid,
                    HeaderUUID = header.UUID,
                    UserUUID = userUuid,
                    Message = message,
                    CreatedAt = Utils.GetCurrentUtcTime(),
                    IsActive = true
                };

                await _ticketLineRepository.AddAsync(line);

                if (attachments != null && attachments.Any())
                {
                    foreach (var file in attachments)
                    {
                        await _attachmentRepository.AddAsync(new Support_TicketLineAttachment
                        {
                            UUID = Utils.GetUUID(),
                            LineUUID = lineUuid,
                            URL = file,
                            IsActive = true
                        });
                    }
                }
            }
        }
        //public async Task<string> RegenerateOtpAsync(string ticketUuid)
        //{
        //    if (string.IsNullOrWhiteSpace(ticketUuid))
        //        throw new Exception("Invalid ticket.");

        //    var ticket = await _repository.GetByUuidAsync(ticketUuid);

        //    if (ticket == null)
        //        throw new Exception("Ticket not found.");

        //    var newOtp = GenerateOTP();

        //    ticket.Otp = newOtp;

        //    await _repository.UpdateAsync(ticket);

        //    return newOtp;
        //}
        private string GenerateOTP(int length = 6)
        {
            var random = new Random();
            var otp = "";

            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10);
            }

            return otp;
        }
    }
}