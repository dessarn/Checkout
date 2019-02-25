using SuperMarketBasket.DataObjects;
using SuperMarketBasket.DataStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.DataStore
{
    /// <summary>
    /// It the interest of time this is being hardcoded.
    /// This would normally be a database, set of documents in a directory or whatever way of persisting the data to disk.
    /// </summary>
    public class FakeDataStore : IFakeDataStore
    {
        List<Product> data = new List<Product>()
        {
            new Product("A99", "Apple", 0.50m, new SpecialOffer(3, 1.30m)),
            new Product("B15", "Biscuits", 0.30m, new SpecialOffer(2, 0.45m)),
            new Product("C40", "Coffee", 1.80m, null),
            new Product("T23", "Tissues", 0.99m, null),
            new Product("P43", "Pear", 0.45m, null)

        };

        public List<Product> FetchData()
        {
            return data;
        }

        public Product FetchItem(string id)
        {
            return data.FirstOrDefault(x => x.SKU.Equals(id));
        }

        public bool UpdateItem(Product item)
        {
            if(data.Any(x => x.SKU == item.SKU))
            {
                int index = data.FindIndex(x => x.SKU == item.SKU);
                data[index] = item;
                return true;
            }
            return false;
        }
    }
}
