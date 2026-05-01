using Microsoft.AspNetCore.Mvc;
using DrDanielaVintu.Labs.Lab3.Builders;
using DrDanielaVintu.Labs.Lab3.Models;
using DrDanielaVintu.Labs.Lab3.Singletons;
using System;

namespace DrDanielaVintu.Controllers
{
    public class Lab3Controller : Controller
    {
        public IActionResult Index()
        {
            // 1. Builder Demo
            var builder = new CustomMealPlanBuilder();
            var director = new NutritionistDirector();
            director.ConstructWeightLossPlan(builder);
            var mealPlan = builder.GetResult();

            // 2. Prototype Demo
            var originalRecord = new PatientRecord 
            { 
                PatientName = "Maria Ionescu", 
                Diagnosis = "Deficit Vitamina D", 
                LastVisit = DateTime.Now.AddMonths(-1) 
            };
            var clonedRecord = (PatientRecord)originalRecord.Clone();
            clonedRecord.PatientName = "Maria Ionescu (Copie/Update)";

            // 3. Singleton Demo
            var settings = ClinicSettings.Instance;

            ViewBag.MealPlan = mealPlan.ToString();
            ViewBag.OriginalRecord = originalRecord.ToString();
            ViewBag.ClonedRecord = clonedRecord.ToString();
            ViewBag.ClinicName = settings.ClinicName;
            ViewBag.InitializedAt = settings.InitializedAt.ToString("HH:mm:ss.fff");

            return View();
        }
    }
}
