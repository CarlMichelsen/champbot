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
    public async Task<ServiceResponse<ReminderDto>> AddReminder(CreateReminderDto createReminder)
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
            var err = Enum.GetName(addResult.Error!.Type)!;
            return new ServiceResponse<ReminderDto>(err);
        }

        var mappedResult = EventMapper.MapReminder(addResult.Unwrap());
        if (mappedResult.IsError)
        {
            var err = Enum.GetName(mappedResult.Error!.Type)!;
            return new ServiceResponse<ReminderDto>(err);
        }

        return new ServiceResponse<ReminderDto>(mappedResult.Unwrap());
    }

    public async Task<ServiceResponse<ReminderDto>> EditReminder(EditReminderDto editReminder)
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
            var err = Enum.GetName(editResult.Error!.Type)!;
            return new ServiceResponse<ReminderDto>(err);
        }

        var mappedResult = EventMapper.MapReminder(editResult.Unwrap());
        if (mappedResult.IsError)
        {
            var err = Enum.GetName(mappedResult.Error!.Type)!;
            return new ServiceResponse<ReminderDto>(err);
        }

        return new ServiceResponse<ReminderDto>(mappedResult.Unwrap());
    }

    public async Task<ServiceResponse> RemoveReminder(long eventId, long reminderId)
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
            var err = Enum.GetName(removeResult.Error!.Type)!;
            return new ServiceResponse<ReminderDto>(err);
        }

        return new ServiceResponse();
    }
}