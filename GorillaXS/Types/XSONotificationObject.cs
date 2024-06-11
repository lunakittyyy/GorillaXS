using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaXS.Types
{
    public class XSONotificationObject
    {
        public int type = 1;
        public int index = 0;
        public float timeout = 0.5f;
        public float height = 175;
        public float opacity = 1;
        public float volume = 0.7f;
        public string title = "";
        public string content = "";
        public bool useBase64Icon = false;
        public string icon = "";
        public string sourceApp = "";
    }
}
