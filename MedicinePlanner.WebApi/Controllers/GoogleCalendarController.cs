using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using MedicinePlanner.WebApi.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleCalendarController : ControllerBase
    {
        private readonly IOptions<GoogleSecretsOptions> _options;
        public GoogleCalendarController(IOptions<GoogleSecretsOptions> options)
        {
            _options = options;
        }

        [HttpGet("GetEvents")]
        public async Task<ActionResult<Events>> GetEvents()
        {
            string[] Scopes = { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents };
            string ApplicationName = "MedicinePlanner";

            ClientSecrets clientSecrets = new ClientSecrets
            {
                ClientId = _options.Value.clientId,
                ClientSecret = _options.Value.clientSecret
            };
            UserCredential credential;
            using (var stream = new FileStream("F:\\JULIA\\University\\Програмування\\C #\\MedicinePlanner\\client_secret.json", 
                FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                   GoogleClientSecrets.Load(stream).Secrets,
                   Scopes,
                   "user",
                   CancellationToken.None).Result;

            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            Event newEvent = new Event()
            {
                Summary = "Test from MedicinePlanner",
                Location = "800 Howard St., San Francisco, CA 94103",
                Description = "A chance to hear more about Google's developer products.",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2020-11-04T00:00:00-00:00"),
                    TimeZone = "UTC",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2020-11-04T01:00:00-00:00"),
                    TimeZone = "UTC",
                },
                //Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=2" },
                Attendees = new EventAttendee[] {
        new EventAttendee() { Email = "lpage@example.com" },
        new EventAttendee() { Email = "sbrin@example.com" },
    },
                Reminders = new Event.RemindersData()
                {
                    UseDefault = false,
                    Overrides = new EventReminder[] {
            new EventReminder() { Method = "email", Minutes = 24 * 60 }
            //,new EventReminder() { Method = "sms", Minutes = 10 },
        }
                }
            };

            String calendarId = "primary";
            EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
            Event createdEvent = request.Execute();
            
            // Define parameters of request.
            EventsResource.ListRequest request1 = service.Events.List("primary");
            request1.TimeMin = DateTime.Now;
            request1.ShowDeleted = false;
            request1.SingleEvents = true;
            request1.MaxResults = 10;
            request1.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request1.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                return Ok(events);
            }
            else
            {
                return Ok("No upcoming events found.");
            }
            
        }
    }
}
