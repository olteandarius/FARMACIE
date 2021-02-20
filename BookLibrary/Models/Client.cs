using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public class Client : IEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(maximumLength: 13, MinimumLength = 13)]
        public string CNP { get; set; }

        public ApplicationUser Owner { get; set; }

        public ICollection<Rental> RentedBooks { get; } = new List<Rental>();
    }
}
