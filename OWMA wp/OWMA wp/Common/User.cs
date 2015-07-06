using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;

namespace OWMA_wp.Common
{
    static public class CurrentAuthUser
    {
        public static string uid;
        public static string access_token;
        public static string client;
        public static string expiry;
        public static bool isConnected = false;
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public uint id { get; set; }
        [DataMember]
        public String email { get; set; }
        [DataMember]
        public String password { get; set; }
        [DataMember]
        public String first_name { get; set; }
        [DataMember]
        public String last_name { get; set; }
        [DataMember]
        public int gender { get; set; }
        [DataMember]
        public DateTime birthday { get; set; }

        public User(uint _id)
        {
            id = _id;
        }

        public User(String _email, String _password)
        {
            email = _email;
            password = _password;
        }

        public User() { }

    }

    [DataContract]
    public class ObjectUser
    {
        [DataMember]
        public User user { get; set; }

        public ObjectUser(String _email, String _password)
        {
            user = new User(_email, _password);
        }

        public ObjectUser(User _user)
        {
            user = _user;
        }
    }

    static public class UserNetwork
    {
        public static async Task<ObjectUser> Register(String Email, String Password, String PasswordConfirmation)
        {
            HttpResponseMessage response = await Network.Post("/auth", Network.Serialize(new ObjectUser(Email, Password)));
            if (response.IsSuccessStatusCode)
                return Network.Deserialize<ObjectUser>(await response.Content.ReadAsStringAsync());
            var errorHandler = Network.Deserialize<ErrorHandler>(await response.Content.ReadAsStringAsync());
            Utils.Notify("Une erreur s'est produite", errorHandler.errors.First().message);
            return null;
        }

        public static async Task<ObjectUser> Login(String Email, String Password)
        {
            HttpResponseMessage response = await Network.Post("/auth/sign_in", Network.Serialize(new ObjectUser(Email, Password)));
            if (response.IsSuccessStatusCode)
                return Network.Deserialize<ObjectUser>(await response.Content.ReadAsStringAsync());
            var errorHandler = Network.Deserialize<ErrorHandler>(await response.Content.ReadAsStringAsync());
            Utils.Notify("Une erreur s'est produite", errorHandler.errors.First().message);
            return null;
        }

        internal static async Task<bool> Update(User user)
        {
            HttpResponseMessage response = await Network.Patch("/user/" + user.id, Network.Serialize(new ObjectUser(user)), true);
            if (response.IsSuccessStatusCode)
                return true;
            var errorHandler = Network.Deserialize<ErrorHandler>(await response.Content.ReadAsStringAsync());
            Utils.Notify("Une erreur s'est produite", errorHandler.errors.First().message);
            return false;
        }
    }
}
