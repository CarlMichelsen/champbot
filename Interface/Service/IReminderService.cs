using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Event;

namespace Interface.Service;

public interface IReminderService
{
    Task<Result<ServiceResponse<ReminderDto>>> AddReminder(CreateReminderDto createReminder);

    Task<Result<ServiceResponse>> RemoveReminder(long eventId, long reminderId);

    Task<Result<ServiceResponse<ReminderDto>>> EditReminder(EditReminderDto editReminder);
}