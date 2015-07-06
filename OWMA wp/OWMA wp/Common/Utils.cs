using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace OWMA_wp.Common
{
    public class Utils
    {
        public class ObservablePairList<T, U> :
            ObservableCollection<Pair<T, U>>
        {
            public IList<Pair<T, U>> Values { get { return Items; } }
        }

        public class Pair<T, U>
        {
            public T Key { get; set; }

            public U Value { get; set; }

            public Pair(T key, U value)
            {
                Value = value;
                Key = key;
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public static void Notify(string title, string text)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(title));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(text));

            ((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static int toInt(string str)
        {
            return Int32.Parse(str);
        }

        //public static class JWT
        //{
        //    private static string _secretKey = "plopplop";

        //    public static string Encoder(string payload)
        //    {
        //        return Jwt.Encode(payload, JwsAlgorithms.HS256, Utils.GetBytes(_secretKey));
        //    }

        //    public static string Decoder(string payload)
        //    {
        //        return Jwt.Decode(payload, JwsAlgorithms.HS256);
        //    }
        //}
    }
}