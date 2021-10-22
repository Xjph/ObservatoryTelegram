# Observatory Telegram

This is a plugin for Observatory Core.

It will let you have notifications sent to a Telegram Bot.

To use this plugin, copy the DLL to the `plugins` folder under your Observatory Core installation. You can find the folder easily by pushing the _Open Plugin Folder_ button in Observatory Core - but ensure you close Observatory before copying the DLL there.

You must also:
* Have (or create) a Telegram account
* Have (or create) a Telegram Bot. See [BotFather](https://core.telegram.org/bots#6-botfather) for more details.
* Once you have a bot, send it a message ("Hello", or whatever). The will allow the plugin to find the necessary Chat ID.
* Your bot will have an API Key in the form `110201543:AAHdqTcvCH1vGWJxfSeofSAs0K5PALDsaw` - enter this in the Settings for Observatory Telegram
* Once entered, tick the _Force ChatID Update_ tickbox
* If everything worked OK, you should receive a test message in Telegram
* If you didn't receive a test message, check the error log
