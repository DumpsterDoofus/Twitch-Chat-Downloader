# Twitch Chat Downloader

[![Build Status](https://dev.azure.com/peterrichter128/Twitch-Chat-Downloader/_apis/build/status/DumpsterDoofus.Twitch-Chat-Downloader?branchName=master)](https://dev.azure.com/peterrichter128/Twitch-Chat-Downloader/_build/latest?definitionId=2&branchName=master)

Have you ever downloaded a [Twitch](https://twitch.tv) video using [YouTube-DL](https://github.com/rg3/youtube-dl), watched it, and wished you could download the chat too? Now you can.

This console app downloads the chat on a Twitch video and converts it to an SRT file, so you can read chat as subtitles while watching the video. Here's it in action on VLC:

![](http://i.imgur.com/5thpdc8.jpg)

- Q: Why create this when [Chat Replay](https://help.twitch.tv/customer/portal/articles/2337148-chat-replay-faq) already exists?
 - A: Because Twitch Past Broadcasts are deleted after 30 days, and although you can download videos with YouTube-DL, at present you can't download the chat.

## How to use

### Setup

1. Ensure the [.NET Core runtime](https://dotnet.microsoft.com/download) is on the path.
2. Download and unzip the [release](https://github.com/DumpsterDoofus/Twitch-Chat-Downloader/releases/download/1.0.0/TwitchChatDownloader.7z).
3. In PowerShell (or similar terminal), run `dotnet TwitchChatDownloader.dll --help` to display the help, printed below for convenience:

```
TwitchChatDownloader 1.0.0
Copyright (C) 2019 TwitchChatDownloader

  -v, --videoid      The video ID (for example, https://www.twitch.tv/videos/213105685 has ID 213105685). This will
                     download subtitles for a single video.

  -u, --username     The username (for example, https://www.twitch.tv/zfg1 has username zfg1). Use this if you want
                     to download subtitles for all of a user's videos of a certain type.

  -t, --videotype    (Default: All) The type of videos to download (only matters if a username is provided). Can be:
                     All (default), Upload, Archive, or Highlight.

  --help             Display this help screen.

  --version          Display version information.
```

### Examples

#### Downloading for a single video (https://www.twitch.tv/videos/69027652)

```
dotnet TwitchChatDownloader.dll --videoid 69027652
```

Example log output:

```
[12:51:59 INF] Getting info for video ID: 69027652
[12:51:59 INF] Got info for video "Double 46"
[12:51:59 INF] Saving video Double 46.
[12:51:59 INF] Getting comments for video Double 46, ID 69027652.
[12:52:00 INF] Got 149 comments for video.
[12:52:00 INF] Wrote SRT file to C:\Users\Peter\Desktop\TwitchChatDownloader\SRT\Double 46-v69027652.srt.
[12:52:00 INF] Done saving video chat.
```

In this case, it saved off a single SRT file (`Double 46-v69027652.srt`) containing the comments as subtitles.

#### Downloading all highlights from a user's channel (https://www.twitch.tv/zfg1)

```
dotnet TwitchChatDownloader.dll --username zfg1 --videotype highlight
```

This will download a bunch of SRT files (for each highlight-type video).

### Advanced Configuration

The `appsettings.json` contains configuration settings that you can edit, such as:

- `SrtFileSettings:OutputDirectoryPath`: This is where SRT files are saved.
- `SrtSettings` has a few options:
    - `MaxMessagesOnscreen`: Puts an upper bound on how many comments are visible at any time. Useful for very chatty streams, so that your video doesn't get obscured by a wall of text.
    - `MaxSecondsOnscreen`: Upper bound on long a message can be visible. Messages may be visible less than this time if messages are coming in quickly, since `MaxMessagesOnscreen` will "bump off" older messages with newer ones.
    - `DeltaMilliseconds`: When an newer message "bumps off" an older message, the older message will disappear a few milliseconds before the newer one appears. This controls that timing. This may be useful for video players where having zero spacing between messages (the default behavior) causes them to "climb up the screen".
- `CommentsCacheSettings:CacheDirectoryPath`: This app reaches out to Twitch's API to get comments for a video. Comments for videos are cached on disk, so that subsequent runs for the same video don't have to reach out over the network again, speeding up subsequent runs. This is useful if you've tweaked some `SrtSettings`, and want to rerun a download and not have to wait as long.
- `Serilog:WriteTo`: Contains [Serilog](https://github.com/serilog/serilog-settings-configuration) settings such as the file log path, and verbosity levels. May be useful for debugging purposes.
- `TwitchSettings:ClientId`: You can get a different Client ID by [registering an application at Twitch](https://dev.twitch.tv/docs/authentication/#registration). I've included a valid one, so you shouldn't have to change this, unless it gets revoked due to abuse.
