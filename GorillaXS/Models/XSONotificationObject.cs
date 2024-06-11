namespace GorillaXS.Models
{
    internal class XSONotificationObject
    {
        /// <summary>
        /// The type of message to send. 1 defines a normal notification.
        /// </summary>
        public int type = 1;

        /// <summary>
        /// Used for Media Player, changes the icon on the wrist. (depricated, see note below)
        /// </summary>
        public int index = 0;

        /// <summary>
        /// How long the notification will stay on screen for in seconds.
        /// </summary>
        public float timeout = 0.5f;

        /// <summary>
        /// Height notification will expand to if it has content other than a title. Default is 175.
        /// </summary>
        public float height = 175;

        /// <summary>
        /// Opacity of the notification, to make it less intrusive. Setting to 0 will set to 1.
        /// </summary>
        public float opacity = 1;

        /// <summary>
        /// Notification sound volume.
        /// </summary>
        public float volume = 0.7f;

        /// <summary>
        /// File path to .ogg audio file. Can be "default", "error", or "warning". Notification will be silent if left empty.
        /// </summary>
        public string audioPath;

        /// <summary>
        /// Notification title, supports Rich Text Formatting.
        /// </summary>
        public string title = "";

        /// <summary>
        /// Notification content, supports Rich Text Formatting, if left empty, notification will be small.
        /// </summary>
        public string content = "";

        /// <summary>
        /// Set to true if using Base64 for the icon image.
        /// </summary>
        public bool useBase64Icon = false;

        /// <summary>
        /// Base64 Encoded image, or file path to image. Can also be "default", "error", or "warning".
        /// </summary>
        public string icon = "";

        /// <summary>
        /// Somewhere to put your app name for debugging purposes.
        /// </summary>
        public string sourceApp = "";
    }
}
