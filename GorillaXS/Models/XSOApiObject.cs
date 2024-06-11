namespace GorillaXS.Models
{
    internal class XSOApiObject
    {
        /// <summary>
        /// The name of the application sending the message
        /// </summary>
        public string sender;

        /// <summary>
        /// The name of the application the message is intended for. Ex 'xsoverlay'
        /// </summary>
        public string target;

        /// <summary>
        /// The command to issue to the target
        /// </summary>
        public string command;

        /// <summary>
        /// JSON Payload with command specific data. See below for endpoint specific for endpoint message formats
        /// </summary>
        public string jsonData;

        /// <summary>
        /// Raw data payload with a singular value in string format like "true", "false", etc. This is mainly used by the UI to do things like setting settings, where the string will be parsed
        /// </summary>
        public string rawData;
    }
}
