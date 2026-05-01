using System;
using System.Collections.Generic;
using System.Text;

namespace DrDanielaVintu.Labs.Lab4.Composite
{
    /// <summary>
    /// Componenta de bază a ierarhiei.
    /// </summary>
    public interface IMedicalComponent
    {
        string Display(int depth);
    }

    /// <summary>
    /// Frunza (Leaf) - O observație individuală.
    /// </summary>
    public class Observation : IMedicalComponent
    {
        private readonly string _text;
        public Observation(string text) => _text = text;

        public string Display(int depth)
        {
            return new string('-', depth) + " Observație: " + _text + "\n";
        }
    }

    /// <summary>
    /// Compozitul - O secțiune care conține alte componente.
    /// </summary>
    public class MedicalSection : IMedicalComponent
    {
        private readonly string _name;
        private readonly List<IMedicalComponent> _children = new List<IMedicalComponent>();

        public MedicalSection(string name) => _name = name;

        public void Add(IMedicalComponent component) => _children.Add(component);
        public void Remove(IMedicalComponent component) => _children.Remove(component);

        public string Display(int depth)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string('-', depth) + " SECȚIUNE: " + _name + "\n");

            foreach (var component in _children)
            {
                sb.Append(component.Display(depth + 2));
            }

            return sb.ToString();
        }
    }
}
