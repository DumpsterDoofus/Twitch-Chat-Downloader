## Twitch Chat Downloader

Have you ever downloaded a [Twitch](https://twitch.tv) video using [YouTube-DL](https://github.com/rg3/youtube-dl), watched it, and wished you could download the chat too? Now you can.

This console app downloads the chat on a Twitch video and converts it to an SRT file, so you can read chat as subtitles while watching the video. Here's it in action on VLC:

![](http://i.imgur.com/5thpdc8.jpg)

- Q: Why create this when [Chat Replay](https://help.twitch.tv/customer/portal/articles/2337148-chat-replay-faq) already exists?
 - A: Because Twitch Past Broadcasts are deleted after 30 days, and although you can download videos with YouTube-DL, at present you can't download the chat.

## How to build

1. Build solution in Visual Studio. 

2. The executable is at `..\ConsoleDownloader\bin\Debug\TwitchChatDownloader.exe`. 

You may need to replace the app.config's `TwitchClientId` in case the one I prepackaged in source control ever becomes invalid (but it should work for now). You can get these by [registering an application at Twitch](https://www.twitch.tv/kraken/oauth2/clients/new). Security note: [The Twitch Client ID is not a secret](https://dev.twitch.tv/docs/authentication/), which is why I leave it in source control.

In the future I may just make a prepackaged EXE so you don't have to build it.

## How to use 

Usage is `TwitchChatDownloader.exe [flags]`

Flags (case-insensitive):
- `-path` (required): Either a URL of a Twitch video or channel, or the physical path of a previously-downloaded JSON file.
- `-inputtype`: How the `path` gets processed.
 - `url` (default): Downloads a single video at the specified URL.
 - `file`: Path of a JSON file.
 - `pastbroadcasts`: Downloads all past broadcasts of the channel at the specified URL.
 - `highlights`: Downloads all highlights of the channel at the specified URL.
- `-outputtype`: How the chat gets saved.
 - `srt` (default): Messages are saved to an SRT file, which can be used as a subtitle track on video players. Messages stay onscreen for either 5 seconds or the time until the next message, whichever is longer. Usernames are colored with the same color as they do in chat.
 - `json`: Saves raw traffic in original form as received from Twitch's API. This is useful in the event I change the behavior of the SRT save process, since saving as SRT is "lossy" from an information standpoint.

### Examples

#### A single video, saving chat as SRT
```
TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype srt
TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 (behaves same as previous)
```

#### A single video, saving chat as JSON
```
TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype url -outputtype json
```

#### Converting a JSON file to SRT
```
TwitchChatDownloader -path "JsonFromExample3.json" -inputtype file -outputtype srt`
```

#### Saving all video highlights from a channel as SRT

```
TwitchChatDownloader -path https://www.twitch.tv/zfg1 -inputtype highlights -outputtype srt
```

#### Saving all past broadcasts from a channel as JSON

```
TwitchChatDownloader -path https://www.twitch.tv/zfg1/v/69027652 -inputtype pastbroadcasts -outputtype json
```
