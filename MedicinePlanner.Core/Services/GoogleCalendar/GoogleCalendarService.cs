using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Core.Shared;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicinePlanner.Core.Resources;
using MedicinePlanner.Data.Enums;

namespace MedicinePlanner.Core.Services.GoogleCalendar
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private DateTimeOffset TimeOfLastMeal;

        private readonly IUserService _userService;
        private readonly IMedicineScheduleService _medicineScheduleService;
        private readonly IFoodScheduleService _foodScheduleService;
        public GoogleCalendarService(IUserService userService, IMedicineScheduleService medicineScheduleService,
           IFoodScheduleService foodScheduleService)
        {
            _userService = userService;
            _medicineScheduleService = medicineScheduleService;
            _foodScheduleService = foodScheduleService;
        }

        public async Task SetEvents(Guid userId, string accessToken)
        {
            string ApplicationName = "MedicinePlanner";
            
            GoogleCredential cred = GoogleCredential.FromAccessToken(accessToken);

            //new calendar service
            CalendarService service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred,
                ApplicationName = ApplicationName,
            });

            //get user and clear their MP calendar
            User user = await _userService.GetByIdAsync(userId);
            if (user.Calendar != null)
            {
                try
                {
                    await DeleteCalendar(service, user.Calendar);
                }
                catch (Exception ex)
                {
                    
                }
            }
            user = await UpdateUserCalendar(user, service);

            IEnumerable<MedicineSchedule> medicineSchedules = await _medicineScheduleService.GetAllByUserIdAsync(user.Id);

            List<FoodSchedule> allFoodSchedules =
                (await _foodScheduleService.GetAllByMedicineScheduleIdRangeAsync(medicineSchedules.ToArray()))
                .ToList();

            List<FoodSchedule> foodSchedulesToAdd = GetDistinctFoodSchedulesAsync(allFoodSchedules);

            //generate days
            List<Day> days = new List<Day>();
            foreach (FoodSchedule foodSched in foodSchedulesToAdd)
            {
                List<FoodSchedule> foodSchedulesForDay =
                    allFoodSchedules.Where(fs => fs.Date.Date == foodSched.Date.Date).ToList();
                days.Add(GenerateDay(foodSchedulesForDay));
            }

            await AddEvents(service, user.Calendar, days);
        }

        private async Task AddEvents(CalendarService service, string calendarId, List<Day> days)
        {
            BatchRequest req = new BatchRequest(service);

            foreach (Day day in days)
            {
                foreach (Take take in day.Takes)
                {
                    req.Queue<Event>(service.Events.Insert(
                            new Event
                            {
                                Summary = take.Description,
                                Start = new EventDateTime()
                                {
                                    DateTime = take.TimeFrom.DateTime
                                },
                                End = new EventDateTime()
                                {
                                    DateTime = take.TimeTo.DateTime
                                },
                                Reminders = new Event.RemindersData()
                                {
                                    UseDefault = false,
                                    Overrides = new []
                                    {
                                        new EventReminder() { Method = "popup", Minutes = 5 }
                                    }
                                }
                            }, calendarId),
                        (content, error, i, message) => { });
                }
            }
            await req.ExecuteAsync();
        }

        private async Task DeleteCalendar(CalendarService service, string calendarId)
        {
            await service.Calendars.Delete(calendarId).ExecuteAsync();
        }

        private Day GenerateDay(List<FoodSchedule> foodSchedules)
        {
            Day day = new Day
            {
                Date = foodSchedules.First().Date.Date
            };
            List<Take> mealsTakes = GenerateMealTakes(foodSchedules.First());
            List<Take> allTakes = GenerateAllTakes(foodSchedules, mealsTakes);
            day.Takes = allTakes;

            return day;
        }

        private List<Take> GenerateAllTakes(List<FoodSchedule> foodSchedules, List<Take> mealsTakes)
        {
            List<Take> allTakes = new List<Take>();
            foreach (FoodSchedule foodSched in foodSchedules)
            {
                Medicine med = foodSched.MedicineSchedule.Medicine;

                List<Take> medicineTakes = GenerateMedicineTakes(med, mealsTakes, foodSched);
                allTakes.AddRange(medicineTakes);
            }
            allTakes.AddRange(mealsTakes);

            return allTakes;
        }

        private List<Take> GenerateMedicineTakes(Medicine medicine, List<Take> mealsTakes, FoodSchedule foodSchedule)
        {
            List<Take> medicineTakes = new List<Take>();

            if (medicine.NumberOfTakes == 1)
            {
                switch (medicine.FoodRelationId)
                {
                    case FoodRelationType.RegardlessOfMeal:
                        medicineTakes.Add(new Take()
                        {
                            TimeFrom = foodSchedule.TimeOfFirstMeal.AddMinutes(-10),
                            TimeTo = foodSchedule.TimeOfFirstMeal.AddMinutes(-8),
                            Description = GetDescription(medicine)
                        });
                        return medicineTakes;
                    case FoodRelationType.DuringMeal:
                        mealsTakes.Find(meal => meal.TimeFrom.Hour == foodSchedule.TimeOfFirstMeal.Hour).Description +=
                            "; " + GetDescription(medicine);
                        return medicineTakes;
                    case FoodRelationType.BeforeFood:
                        medicineTakes.Add(new Take()
                        {
                            TimeFrom = foodSchedule.TimeOfFirstMeal.AddMinutes(-medicine.FoodInterval ?? default),
                            TimeTo = foodSchedule.TimeOfFirstMeal.AddMinutes(-medicine.FoodInterval + 2 ?? default(int) + 2),
                            Description = GetDescription(medicine)
                        });
                        return medicineTakes;
                    case FoodRelationType.AfterFood:
                        medicineTakes.Add(new Take()
                        {
                            TimeFrom = foodSchedule.TimeOfFirstMeal.AddMinutes(medicine.FoodInterval ?? default),
                            TimeTo = foodSchedule.TimeOfFirstMeal.AddMinutes(medicine.FoodInterval + 2 ?? default(int) + 2),
                            Description = GetDescription(medicine)
                        });
                        return medicineTakes;
                }
            }

            TimeOfLastMeal = SetTimeOfLastMeal(foodSchedule);

            double intervalBetweenTakes = GetInterval(TimeOfLastMeal, foodSchedule.TimeOfFirstMeal,
                medicine.NumberOfTakes);

            if (medicine.FoodRelationId == FoodRelationType.RegardlessOfMeal)
            {
                medicineTakes.Add(new Take()
                {
                    TimeFrom = foodSchedule.TimeOfFirstMeal,
                    TimeTo = foodSchedule.TimeOfFirstMeal.AddMinutes(2),
                    Description = GetDescription(medicine)
                });

                DateTimeOffset timeToAdd = medicineTakes.First().TimeFrom;
                for (int i = 1; i < medicine.NumberOfTakes; i++)
                {
                    timeToAdd = timeToAdd.AddMinutes(intervalBetweenTakes);
                    medicineTakes.Add(new Take()
                    {
                        TimeFrom = timeToAdd,
                        TimeTo = timeToAdd.AddMinutes(2),
                        Description = GetDescription(medicine)
                    });
                }
                return medicineTakes;
            }

            if (medicine.FoodRelationId == FoodRelationType.DuringMeal)
            {
                Take firstMeal = mealsTakes.Find(meal => meal.TimeFrom.Hour == foodSchedule.TimeOfFirstMeal.Hour);
                firstMeal.Description += "; " + GetDescription(medicine);

                DateTimeOffset timeToAdd = firstMeal.TimeFrom;
                for (int i = 1; i < medicine.NumberOfTakes; i++)
                {
                    timeToAdd = timeToAdd.AddMinutes(intervalBetweenTakes);
                    Take nearestMeal = GetNearestMeal(mealsTakes, timeToAdd);
                    mealsTakes.Find(meal => meal.TimeFrom == nearestMeal.TimeFrom).Description +=
                        "; " + GetDescription(medicine);
                }
                return medicineTakes;
            }

            if (medicine.FoodRelationId == FoodRelationType.BeforeFood)
            {
                medicineTakes.Add(new Take()
                {
                    TimeFrom = foodSchedule.TimeOfFirstMeal.AddMinutes(-medicine.FoodInterval ?? default),
                    TimeTo = foodSchedule.TimeOfFirstMeal.AddMinutes(-medicine.FoodInterval ?? default).AddMinutes(2),
                    Description = GetDescription(medicine)
                });

                DateTimeOffset timeToAdd = medicineTakes.First().TimeFrom;
                for (int i = 1; i < Math.Min(medicine.NumberOfTakes, foodSchedule.NumberOfMeals); i++)
                {
                    timeToAdd = timeToAdd.AddMinutes(intervalBetweenTakes);
                    Take nearestMeal = GetNearestMeal(mealsTakes, timeToAdd);
                    medicineTakes.Add(new Take()
                    {
                        TimeFrom = nearestMeal.TimeFrom.AddMinutes(-medicine.FoodInterval ?? default),
                        TimeTo = nearestMeal.TimeFrom.AddMinutes(-medicine.FoodInterval ?? default).AddMinutes(2),
                        Description = GetDescription(medicine)
                    });
                }
                return medicineTakes;
            }

            if (medicine.FoodRelationId == FoodRelationType.AfterFood)
            {
                medicineTakes.Add(new Take()
                {
                    TimeFrom = foodSchedule.TimeOfFirstMeal.AddMinutes(medicine.FoodInterval ?? default),
                    TimeTo = foodSchedule.TimeOfFirstMeal.AddMinutes(medicine.FoodInterval ?? default).AddMinutes(2),
                    Description = GetDescription(medicine)
                });

                DateTimeOffset timeToAdd = medicineTakes.First().TimeFrom;
                for (int i = 1; i < Math.Min(medicine.NumberOfTakes, foodSchedule.NumberOfMeals); i++)
                {
                    timeToAdd = timeToAdd.AddMinutes(intervalBetweenTakes);
                    Take nearestMeal = GetNearestMeal(mealsTakes, timeToAdd);
                    medicineTakes.Add(new Take()
                    {
                        TimeFrom = nearestMeal.TimeFrom.AddMinutes(medicine.FoodInterval ?? default),
                        TimeTo = nearestMeal.TimeFrom.AddMinutes(medicine.FoodInterval ?? default).AddMinutes(2),
                        Description = GetDescription(medicine)
                    });
                }
                return medicineTakes;
            }

            return medicineTakes;
        }

        private List<Take> GenerateMealTakes(FoodSchedule foodSchedule)
        {
            TimeOfLastMeal = SetTimeOfLastMeal(foodSchedule);

            List<Take> mealsForDay = new List<Take>()
            {
                new Take()
                {
                    TimeFrom = foodSchedule.TimeOfFirstMeal,
                    TimeTo = foodSchedule.TimeOfFirstMeal.AddMinutes(30),
                    Description = MessagesResource.MEAL_TIME
                }
            };

            double intervalBetweenFood = GetInterval(TimeOfLastMeal, foodSchedule.TimeOfFirstMeal, foodSchedule.NumberOfMeals);


            DateTimeOffset timeToAdd = foodSchedule.TimeOfFirstMeal;
            for (int i = 1; i < foodSchedule.NumberOfMeals; i++)
            {
                timeToAdd = timeToAdd.AddMinutes(intervalBetweenFood);
                mealsForDay.Add(new Take()
                {
                    TimeFrom = timeToAdd,
                    TimeTo = timeToAdd.AddMinutes(30),
                    Description = MessagesResource.MEAL_TIME
                });
            }

            return mealsForDay;
        }

        private Take GetNearestMeal(List<Take> meals, DateTimeOffset medicineTakeTime)
        {
            Take nearestMeal = meals.First();

            long diff = DateTime.MaxValue.Ticks;
            foreach (Take meal in meals)
            {
                long timeDiff = Math.Abs(meal.TimeFrom.Ticks - medicineTakeTime.Ticks);
                if (diff > timeDiff)
                {
                    diff = timeDiff;
                    nearestMeal = meal;
                }
            }

            return nearestMeal;
        }

        private string GetDescription(Medicine medicine)
        {
            return string.Format(MessagesResource.TAKE_DESCRIPTION, medicine.Name, medicine.PharmaceuticalForm.Name,
                medicine.Dosage, medicine.FoodRelation.Name);
        }

        private double GetInterval(DateTimeOffset lastHour, DateTimeOffset firstHour, int numberOfTakes)
        {
            return Math.Round((double)(lastHour.Hour - firstHour.Hour) / (numberOfTakes - 1), 1) * 60;
        }

        private List<FoodSchedule> GetDistinctFoodSchedulesAsync(List<FoodSchedule> foodSchedules)
        {
            List<FoodSchedule> distinctFoodSchedules = foodSchedules.GroupBy(fs => fs.Date.Date)
                .Select(fs => fs.First()).ToList();

            return distinctFoodSchedules;
        }

        private async Task<User> UpdateUserCalendar(User user, CalendarService service)
        {
            Calendar primaryCalendar = await service.Calendars.Get("primary").ExecuteAsync();
            Calendar calendarNew = new Calendar
            {
                Summary = "MedicinePlanner",
                TimeZone = primaryCalendar.TimeZone
            };
            Calendar createdCalendar = await service.Calendars.Insert(calendarNew).ExecuteAsync();
            user.Calendar = createdCalendar.Id;
            return await _userService.UpdateAsync(user);
        }

        private DateTimeOffset SetTimeOfLastMeal(FoodSchedule foodSchedule)
        {
            return foodSchedule.TimeOfFirstMeal.AddHours(12).Hour <= 21
                ? foodSchedule.TimeOfFirstMeal.AddHours(12)
                : new DateTimeOffset(foodSchedule.TimeOfFirstMeal.Year, foodSchedule.TimeOfFirstMeal.Month,
                    foodSchedule.TimeOfFirstMeal.Day,
                    20, 0, 0, new TimeSpan(0, 0, 0));
        }
    }
}