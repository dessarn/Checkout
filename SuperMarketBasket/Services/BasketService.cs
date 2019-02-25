using SuperMarketBasket.DataObjects;
using SuperMarketBasket.DataStore;
using SuperMarketBasket.DataStore.Interfaces;
using SuperMarketBasket.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SuperMarketBasket.Services
{
    public class BasketService : IBasketService
    {
        private IFakeDataStore fakeDataStore;

        public BasketService(IFakeDataStore dataStore)
        {
            this.fakeDataStore = dataStore;
        }

        public List<Product> GetProducts()
        {
            return fakeDataStore.FetchData();
        }

        public Product GetProductById(string sku)
        {
            if(!string.IsNullOrWhiteSpace(sku))
                return fakeDataStore.FetchItem(sku);
            return null;
        }

        public decimal CalculateBasketTotal(List<Product> productsInBasket)
        {
            decimal total = 0;
            Dictionary<string, int> specialOffersDict = new Dictionary<string, int>();
            foreach (var product in productsInBasket)
            {
                if (product.Offer != null)
                {
                    if (specialOffersDict.ContainsKey(product.SKU))
                        specialOffersDict[product.SKU]++;
                    else
                        specialOffersDict.Add(product.SKU, 1);
                    
                    if(specialOffersDict[product.SKU] == product.Offer.Number)
                    {
                        specialOffersDict[product.SKU] = 0;
                        total -= product.UnitPrice * (product.Offer.Number - 1);
                        total += product.Offer.OfferPrice;
                    }
                    else
                    {
                        total += product.UnitPrice;
                    }
                }
                else
                {
                    total += product.UnitPrice;
                }
            }
            return total;
        }

        public bool ChangeSpecialOffer(string sku, SpecialOffer specialOffer)
        {
            var product = fakeDataStore.FetchItem(sku);
            if (product != null)
            {
                product.Offer = specialOffer;
                return fakeDataStore.UpdateItem(product);
            }
            return false;
        } 
    }
}
