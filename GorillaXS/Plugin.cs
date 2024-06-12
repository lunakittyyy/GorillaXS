using BepInEx;
using GorillaXS.Types;
using System;
using Newtonsoft.Json;
using WebSocketSharp;
using System.Text;
using UnityEngine;

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

        public void Initialize()
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
        /// <param name="icon">Name of icon or Base64. Can be "default", "error", "warning", or a Base64 encoded image</param>
        public static void Notify(string title, string content, float height = 88, float timeout = 3, string icon = "", string AudioPath = "default")
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

            XSOApiObject apiObj = new XSOApiObject()
            {
                sender = "gorillaxs",
                target = "xsoverlay",
                command = "SendNotification",
                jsonData = JsonConvert.SerializeObject(notification),
                rawData = null
            };
            try
            {
                XSPlugin.webSocket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiObj)));
            } catch
            {
                Debug.LogError("Failed to send notification to XSOverlay, trying to re-establish WebSocket connection");
                XSPlugin.Instance.Initialize();
            }
        }
    }
}
