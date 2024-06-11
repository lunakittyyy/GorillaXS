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
    public class Plugin : BaseUnityPlugin
    {
        public static WebSocket ws;
        void Start() => Utilla.Events.GameInitialized += OnGameInitialized;

        void OnGameInitialized(object sender, EventArgs e)
        {

            ws = new WebSocket("ws://127.0.0.1:42070/?client=gorillaxs");
            ws.Connect();

            gameObject.AddComponent<PUNNotifications>();
            Notify("Initialized", "GorillaXS active");
        }

        public static void Notify(string title, string content)
        {
            XSONotificationObject notification = new XSONotificationObject();
            notification.title = title;
            notification.content = content;
            notification.sourceApp = "GorillaXS";
            notification.timeout = 3;
            notification.height = 88;

            XSOApiObject apiObj = new XSOApiObject();
            apiObj.sender = "gorillaxs";
            apiObj.target = "xsoverlay";
            apiObj.command = "SendNotification";
            apiObj.jsonData = JsonConvert.SerializeObject(notification);
            apiObj.rawData = null;

            ws.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiObj)));
        }
    }
}
