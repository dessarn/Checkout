using SuperMarketBasket.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketBasket.DataStore.Interfaces
{
    public interface IFakeDataStore
    {
        List<Product> FetchData();
        Product FetchItem(string id);
        bool UpdateItem(Product item);
    }
}
