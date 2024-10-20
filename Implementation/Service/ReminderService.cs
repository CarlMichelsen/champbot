using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Event;
using Implementation.Util;
using Interface.Accessor;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class ReminderService(
    IUserContextAccessor userContextAccessor,
    IReminderRepository reminderRepository) : IReminderService
{
    public async Task<Result<ServiceResponse<ReminderDto>>> AddReminder(CreateReminderDto createReminder)
    {
        var userResult = userContextAccessor.GetUserContext();
        if (userResult.IsError)
        {
            return new ServiceResponse<ReminderDto>("unauthorized");
        }

        var addResult = await reminderRepository.AddReminder(
            userResult.Unwrap().User.Id,
            createReminder.EventId,
            createReminder.ReminderNote,
            TimeSpan.FromMinutes(createReminder.MinutesBeforeEvent));
        
        if (addResult.IsError)
        {
            return addResult.Error!;
        }

        var mappedResult = EventMapper.MapReminder(addResult.Unwrap());
        if (mappedResult.IsError)
        {
            return mappedResult.Error!;
        }

        return new ServiceResponse<ReminderDto>(mappedResult.Unwrap());
    }

    public async Task<Result<ServiceResponse<ReminderDto>>> EditReminder(EditReminderDto editReminder)
    {
        var userResult = userContextAccessor.GetUserContext();
        if (userResult.IsError)
        {
            return new ServiceResponse<ReminderDto>("unauthorized");
        }

        var editResult = await reminderRepository.EditReminder(
            userResult.Unwrap().User.Id,
            editReminder.EventId,
            editReminder.ReminderId,
            editReminder.ReminderNote,
            TimeSpan.FromMinutes(editReminder.MinutesBeforeEvent));
        
        if (editResult.IsError)
        {
            return editResult.Error!;
        }

        var mappedResult = EventMapper.MapReminder(editResult.Unwrap());
        if (mappedResult.IsError)
        {
            return mappedResult.Error!;
        }

        return new ServiceResponse<ReminderDto>(mappedResult.Unwrap());
    }

    public async Task<Result<ServiceResponse>> RemoveReminder(long eventId, long reminderId)
    {
        var userResult = userContextAccessor.GetUserContext();
        if (userResult.IsError)
        {
            return new ServiceResponse("unauthorized");
        }

        var removeResult = await reminderRepository.RemoveReminder(
            userResult.Unwrap().User.Id,
            eventId,
            reminderId);
        
        if (removeResult.IsError)
        {
            return removeResult.Error!;
        }

        return new ServiceResponse();
    }
}