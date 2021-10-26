using System;
using Observatory.Framework;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace Observatory.Telegram
{
    public class TelegramSettings
    {
        public TelegramSettings()
        {
            Logger.AppendLog("", LogFile,TelegramWorker.ObservatoryTelegramVersion);
            LogFile = (Directory.Exists(Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local\ObservatoryCore")))
                ? Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local\ObservatoryCore\Telegram.log")
                : @".\Telegram.Log";
        }

        private string apikey;
        
        [SettingDisplayName("Telegram Bot API Key")]
        public string APIKey { get => (apikey==null) ? "" : apikey; set => apikey=value; }

        [System.Text.Json.Serialization.JsonIgnore]
        [SettingDisplayName("Force ChatID Update")]
        public Action ForceUpdate
        {
            get => GetChatID;
        }

        [SettingDisplayName("Log File")]
        [System.Text.Json.Serialization.JsonIgnore]
        public System.IO.FileInfo LogFileLocation { get => new System.IO.FileInfo(LogFile); set => LogFile = value.FullName; }

        [SettingIgnore]
        public string LogFile { get; set; }


        [SettingIgnore]
        public string ChatID { get; set; }

        internal HttpClient client;

        public void GetChatID()
        {
            if(APIKey=="")
            {
                Logger.AppendLog("API Key blank", LogFile, TelegramWorker.ObservatoryTelegramVersion);
            }
            else if (!APIKey.Contains(":"))
            {
                Logger.AppendLog("Invalid API Key", LogFile, TelegramWorker.ObservatoryTelegramVersion);
            }
            else
            {
                string url = "https://api.telegram.org/bot" + APIKey + "/getUpdates";
                
                string resultStr;
                //client.Timeout = new TimeSpan(0, 0, 8);
                client.DefaultRequestHeaders.Add("ContentType", "text/text");
                client.DefaultRequestHeaders.Add("User-Agent", TelegramWorker.ObservatoryTelegramVersion);
                try
                {
                    HttpResponseMessage result = client.GetAsync(url).Result;
                    resultStr = result.Content.ReadAsStringAsync().Result;
                    if (result.IsSuccessStatusCode && resultStr != "" && resultStr != "[]" && resultStr != "{}")
                    {
                        TelegramResponse tg = JsonSerializer.Deserialize<TelegramResponse>(resultStr);
                        if (tg.Ok)
                        {
                            if (tg.Result.Count == 0)
                            {
                                Logger.AppendLog("Send a message to bot and then tick Force Update in Settings", LogFile, TelegramWorker.ObservatoryTelegramVersion);
                            }
                            else if (tg.Result[0].Message.Chat.Id == 0)
                            {
                                Logger.AppendLog("No Chat ID found", LogFile, TelegramWorker.ObservatoryTelegramVersion);
                            }
                            else
                            {
                                ChatID = tg.Result[0].Message.Chat.Id.ToString();
                                string message = $"<b>Observatory</b>\r\nThis is a message from <i>{TelegramWorker.ObservatoryTelegramVersion}</i> sent to chat id {ChatID}";
                                var request = new System.Net.Http.HttpRequestMessage
                                {
                                    Method = System.Net.Http.HttpMethod.Get,
                                    RequestUri = new Uri($"https://api.telegram.org/bot{APIKey}/sendMessage?chat_id={ChatID}&text={message}&parse_mode=HTML")
                                };

                                System.Net.Http.HttpResponseMessage result2 = client.SendAsync(request).Result;
                                if (!result.IsSuccessStatusCode)
                                {
                                    Logger.AppendLog($"Error {result2.IsSuccessStatusCode}, {result2.StatusCode}, {result2.ReasonPhrase}", LogFile, TelegramWorker.ObservatoryTelegramVersion);
                                    ChatID = "";
                                }
                                else
                                {
                                    Logger.AppendLog($"Chat ID set to {ChatID}", LogFile, TelegramWorker.ObservatoryTelegramVersion);
                                }
                            }
                        }
                        else
                        {
                            Logger.AppendLog($"Error {tg.ErrorCode} {tg.Description}", LogFile, TelegramWorker.ObservatoryTelegramVersion);
                        }
                    }
                    else
                    {
                        Logger.AppendLog($"Error {result.IsSuccessStatusCode}, {result.StatusCode}, {result.ReasonPhrase}", LogFile, TelegramWorker.ObservatoryTelegramVersion);
                    }
                }
                catch (Exception ex)
                {
                    Logger.AppendLog($"Error {ex.Message}",LogFile, TelegramWorker.ObservatoryTelegramVersion);
                }
            }
        }
    }
}
