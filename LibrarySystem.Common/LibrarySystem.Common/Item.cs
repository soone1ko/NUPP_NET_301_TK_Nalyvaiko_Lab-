using System;

namespace LibrarySystem.Common
{
    // Базовий клас для елементів бібліотеки
    public class Item
    {
        // Властивості
        public Guid Id { get; set; }
        public string Назва { get; set; }
        public int Рік { get; set; }

        // Статичне поле для підрахунку всіх елементів
        public static int ЗагальнаКількість { get; private set; }

        // Конструктор
        public Item(string назва, int рік)
        {
            Id = Guid.NewGuid();
            Назва = назва;
            Рік = рік;
            ЗагальнаКількість++;
        }

        // Віртуальний метод для отримання опису
        public virtual string ОтриматиОпис()
        {
            return $"Назва: {Назва}, Рік: {Рік}";
        }

        // Делегат і подія
        public delegate void ОбробникПодії(string повідомлення);
        public event ОбробникПодії ПриЗмініСтатусу;

        // Метод для виклику події
        public void ВикликатиПодію(string повідомлення)
        {
            ПриЗмініСтатусу?.Invoke(повідомлення);
        }
    }
}