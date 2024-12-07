using System.Collections.Generic;

namespace ClientManagerApp.Models
{
    public class State
    {
        public string Name { get; set; }
        public List<Transition> Transitions { get; set; } = new(); // Инициализация списка
    }
}
