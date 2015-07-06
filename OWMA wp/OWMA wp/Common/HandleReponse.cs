using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Newtonsoft.Json.Linq;

namespace OWMA_wp.Common
{
    // Delegate type
    delegate void Del();

    class HandleReponse
    {
        public static async void WithDialog(JToken Json, string Confirmation, Del callback = null)
        {
            Debug.WriteLine(Json);
            if (Json != null)
            {
                if ((string)Json.SelectToken("status") == "error" || Json.SelectToken("errors") != null)
                {
                    var errors = Json.SelectToken("errors");
                    MessageDialog messageDialog = new MessageDialog("Erreur :\n" + errors.First);
                    messageDialog.CancelCommandIndex = 1;
                    await messageDialog.ShowAsync();
                }
                else
                {
                    Utils.Notify(Confirmation, "");
                    if (callback != null)
                        callback();
                }
            }
        }
    }
}
