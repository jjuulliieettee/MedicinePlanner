using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasMeterAccounting.Data
{
    public static class DataSeed
    {
        public static void SeedData(ApplicationContext context)
        {
            SeedFoodRelations(context);
            SeedPharmaceuticalForms(context);
            SeedMedicine(context);
        }

        public static void SeedFoodRelations(ApplicationContext context)
        {
            if (!context.FoodRelations.Any())
            {
                List<FoodRelation> foodRelations = new List<FoodRelation>()
                {
                    new FoodRelation
                    {
                        Name = "Before meal"
                    },
                    new FoodRelation
                    {
                        Name = "After meal"
                    },
                    new FoodRelation
                    {
                        Name = "During meal"
                    },
                    new FoodRelation
                    {
                        Name = "Regardless of meal"
                    }
                };
                context.FoodRelations.AddRange(foodRelations);
                context.SaveChanges();
            }
        }

        public static void SeedPharmaceuticalForms(ApplicationContext context)
        {
            if (!context.PharmaceuticalForms.Any())
            {
                List<PharmaceuticalForm> pharmaceuticalForms = new List<PharmaceuticalForm>()
                {
                    new PharmaceuticalForm
                    {
                        Name = "Pills"
                    },
                    new PharmaceuticalForm
                    {
                        Name = "Liquid"
                    }
                };
                context.PharmaceuticalForms.AddRange(pharmaceuticalForms);
                context.SaveChanges();
            }
        }

        public static void SeedMedicine(ApplicationContext context )
        {
            if (!context.Medicines.Any())
            {
                FoodRelation foodRelation = context.FoodRelations.FirstOrDefault(fr => fr.Name == "Before meal");
                PharmaceuticalForm pharmaceuticalForm = context.PharmaceuticalForms.FirstOrDefault(pf => pf.Name == "Pills");
                List<Medicine> medicines = new List<Medicine>()
                {
                    new Medicine
                    {
                        Name = "Medicine 1",
                        ActiveSubstance = "Some active substance",
                        Dosage = 4,
                        NumberOfTakes = 2,
                        FoodRelationId = foodRelation.Id,
                        PharmaceuticalFormId = pharmaceuticalForm.Id,
                        FoodInterval = 10
                    },
                    new Medicine
                    {
                        Name = "Medicine 2",
                        ActiveSubstance = "Some other active substance",
                        Dosage = 3,
                        NumberOfTakes = 3,
                        FoodRelationId = foodRelation.Id,
                        PharmaceuticalFormId = pharmaceuticalForm.Id,
                        FoodInterval = 15
                    }
                };
                context.Medicines.AddRange(medicines);
                context.SaveChanges();
            }
        }

    }
}
