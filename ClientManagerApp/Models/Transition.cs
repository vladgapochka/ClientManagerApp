namespace ClientManagerApp.Models
{
    public class Transition
    {
        public State From { get; set; }
        public State To { get; set; }
        public string Action { get; set; } // Добавление, Удаление, Редактирование
    }
}
