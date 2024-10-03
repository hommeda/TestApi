using Resturant.Core.Utilities;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Core
{
    [Serializable]
    [DataContract(Namespace = "Message")]
    public class Message
    {
        [NonSerialized]
        private string _type, _content, _log;

        private readonly IStringLocalizer<SharedResource> _localizer;

        [DataMember]
        public string Type { get => _type; set => _type = value; }
        [DataMember]
        public string Content { get => _content; set => _content = value; }
        [DataMember]
        public string Log { get => _log; set => _log = value; }

        public Message(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public void MsgSuccess()
        {
            _type = "Success";
            Content = _localizer["SaveMsgSuccess"];
        }

        public void MsgSuccess(string content)
        {
            _type = "Success";
            _content = content;
        }

        public void MsgError(string content)
        {
            _type = "Error";
            _content = content;
        }

        public void MsgLogError(string log)
        {
            _type = "Error";
            _log = log;
            var content = log.MessageDuplicate();
            if (!string.IsNullOrEmpty(content))
            {
                Content = content;
                return;
            }
            Content = _localizer["UnexpectedError"];
        }

        public void MsgError(string content, string log)
        {
            _type = "Error";
            _content = content;
            _log = log;
        }
    }
}
