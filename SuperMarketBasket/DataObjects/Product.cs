using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.DataObjects
{
    public class Product : INotifyPropertyChanged
    {
        #region Properties
        public String SKU { get; set; }
        public String Name { get; set; }
        public decimal UnitPrice { get; set; }
        public SpecialOffer Offer { get; set; }
        #endregion

        public Product(string sku, string name, decimal unitPrice, SpecialOffer offer)
        {
            this.SKU = sku;
            this.Name = name;
            this.UnitPrice = unitPrice;
            this.Offer = offer;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
