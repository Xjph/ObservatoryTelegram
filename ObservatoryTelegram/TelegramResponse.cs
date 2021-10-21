using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Observatory.Telegram
{
    public class TelegramResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("description")]
        public string Description{ get; set; }

        [JsonPropertyName("result")]
        public List<TelegramResponseResult> Result { get; set; }



        public TelegramResponse()
        {
            Result = new();
            Description = string.Empty;
            ErrorCode = 0;
        }
    }


    public class TelegramResponseResult
    {
        [JsonPropertyName("message")]
        public TelegramResponseResultMessage Message { get; set; }

        public TelegramResponseResult()
        {
            Message = new();
        }

    }

    public class TelegramResponseResultMessage
    {
        [JsonPropertyName("chat")]
        public TelegramResponseResultMessageChat Chat { get; set; }


        public TelegramResponseResultMessage()
        {
            Chat = new();
        }
    }

    public class TelegramResponseResultMessageChat
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        public TelegramResponseResultMessageChat()
        {
            Id = 0;
        }
    }

}
