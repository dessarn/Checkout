using SuperMarketBasket.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.Services.Interfaces
{
    public interface IBasketService
    {
        List<Product> GetProducts();
        Product GetProductById(string sku);
        decimal CalculateBasketTotal(List<Product> ProductsInBasket);
        bool ChangeSpecialOffer(string sku, SpecialOffer specialOffer);

    }
}
