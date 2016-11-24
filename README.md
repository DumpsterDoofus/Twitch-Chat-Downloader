# ReChatDownloader

Have you ever downloaded a [Twitch](https://twitch.tv) video using [YouTube-DL](https://github.com/rg3/youtube-dl), watched it, and wished you could download the chat too? Now you can.

This console app downloads the chat on a Twitch video and converts it to an SRT file, so you can read chat as subtitles while watching the video. Here's it in action on VLC:

![](http://i.imgur.com/5thpdc8.jpg)

- Q: Why create this when [Chat Replay](https://help.twitch.tv/customer/portal/articles/2337148-chat-replay-faq) already exists?
 - A: Because Twitch Past Broadcasts are deleted after 30 days, and although you can download them with YouTube-DL, at present you can't download the chat.

## How to build

1. Build solution in Visual Studio. 

2. The executable is at `..\ConsoleDownloader\bin\Debug\TwitchChatDownloader.exe`. 

You may need to replace the app.config's `TwitchClientId` in case the one I prepackaged in source control ever becomes invalid (but it should work for now). You can get these by [registering an application at Twitch](https://www.twitch.tv/kraken/oauth2/clients/new). Security note: [The Twitch Client ID is not a secret](https://dev.twitch.tv/docs/authentication/), which is why I leave it in source control.

In the future I may just make a prepackaged EXE so you don't have to build it.

## How to use 

Usage is `TwitchChatDownloader.exe [flags]`

Flags:
- `-path` (required): Either a URL of a Twitch video or the physical path of a previously-downloaded JSON file.
- `-inputtype`: Either "file" or "url". Defaults to "url".
- `-outputtype`: Either "srt" or "json". Defaults to "srt".

Examples:

1. `TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype srt`
2. `TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652` (same as example 1)
3. `TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype json`
4. `TwitchChatDownloader -path "JsonFromExample3.json" -inputtype file -outputtype srt`
