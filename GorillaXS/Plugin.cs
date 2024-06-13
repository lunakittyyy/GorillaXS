using BepInEx;
using GorillaXS.Types;
using Newtonsoft.Json;
using WebSocketSharp;
using System.Text;
using UnityEngine;
using System;
using System.IO;

namespace GorillaXS
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    internal class XSPlugin : BaseUnityPlugin
    {
        public static XSPlugin Instance;

        private WebSocket webSocket;

        public void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this); // FUCK OFF.

            GorillaTagger.OnPlayerSpawned(Initialize);
        }

        public void Initialize()
        {
            DefineWebSocket();
        }

        // define and connect our websocket
        public void DefineWebSocket()
        {
            if (IsWebSocketValid()) return; // ignore if we have a perfectly fine websocket !

            webSocket = new WebSocket(Constants.WebsocketUrl);
            webSocket.Connect();
        }

        // send bytes through our websocket
        public void SendWebSocketBytes(byte[] bytes) => webSocket.Send(bytes);

        // whether our websocket is validated
        public bool IsWebSocketValid() => webSocket != null && webSocket.IsAlive;

        // sends a log to the console
        public void Log(object data, BepInEx.Logging.LogLevel level = BepInEx.Logging.LogLevel.Info) => Logger.Log(level, data);
    }

    public static class Notifier
    {
        public static void Notify(XSONotificationObject notification)
        {
            if (notification == null) return;
            
            if (notification.sourceApp.IsNullOrEmpty())
            {
                notification.sourceApp = "GorillaXS";
            }

            XSOApiObject apiObj = new()
            {
                sender = "gorillaxs",
                target = "xsoverlay",
                command = "SendNotification",
                jsonData = JsonConvert.SerializeObject(notification),
                rawData = null
            };

            if (XSPlugin.Instance.IsWebSocketValid())
            {
                XSPlugin.Instance.SendWebSocketBytes(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiObj)));
            }
            else
            {
                Debug.LogError("Failed to send notification to XSOverlay, trying to re-establish WebSocket connection");
                XSPlugin.Instance.DefineWebSocket();
            }
        }

        /// <summary>
        /// Send a notification to XSOverlay via WebSockets.
        /// </summary>
        /// <param name="title">Title displayed above the notification. Required</param>
        /// <param name="content">Main body of the notification. Required</param>
        /// <param name="height">Height of the notification</param>
        /// <param name="timeout">Time before the notification disappears</param>
        /// <param name="icon">Name of icon or Base64. Can be "default", "error", "warning", or a Base64 encoded image</param>
        public static void Notify(string title, string content, float height = 88, float timeout = 3, string icon = "", string AudioPath = "default")
        {
            XSONotificationObject notification = new()
            {
                title = title,
                content = content,
                timeout = timeout,
                height = height,
                sourceApp = "GorillaXS",
                audioPath = AudioPath
            };

            if (icon == "default" || icon == "warning" || icon == "error")
            {
                notification.icon = icon;
            }
            else if (icon.IsNullOrWhiteSpace())
            {
                notification.icon = "default";
            }
            else
            {
                notification.useBase64Icon = true;
                notification.icon = icon;
            }

            Notify(notification);
        }
    }

    public static class Decoding
    {
        public static string ToBase64String(byte[] bytes) => Convert.ToBase64String(bytes);

        public static string ToBase64String(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            string base64String = ToBase64String(bytes);

            stream.Close();

            return base64String;
        }
    }
}
