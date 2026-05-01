namespace DrDanielaVintu.Labs.Lab2.AbstractFactory
{
    // Produse Abstracte A
    public interface IHeader { string GetTitle(); }
    // Produse Abstracte B
    public interface IButton { string GetStyle(); }

    // Produse Concrete - Familia Standard
    public class StandardHeader : IHeader { public string GetTitle() => "Clinica Dr. Daniela Vintu (Standard)"; }
    public class StandardButton : IButton { public string GetStyle() => "Color: Blue; Border: 1px solid black;"; }

    // Produse Concrete - Familia Premium
    public class PremiumHeader : IHeader { public string GetTitle() => "💎 Centrul de Excelență în Nutriție (Premium) 💎"; }
    public class PremiumButton : IButton { public string GetStyle() => "Color: Gold; Border: 2px solid gold; Font-Weight: Bold;"; }

    // Fabrica Abstractă
    public interface IClinicUIFactory
    {
        IHeader CreateHeader();
        IButton CreateButton();
    }

    // Fabrici Concrete
    public class StandardUIFactory : IClinicUIFactory
    {
        public IHeader CreateHeader() => new StandardHeader();
        public IButton CreateButton() => new StandardButton();
    }

    public class PremiumUIFactory : IClinicUIFactory
    {
        public IHeader CreateHeader() => new PremiumHeader();
        public IButton CreateButton() => new PremiumButton();
    }
}
