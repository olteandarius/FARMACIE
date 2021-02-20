using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public class Rental : IEntity, IValidatableObject
    {
        [NotMapped]
        public List<SelectListItem> Books { get; set; }
        [NotMapped]
        public List<SelectListItem> ClientCNPs { get; set; }

        public Guid Id { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Rental date")]
        public DateTime RentalDate { get; set; }

        [Display(Name = "Rental duration")]
        public int? RentalDuration { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Return date")]
        public DateTime ReturnDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RentalDate < DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    $"The rental date must be no earlier than today ({DateTime.Now}).",
                    new[] { "RentalDate" });
            }

            if (ReturnDate <= DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    $"The return date must be in the future (after {DateTime.Now}).",
                    new[] { "ReturnDate" });
            }

            if (RentalDuration < 7)
            {
                yield return new ValidationResult(
                    $"The rental duration must be at least 7 days.",
                    new[] { "RentalDuration" });
            }

            if (RentalDate > ReturnDate)
            {
                yield return new ValidationResult(
                    $"The return date must be after or at the rental date.",
                    new[] { "RentalDate", "ReturnDate" });
            }
        }
    }
}
