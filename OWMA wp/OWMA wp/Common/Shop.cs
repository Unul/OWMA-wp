using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWMA_wp.Common
{
    public class Shop
    {
        public uint id;
        public string name;
        public string description;
        public uint event_id;
    }

    public class ObjectShop
    {
        public Shop shop;
    }

    public class Shops
    {
        public List<Shop> shops;
    }

    public static class ShopNetwork
    {
        public static async Task<ObjectShop> create(Shop shop)
        {
            var response = await Network.Post("/events/" + shop.event_id + "/shops", Network.Serialize(shop), true);
            if (response.IsSuccessStatusCode)
                return Network.Deserialize<ObjectShop>(await response.Content.ReadAsStringAsync());
            var errorHandler = Network.Deserialize<ErrorHandler>(await response.Content.ReadAsStringAsync());
            Utils.Notify("Une erreur s'est produite", errorHandler.errors.First().message);
            return null;
        }

        public static async Task<Shops> GetAll(Event oEvent)
        {
            var response = await Network.Get("/events/" + oEvent.id + "/shops");
            if (response.IsSuccessStatusCode)
                return Network.Deserialize<Shops>(await response.Content.ReadAsStringAsync());
            var errorHandler = Network.Deserialize<ErrorHandler>(await response.Content.ReadAsStringAsync());
            Utils.Notify("Une erreur s'est produite", errorHandler.errors.First().message);
            return null;
        }
    }
}
