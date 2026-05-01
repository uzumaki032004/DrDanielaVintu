using DrDanielaVintu.Labs.Lab3.Models;

namespace DrDanielaVintu.Labs.Lab3.Builders
{
    public interface IMealPlanBuilder
    {
        void SetBasicInfo(string name, int calories);
        void AddMeal(string meal);
        void AddExclusion(string exclusion);
        void SetSupplements(bool include);
        ComplexMealPlan GetResult();
    }

    public class CustomMealPlanBuilder : IMealPlanBuilder
    {
        private ComplexMealPlan _mealPlan = new ComplexMealPlan();

        public void SetBasicInfo(string name, int calories)
        {
            _mealPlan.Name = name;
            _mealPlan.DailyCalories = calories;
        }

        public void AddMeal(string meal) => _mealPlan.Meals.Add(meal);
        public void AddExclusion(string exclusion) => _mealPlan.Exclusions.Add(exclusion);
        public void SetSupplements(bool include) => _mealPlan.IncludeSupplementPlan = include;

        public ComplexMealPlan GetResult()
        {
            var result = _mealPlan;
            _mealPlan = new ComplexMealPlan(); // Reset for next build
            return result;
        }
    }

    /// <summary>
    /// Directorul care gestionează procesul de construire.
    /// </summary>
    public class NutritionistDirector
    {
        public void ConstructWeightLossPlan(IMealPlanBuilder builder)
        {
            builder.SetBasicInfo("Plan Slăbire Rapidă", 1500);
            builder.AddMeal("Mic dejun: Omletă");
            builder.AddMeal("Prânz: Piept de pui cu salată");
            builder.AddMeal("Cină: Pește alb");
            builder.AddExclusion("Zahăr");
            builder.AddExclusion("Pâine albă");
            builder.SetSupplements(true);
        }
    }
}
