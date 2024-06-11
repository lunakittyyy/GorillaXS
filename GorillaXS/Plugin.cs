using BepInEx;
using GorillaXS.Types;
using System;
using Newtonsoft.Json;
using WebSocketSharp;
using System.Text;

namespace GorillaXS
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class XSPlugin : BaseUnityPlugin
    {
        public static WebSocket ws;
        public static XSPlugin instance;
        void Start() => Utilla.Events.GameInitialized += OnGameInitialized;

        void OnGameInitialized(object sender, EventArgs e)
        {
            instance = this;

            ws = new WebSocket("ws://127.0.0.1:42070/?client=gorillaxs");
            ws.Connect();
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
            XSONotificationObject notification = new XSONotificationObject();
            notification.title = title;
            notification.content = content;
            if (Base64Icon != "")
            {
                notification.useBase64Icon = true;
                notification.icon = Base64Icon;
            }
            notification.timeout = timeout;
            notification.height = height;
            notification.sourceApp = "GorillaXS";
            notification.audioPath = AudioPath;

            XSOApiObject apiObj = new XSOApiObject();
            apiObj.sender = "gorillaxs";
            apiObj.target = "xsoverlay";
            apiObj.command = "SendNotification";
            apiObj.jsonData = JsonConvert.SerializeObject(notification);
            apiObj.rawData = null;

            XSPlugin.ws.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiObj)));
        }
    }
}
