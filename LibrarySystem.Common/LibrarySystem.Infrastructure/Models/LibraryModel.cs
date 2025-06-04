using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Infrastructure.Models
{
    public class LibraryModel
    {
        [Key]
        public int Id { get; set; }
        public string Nazva { get; set; }
        public ICollection<KnyhaModel> Knyhy { get; set; } = new List<KnyhaModel>();
    }
}