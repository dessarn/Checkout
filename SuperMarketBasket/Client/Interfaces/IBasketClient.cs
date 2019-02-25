using SuperMarketBasket.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.Client.Interfaces
{
    public interface IBasketClient
    {
        List<Product> ProductList { get; }
        List<Product> Basket { get; }
        void GetItems();
        Decimal Checkout();
        Boolean ChangeSpecialOffer(string sku, SpecialOffer specialOffer);
        void Scan(string sku);
    }
}
