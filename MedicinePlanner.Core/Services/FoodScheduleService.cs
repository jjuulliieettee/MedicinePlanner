using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<DateTimeOffset> dates = new List<DateTimeOffset>();
            foreach (MedicineSchedule medSched in medicineSchedules)
            {
                DateTimeOffset currentDate = new DateTimeOffset(medSched.StartDate.Year, medSched.StartDate.Month, medSched.StartDate.Day,
                    foodSchedule.TimeOfFirstMeal.Hour, foodSchedule.TimeOfFirstMeal.Minute, foodSchedule.TimeOfFirstMeal.Second,
                    new TimeSpan(0, 0, 0));
                for (DateTimeOffset date = currentDate; date.Date <= medSched.EndDate.Date; date = date.AddDays(1))
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
                throw new ApiException("Food schedule not found!");
            }

            await _foodScheduleRepo.DeleteAsync(foodSchedule);
        }

        public async Task DeleteAllAsync(Guid medicineScheduleId)
        {
            IEnumerable<FoodSchedule> foodSchedules = await _foodScheduleRepo.GetAllByMedicineScheduleIdAsync(medicineScheduleId);

            if (!foodSchedules.Any())
            {
                throw new ApiException("Food schedules not found!");
            }

            await _foodScheduleRepo.DeleteAllAsync(foodSchedules);
        }

        public async Task<FoodSchedule> EditAsync(FoodSchedule foodSchedule, Guid userId)
        {
            FoodSchedule foodScheduleOld = await _foodScheduleRepo.GetByIdAsync(foodSchedule.Id);
            if (foodScheduleOld == null)
            {
                throw new ApiException("Food schedule not found!");
            }

            List<DateTimeOffset> dates = new List<DateTimeOffset>() { foodSchedule.Date };

            List<FoodSchedule> notEditableFoodSchedules = new List<FoodSchedule>() { await _foodScheduleRepo.EditAsync(foodSchedule) };

            await EditAllForDates(dates, userId, foodSchedule, notEditableFoodSchedules);

            return foodSchedule;
        }

        public async Task EditAllBasedOnFoodScheduleAsync(FoodSchedule foodSchedule, Guid userId)
        {
            IEnumerable<FoodSchedule> foodSchedules = (await _foodScheduleRepo.GetAllByMedicineScheduleIdAsync(foodSchedule.MedicineScheduleId))
                .Where(fs => fs.Date.Date >= DateTime.UtcNow.Date);

            if (!foodSchedules.Any())
            {
                throw new ApiException("Food schedules not found!");
            }

            List<DateTimeOffset> dates = new List<DateTimeOffset>();

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
                throw new ApiException("Food schedules not found!");
            }

            FoodSchedule foodScheduleFirst = foodSchedules.FirstOrDefault();

            await _foodScheduleRepo.DeleteAllAsync(foodSchedules);

            IEnumerable<MedicineSchedule> medicineSchedules = new List<MedicineSchedule>() { medicineSchedule };

            await AddAsync(foodScheduleFirst, medicineSchedules);
        }

        public async Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdAsync(Guid medicineScheduleId)
        {
            return (await _foodScheduleRepo.GetAllByMedicineScheduleIdAsync(medicineScheduleId)).Where(fs => fs.Date.Date >= DateTime.UtcNow.Date);
        }

        public async Task<FoodSchedule> GetByDateAsync(DateTimeOffset date, Guid medicineScheduleId)
        {
            return await _foodScheduleRepo.GetByDateAsync(date, medicineScheduleId);
        }

        public async Task<FoodSchedule> GetByIdAsync(Guid id)
        {
            return await _foodScheduleRepo.GetByIdAsync(id);
        }

        private async Task EditAllForDates(IEnumerable<DateTimeOffset> dates, Guid userId, FoodSchedule foodSchedule,
            IEnumerable<FoodSchedule> staticFoodSchedules)
        {

            IEnumerable<FoodSchedule> foodSchedules = (await _foodScheduleRepo.GetAllByDateRangeAndUserIdAsync(dates.ToArray(), userId))
                .Where(foodSched => !staticFoodSchedules.Any(fs => fs.Id == foodSched.Id));
            foreach (FoodSchedule foodSched in foodSchedules)
            {
                foodSched.TimeOfFirstMeal = new DateTimeOffset(foodSched.TimeOfFirstMeal.Year, foodSched.TimeOfFirstMeal.Month,
                    foodSched.TimeOfFirstMeal.Day, foodSchedule.TimeOfFirstMeal.Hour, foodSchedule.TimeOfFirstMeal.Second,
                    foodSchedule.TimeOfFirstMeal.Millisecond, new TimeSpan(0, 0, 0));

                foodSched.NumberOfMeals = foodSchedule.NumberOfMeals;
            }

            await _foodScheduleRepo.EditAllAsync(foodSchedules);
        }
    }
}
