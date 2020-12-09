using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicinePlanner.Core.Resources;
using MedicinePlanner.Core.Services.GoogleCalendar;

namespace MedicinePlanner.Core.Services
{
    public class FoodScheduleService : IFoodScheduleService
    {
        private readonly IFoodScheduleRepo _foodScheduleRepo;
        public FoodScheduleService(IFoodScheduleRepo foodScheduleRepo)
        {
            _foodScheduleRepo = foodScheduleRepo;
        }

        public async Task AddAsync(FoodSchedule foodSchedule, IEnumerable<MedicineSchedule> medicineSchedules)
        {
            List<FoodSchedule> foodSchedules = new List<FoodSchedule>();
            List<DateTime> dates = new List<DateTime>();
            foreach (MedicineSchedule medSched in medicineSchedules)
            {
                DateTime currentDate = new DateTime(
                    medSched.StartDate.Year, 
                    medSched.StartDate.Month,
                    medSched.StartDate.Day,
                    foodSchedule.TimeOfFirstMeal.Hour, 
                    foodSchedule.TimeOfFirstMeal.Minute,
                    foodSchedule.TimeOfFirstMeal.Second);
                for (DateTime date = currentDate; date.Date <= medSched.EndDate.Date; date = date.AddDays(1))
                {
                    foodSchedules.Add(new FoodSchedule
                    {
                        MedicineScheduleId = medSched.Id,
                        TimeOfFirstMeal = date,
                        NumberOfMeals = foodSchedule.NumberOfMeals,
                        Date = date
                    });
                    dates.Add(date);
                }
            }
            IEnumerable<FoodSchedule> newFoodSchedules = await _foodScheduleRepo.AddAsync(foodSchedules);
            await EditAllForDates(dates, medicineSchedules.First().UserId, foodSchedule, newFoodSchedules);
        }

        public async Task DeleteAsync(Guid id)
        {
            FoodSchedule foodSchedule = await _foodScheduleRepo.GetByIdAsync(id);

            if (foodSchedule == null)
            {
                throw new ApiException(MessagesResource.FOOD_SCHEDULE_NOT_FOUND);
            }

            await _foodScheduleRepo.DeleteAsync(foodSchedule);
        }

        public async Task<FoodSchedule> EditAsync(FoodSchedule foodSchedule, Guid userId)
        {
            FoodSchedule foodScheduleOld = await _foodScheduleRepo.GetByIdAsync(foodSchedule.Id);
            if (foodScheduleOld == null)
            {
                throw new ApiException(MessagesResource.FOOD_SCHEDULE_NOT_FOUND);
            }

            foodSchedule.Date = foodScheduleOld.Date;
            foodSchedule.TimeOfFirstMeal = new DateTime(
                foodScheduleOld.Date.Year,
                foodScheduleOld.Date.Month,
                foodScheduleOld.Date.Day,
                foodSchedule.TimeOfFirstMeal.Hour,
                foodSchedule.TimeOfFirstMeal.Minute,
                foodSchedule.TimeOfFirstMeal.Second);
            
            foodSchedule.MedicineScheduleId = foodScheduleOld.MedicineScheduleId;

            List<DateTime> dates = new List<DateTime>() { foodSchedule.Date };

            List<FoodSchedule> notEditableFoodSchedules = new List<FoodSchedule>() { await _foodScheduleRepo.EditAsync(foodSchedule) };

            await EditAllForDates(dates, userId, foodSchedule, notEditableFoodSchedules);

            return foodSchedule;
        }

        public async Task EditAllBasedOnFoodScheduleAsync(Guid foodScheduleId, Guid userId)
        {
            FoodSchedule foodSchedule = await _foodScheduleRepo.GetByIdAsync(foodScheduleId);
            if (foodSchedule == null)
            {
                throw new ApiException(MessagesResource.FOOD_SCHEDULE_NOT_FOUND);
            }

            IEnumerable<FoodSchedule> foodSchedules = (await _foodScheduleRepo.GetAllByMedicineScheduleIdAsync(foodSchedule.MedicineScheduleId))
                .Where(fs => fs.Date.Date >= DateTime.UtcNow.Date);

            if (!foodSchedules.Any())
            {
                throw new ApiException(MessagesResource.FOOD_SCHEDULE_NOT_FOUND);
            }

            List<DateTime> dates = new List<DateTime>();

            foreach (FoodSchedule foodSched in foodSchedules)
            {
                dates.Add(foodSched.Date);
            }

            List<FoodSchedule> notEditableFoodSchedules = new List<FoodSchedule>() { foodSchedule };

            await EditAllForDates(dates, userId, foodSchedule, notEditableFoodSchedules);
        }

        public async Task EditAllBasedOnMedicineScheduleAsync(MedicineSchedule medicineSchedule)
        {
            IEnumerable<FoodSchedule> foodSchedules = await _foodScheduleRepo.GetAllByMedicineScheduleIdAsync(medicineSchedule.Id);
            if (!foodSchedules.Any())
            {
                throw new ApiException(MessagesResource.FOOD_SCHEDULE_NOT_FOUND);
            }

            FoodSchedule foodScheduleFirst = foodSchedules.FirstOrDefault();

            await _foodScheduleRepo.DeleteAllAsync(foodSchedules);

            IEnumerable<MedicineSchedule> medicineSchedules = new List<MedicineSchedule>() { medicineSchedule };

            await AddAsync(foodScheduleFirst, medicineSchedules);
        }

        public async Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdAsync(Guid medicineScheduleId)
        {
            return (await _foodScheduleRepo.GetAllByMedicineScheduleIdAsync(medicineScheduleId))
                .Where(fs => fs.Date.Date >= DateTime.UtcNow.Date);
        }

        public async Task<FoodSchedule> GetByDateAsync(DateTime date, Guid medicineScheduleId)
        {
            return await _foodScheduleRepo.GetByDateAsync(date, medicineScheduleId);
        }

        public async Task<FoodSchedule> GetByIdAsync(Guid id)
        {
            return await _foodScheduleRepo.GetByIdAsync(id);
        }

        private async Task EditAllForDates(IEnumerable<DateTime> dates, Guid userId, FoodSchedule foodSchedule,
            IEnumerable<FoodSchedule> staticFoodSchedules)
        {

            IEnumerable<FoodSchedule> foodSchedules = (await _foodScheduleRepo.GetAllByDateRangeAndUserIdAsync(dates.ToArray(), userId))
                .Where(foodSched => staticFoodSchedules.All(fs => fs.Id != foodSched.Id));
            foreach (FoodSchedule foodSched in foodSchedules)
            {
                foodSched.TimeOfFirstMeal = new DateTime(
                    foodSched.Date.Year, 
                    foodSched.Date.Month,
                    foodSched.Date.Day, 
                    foodSchedule.TimeOfFirstMeal.Hour, 
                    foodSchedule.TimeOfFirstMeal.Minute,
                    foodSchedule.TimeOfFirstMeal.Second);

                foodSched.NumberOfMeals = foodSchedule.NumberOfMeals;
            }

            await _foodScheduleRepo.EditAllAsync(foodSchedules);
        }

        public async Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdRangeAsync(MedicineSchedule[] medicineSchedules)
        {
            return await _foodScheduleRepo.GetAllByMedicineScheduleIdRangeAsync(medicineSchedules);
        }
    }
}
