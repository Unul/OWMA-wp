using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace OWMA_wp.Common
{
    public class Event
    {
        public uint id;
        public string name;
        public DateTime start;
        public DateTime end;
        public string description;
        public Photo photo;

        public List<Shop> shops = new List<Shop>();

        public Event(string _name, DateTime _start, string _description)
        {
            name = _name;
            start = _start;
            description = _description;
        }
    }

    public class ObjectEvent
    {
        public Event @event;
    }

    public class Events
    {
        public List<Event> events;
    }

    public static class EventNetwork
    {
        public static async Task<bool> Create(Event oEvent)
        {
            var response = await Network.Post("/events", Network.Serialize(oEvent), true);
            if (response.IsSuccessStatusCode)
                return true;
            var errorHandler = Network.Deserialize<ErrorHandler>(await response.Content.ReadAsStringAsync());
            Utils.Notify("Une erreur s'est produite", errorHandler.errors.First().message);
            return false;
        }

        public static async Task<Events> GetAll()
        {
            var response = await Network.Get("/events", true);
            if (response.IsSuccessStatusCode)
                return Network.Deserialize<Events>(await response.Content.ReadAsStringAsync());
            var errorHandler = Network.Deserialize<ErrorHandler>(await response.Content.ReadAsStringAsync());
            Utils.Notify("Une erreur s'est produite", errorHandler.errors.First().message);
            return null;
        }
    }
}
