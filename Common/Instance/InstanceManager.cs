using Common.Singleton.Interface;
using SuperMarketBasket.Client;
using SuperMarketBasket.Client.Interfaces;
using SuperMarketBasket.DataStore;
using SuperMarketBasket.DataStore.Interfaces;
using SuperMarketBasket.Services;
using SuperMarketBasket.Services.Interfaces;

namespace Common.Singleton
{
    public class InstanceManager : IInstanceManager
    {
        private IBasketClient basketClientInstance;
        public IBasketClient BasketClientInstance
        {
            get
            {
                if (basketClientInstance != null)
                {
                    basketClientInstance = new BasketClient(BasketServiceInstance);
                }

                return basketClientInstance;
            }
        }

        private IBasketService basketServiceInstance;
        public IBasketService BasketServiceInstance
        {
            get
            {
                if (basketServiceInstance != null)
                {
                    basketServiceInstance = new BasketService(DataStoreInstance);
                }

                return basketServiceInstance;
            }
        }

        private IFakeDataStore dataStoreInstance;
        public IFakeDataStore DataStoreInstance
        {
            get
            {
                if (dataStoreInstance != null)
                {
                    dataStoreInstance = new FakeDataStore();
                }

                return dataStoreInstance;
            }
        }


    }
}
