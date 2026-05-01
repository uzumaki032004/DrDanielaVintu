using Microsoft.AspNetCore.Mvc;
using DrDanielaVintu.Labs.Lab4.Adapter;
using DrDanielaVintu.Labs.Lab4.Composite;
using DrDanielaVintu.Labs.Lab4.Facade;

namespace DrDanielaVintu.Controllers
{
    public class Lab4Controller : Controller
    {
        public IActionResult Index()
        {
            // 1. Adapter Demo
            var externalSystem = new ExternalLabSystem();
            IClinicReport adapter = new LabResultAdapter(externalSystem);

            // 2. Composite Demo
            var report = new MedicalSection("Raport Complet");
            var bloodSection = new MedicalSection("Analize Sânge");
            bloodSection.Add(new Observation("Glicemie: 105"));
            bloodSection.Add(new Observation("Colesterol: 210"));
            
            var physicalSection = new MedicalSection("Examen Fizic");
            physicalSection.Add(new Observation("Tensiune: 120/80"));
            
            report.Add(bloodSection);
            report.Add(physicalSection);
            report.Add(new Observation("Recomandare: Scădere consum sare."));

            // 3. Facade Demo
            var facade = new OnboardingFacade();
            var onboardingResult = facade.FullOnboarding("Andrei Mureșan");

            ViewBag.AdapterHeader = adapter.GetHeader();
            ViewBag.AdapterResults = adapter.GetResults();
            ViewBag.CompositeReport = report.Display(0);
            ViewBag.OnboardingResult = onboardingResult;

            return View();
        }
    }
}
