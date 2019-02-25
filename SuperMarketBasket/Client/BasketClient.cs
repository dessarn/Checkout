using SuperMarketBasket.Client.Interfaces;
using SuperMarketBasket.DataObjects;
using SuperMarketBasket.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.Client
{
    public class BasketClient : IBasketClient
    {
        public List<Product> ProductList { get; private set; }

        public List<Product> Basket { get; private set; }

        private IBasketService basketservice;

        public BasketClient(IBasketService basketservice)
        {
            this.basketservice = basketservice;
            Basket = new List<Product>();
            ProductList = new List<Product>();
        }

        public void GetItems()
        {
            ProductList = basketservice.GetProducts();
        }

        public Decimal Checkout()
        {
            return basketservice.CalculateBasketTotal(Basket);
        }

        public Boolean ChangeSpecialOffer(string sku, SpecialOffer specialOffer)
        {
            if(!String.IsNullOrWhiteSpace(sku))
            {
                if(basketservice.ChangeSpecialOffer(sku, specialOffer))
                {
                    var product = ProductList.FirstOrDefault(p => p.SKU == sku);
                    if(product != null)
                    {
                        product.Offer = specialOffer;
                        return true;
                    }   
                }
            }
            return false;   
        }

        public void Scan(string sku)
        {
            Product product = basketservice.GetProductById(sku);

            if(product != null)
            {
                Basket.Add(product);
            }             
        }
    }
}
