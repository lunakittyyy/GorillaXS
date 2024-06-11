using BepInEx;
using BepInEx.Configuration;
using GorillaXS.Models;
using Newtonsoft.Json;
using System.Text;
using WebSocketSharp;

namespace GorillaXS
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class GorillaXS : BaseUnityPlugin
    {
        private static GorillaXS Instance;

        private WebSocket webSocket;

        private ConfigEntry<bool> roomNotifications;

        public void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this);

            GorillaTagger.OnPlayerSpawned(Initialize);
        }

        private void Initialize()
        {
            webSocket = new WebSocket("ws://127.0.0.1:42070/?client=gorillaxs");
            webSocket.Connect();

            PushNotification("Initialized", "GorillaXS active");
        }

        public static void PushNotification(string title, string content, int timeout = 3, int height = 88, string icon = "") => Instance?.Notify(title, content, icon, timeout, height);

        private void Notify(string title, string content, string icon, int timeout, int height)
        {
            XSONotificationObject notification = new XSONotificationObject
            {
                title = title,
                content = content,
                sourceApp = "GorillaXS",
                timeout = timeout,
                height = height,
            };

            if (!icon.IsNullOrEmpty())
            {
                notification.useBase64Icon = true;
                notification.icon = icon;
            }

            XSOApiObject apiObj = new XSOApiObject
            {
                sender = "gorillaxs",
                target = "xsoverlay",
                command = "SendNotification",
                jsonData = JsonConvert.SerializeObject(notification),
                rawData = null
            };

            webSocket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiObj)));
        }
    }
}
