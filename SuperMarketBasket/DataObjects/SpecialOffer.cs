using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.DataObjects
{
    public class SpecialOffer
    {
        public int Number { get; set; }
        public decimal OfferPrice { get; set; }

        public SpecialOffer(int number, decimal offerPrice)
        {
            this.Number = number;
            this.OfferPrice = offerPrice;
        }
    }
}
