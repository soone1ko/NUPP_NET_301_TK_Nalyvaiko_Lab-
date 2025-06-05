using System;

namespace LibrarySystem.Common
{
    // Клас Magazine, що успадковує Item
    public class Magazine : Item
    {
        // Додаткова властивість
        public int НомерВипуску { get; set; }

        // Конструктор
        public Magazine(string назва, int рік, int номерВипуску) : base(назва, рік)
        {
            НомерВипуску = номерВипуску;
        }

        // Перевизначений метод
        public override string ОтриматиОпис()
        {
            return $"Журнал: {Назва}, Випуск: {НомерВипуску}, Рік: {Рік}";
        }
    }
}