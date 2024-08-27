using ToDo.Repositories.Interfaces;

public interface IConfirmationService
{
    Task ScheduleConfirmationCodeDeletion( int userId, int delayInSeconds = 120);
}
    