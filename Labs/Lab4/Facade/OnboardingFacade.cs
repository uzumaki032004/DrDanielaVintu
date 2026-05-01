using System;

namespace DrDanielaVintu.Labs.Lab4.Facade
{
    // Subsistem 1
    public class PatientRegistry { public string Register(string name) => $"Pacient {name} înregistrat."; }
    // Subsistem 2
    public class NutritionCalculator { public string CalculateMacros() => "Macros calculați: 40% P, 30% C, 30% F."; }
    // Subsistem 3
    public class EmailConfirmation { public string Send() => "Email de confirmare trimis."; }

    /// <summary>
    /// Fațada care simplifică accesul la subsisteme.
    /// </summary>
    public class OnboardingFacade
    {
        private readonly PatientRegistry _registry = new PatientRegistry();
        private readonly NutritionCalculator _calculator = new NutritionCalculator();
        private readonly EmailConfirmation _email = new EmailConfirmation();

        public string FullOnboarding(string patientName)
        {
            var r1 = _registry.Register(patientName);
            var r2 = _calculator.CalculateMacros();
            var r3 = _email.Send();

            return $"{r1}\n{r2}\n{r3}\nStatus: Finalizat cu succes prin Fațadă.";
        }
    }
}
