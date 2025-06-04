namespace LibrarySystem.Common
{
    // Клас із методом розширення
    public static class РозширенняItem
    {
        // Метод розширення для перевірки, чи є елемент новим
        public static bool ЄНовим(this Item item)
        {
            return item.Рік >= DateTime.Now.Year - 5;
        }
    }
}