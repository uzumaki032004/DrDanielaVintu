using Microsoft.AspNetCore.Mvc;
using DrDanielaVintu.Labs.Lab2.Factories;
using DrDanielaVintu.Labs.Lab2.AbstractFactory;
using System.Collections.Generic;

namespace DrDanielaVintu.Controllers
{
    public class Lab2Controller : Controller
    {
        public IActionResult Index()
        {
            // 1. Factory Method Demo
            var creators = new List<MedicalServiceCreator>
            {
                new ConsultationCreator(),
                new LabTestCreator()
            };

            var factoryResults = new List<string>();
            foreach (var creator in creators)
            {
                factoryResults.Add(creator.GetServiceInfo());
            }

            // 2. Abstract Factory Demo
            IClinicUIFactory standardFactory = new StandardUIFactory();
            IClinicUIFactory premiumFactory = new PremiumUIFactory();

            ViewBag.FactoryResults = factoryResults;
            ViewBag.StandardHeader = standardFactory.CreateHeader().GetTitle();
            ViewBag.StandardButtonStyle = standardFactory.CreateButton().GetStyle();
            ViewBag.PremiumHeader = premiumFactory.CreateHeader().GetTitle();
            ViewBag.PremiumButtonStyle = premiumFactory.CreateButton().GetStyle();

            return View();
        }
    }
}
