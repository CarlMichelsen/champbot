using Domain.Dto;
using Domain.Dto.Event;

namespace Interface.Service;

public interface IReminderService
{
    Task<ServiceResponse<ReminderDto>> AddReminder(CreateReminderDto createReminder);

    Task<ServiceResponse> RemoveReminder(long eventId, long reminderId);

    Task<ServiceResponse<ReminderDto>> EditReminder(EditReminderDto editReminder);
}