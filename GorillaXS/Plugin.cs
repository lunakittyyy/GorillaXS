using BepInEx;
using GorillaXS.Types;
using System;
using Newtonsoft.Json;
using WebSocketSharp;
using System.Text;

namespace GorillaXS
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class XSPlugin : BaseUnityPlugin
    {
        public static WebSocket webSocket;
        public static XSPlugin Instance;
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
        }
    }

    public static class Notifier
    {
        /// <summary>
        /// Send a notification to XSOverlay via WebSockets.
        /// </summary>
        /// <param name="title">Title displayed above the notification. Required</param>
        /// <param name="content">Main body of the notification. Required</param>
        /// <param name="height">Height of the notification</param>
        /// <param name="timeout">Time before the notification disappears</param>
        /// <param name="Base64Icon">Icon data in Base64. If not defined a bell icon will be used</param>
        public static void Notify(string title, string content, float height = 88, float timeout = 3, string Base64Icon = "", string AudioPath = "default")
        {
            XSONotificationObject notification = new XSONotificationObject
            {
                title = title,
                content = content,
                timeout = timeout,
                height = height,
                sourceApp = "GorillaXS",
                audioPath = AudioPath
            };

            if (!Base64Icon.IsNullOrEmpty())
            {
                notification.useBase64Icon = true;
                notification.icon = Base64Icon;
            }

            XSOApiObject apiObj = new XSOApiObject()
            {
                sender = "gorillaxs",
                target = "xsoverlay",
                command = "SendNotification",
                jsonData = JsonConvert.SerializeObject(notification),
                rawData = null
            };

            XSPlugin.webSocket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiObj)));
        }
    }
}
