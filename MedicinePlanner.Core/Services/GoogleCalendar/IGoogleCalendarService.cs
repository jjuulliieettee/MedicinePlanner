using System;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.GoogleCalendar
{
    public interface IGoogleCalendarService
    {
        Task SetEvents(Guid userId, string accessToken);
    }
}
