using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public class Book : IEntity
    {
        public Guid Id { get; set; }

        [MinLength(2)]
        public string Title { get; set; }
        
        public string Author { get; set; }

        [Display(Name = "Publication year")]
        public int PublicationYear { get; set; }

        public decimal Price { get; set; }

        public ApplicationUser Owner { get; set; }

        public ICollection<Rental> RentedBy { get; } = new List<Rental>();
    }
}
