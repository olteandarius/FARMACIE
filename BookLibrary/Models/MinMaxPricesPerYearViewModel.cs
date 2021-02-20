using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public class MinMaxPricesPerYearViewModel
    {
        public int PublicationYear { get; set; }

        public string MinPriceTitle { get; set; }
        public decimal MinPrice { get; set; }

        public string MaxPriceTitle { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
