using Alexa.NET.LocaleSpeech;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using System.Collections.Generic;

namespace AlexaPowerTwitter.Common
{
    public class Accessor
    {
        public Request Request { get; internal set; }
        public string Lang { get; internal set; }
        public ILocaleSpeech LocaleSpeech { get; internal set; }
        public Dictionary<string, string> LocaleResources { get; internal set; }
        public Session Session { get; internal set; }
        public IntentRequest IntentRequest { get; internal set; }
    }
}
