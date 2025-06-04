using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Infrastructure.Models
{
    public class KnyhaModel
    {
        [Key]
        public int Id { get; set; } // Замінюємо Guid на int для EF
        public string Nazva { get; set; }
        public int Rik { get; set; }
        public string Avtor { get; set; }
        public int? LibraryId { get; set; } // Зовнішній ключ для зв’язку
        public LibraryModel Library { get; set; }
    }
}