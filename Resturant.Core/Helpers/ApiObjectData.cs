using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.Serialization;

namespace Resturant.Core
{
    [Serializable]
    [DataContract(Namespace = "ApiObjectData")]
    public class ApiObjectData
    {
        [DataMember]
        public Message Message { get; set; }
        [DataMember]
        public object? ReturnData { get; set; }
        [DataMember]
        public object? ExtraReturnData { get; set; }

        public ApiObjectData()
        {
            Message = new Message(new StringLocalizer<SharedResource>(new ResourceManagerStringLocalizerFactory(new OptionsWrapper<LocalizationOptions>(new LocalizationOptions()), new LoggerFactory())));
        }
    }
}
