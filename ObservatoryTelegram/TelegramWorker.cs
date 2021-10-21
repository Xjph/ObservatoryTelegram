using Observatory.Framework;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Interfaces;
using Observatory.Framework.Files.ParameterTypes;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Observatory.Telegram
{
    public class TelegramWorker : IObservatoryNotifier
    {
        private IObservatoryCore Core;
        private System.Net.Http.HttpClient httpClient;
        public string Name => "Observatory Telegram Notifier";

        public string ShortName => "Telegram";

        public static string ObservatoryTelegramVersion => typeof(TelegramWorker).Assembly.GetName().Version.ToString();

        public string Version => ObservatoryTelegramVersion;

        public PluginUI PluginUI => null;

        private TelegramSettings settings;

        public TelegramWorker()
        {
            settings = new()
            {
                APIKey = ""
            };
        }

        public object Settings
        {
            get => settings;
            set => settings = (TelegramSettings)value;
        }


        public void Load(IObservatoryCore observatoryCore)
        {
            Core = observatoryCore;
            httpClient = new();
            Logger.AppendLog("Plugin Loaded", settings.LogFile,Version);
        }

        public void OnNotificationEvent(NotificationArgs noti)
        {
            if (settings.ChatID != "" && settings.ChatID !="0")
            {
                try
                {
                    string message = $"<b>{noti.Title}</b>\r\n{noti.Detail}";
                    var request = new System.Net.Http.HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Get,
                        RequestUri = new Uri($"https://api.telegram.org/bot{settings.APIKey}/sendMessage?chat_id={settings.ChatID}&text={message}&parse_mode=HTML")
                    };

                    System.Net.Http.HttpResponseMessage result = httpClient.SendAsync(request).Result;
                    if(!result.IsSuccessStatusCode)
                    {
                        Logger.AppendLog($"Error {result.IsSuccessStatusCode}, {result.StatusCode}, {result.ReasonPhrase}", settings.LogFile,Version);
                    }


                }
                catch (Exception ex)
                {
                    Logger.AppendLog(ex.Message, settings.LogFile,Version);
                }
            }
        }

    }

}
