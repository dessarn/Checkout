using SuperMarketBasket.Client.Interfaces;
using SuperMarketBasket.DataStore.Interfaces;
using SuperMarketBasket.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Singleton.Interface
{
    public interface IInstanceManager
    {
        IBasketClient BasketClientInstance { get; }
        IBasketService BasketServiceInstance { get; }
        IFakeDataStore DataStoreInstance { get; }
    }
}
