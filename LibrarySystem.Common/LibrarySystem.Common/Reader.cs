using System;

namespace LibrarySystem.Common
{
    public class Reader
    {
        public Guid Id { get; set; }
        public string Имя { get; set; }
        public DateTime ДатаРегистрации { get; set; }

        public Reader(string имя)
        {
            Id = Guid.NewGuid();
            Имя = имя;
            ДатаРегистрации = DateTime.Now;
        }

        public string ПолучитьИнформацию()
        {
            return $"Читатель: {Имя}, Зарегистрирован: {ДатаРегистрации.ToShortDateString()}";
        }
    }
}