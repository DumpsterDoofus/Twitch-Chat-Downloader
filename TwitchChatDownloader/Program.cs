using System;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using TwitchChatDownloader.Models;
using SimpleInjector;
using TwitchChatDownloader.Config;
using TwitchChatDownloader.Extensions;
using TwitchChatDownloader.Interfaces;
using TwitchChatDownloader.Logging;
using TwitchChatDownloader.Readers.Comments;
using TwitchChatDownloader.Readers.Videos;
using TwitchChatDownloader.Writers.Srt;
using TwitchChatDownloader.Writers.SrtFile;
using TwitchChatDownloader.Writers.SrtLines;
using TwitchChatDownloader.Writers.Video;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using ILogger = Serilog.ILogger;

namespace TwitchChatDownloader
{
    internal static class Program
    {
        private static ILogger _logger;

        private static async Task Main(string[] args)
        {
            try
            {
                using var container = new Container();
                var twitchChatDownloader = container.ComposeObjectGraph();
                await Parser.Default.ParseArguments<UserOptions, VideoOptions>(args)
                    .MapResult(
                        (UserOptions userOptions) => twitchChatDownloader.Process(userOptions),
                        (VideoOptions videoOptions) => twitchChatDownloader.Process(videoOptions),
                        errors =>
                        {
                            _logger.Error($"Command line parsing failed. Errors:\n{string.Join('\n', errors)}");
                            return Task.CompletedTask;
                        });
            }
            catch (Exception exception)
            {
                _logger?.Error($"Something really bad happened. Application shutting down.\n{exception}");
                throw;
            }
        }

        private static TwitchChatDownloader ComposeObjectGraph(this Container container)
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            var loggingConfig = configuration.GetValidatableOrThrow<LogConfig>();
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.RollingFile(loggingConfig.LogDirectoryPath)
                .CreateLogger();
            container.RegisterSingleton(() => _logger);
            container.RegisterSingleton(typeof(ILog<>), typeof(Log<>));
            container.RegisterInstance<ISrtConfig>(configuration.GetValidatableOrThrow<SrtConfig>());
            container.RegisterInstance<ISrtFileConfig>(configuration.GetValidatableOrThrow<SrtFileConfig>());
            container.RegisterInstance<ICommentsCacheConfig>(configuration.GetValidatableOrThrow<CommentsCacheConfig>());
            container.RegisterInstance(CreateTwitchApi(configuration.GetValidatableOrThrow<TwitchConfig>(), _logger));
            container.RegisterSingleton<IVideoRetriever, VideoRetriever>();
            container.RegisterDecorator<IVideoRetriever, LoggingVideoRetriever>(Lifestyle.Singleton);
            container.RegisterSingleton<IVideosRetriever, VideosRetriever>();
            container.RegisterDecorator<IVideosRetriever, LoggingVideosRetriever>(Lifestyle.Singleton);
            container.RegisterSingleton<IVideoWriter, VideoWriter>();
            container.RegisterDecorator<IVideoWriter, LoggingVideoWriter>(Lifestyle.Singleton);
            container.RegisterSingleton<ICommentsRetriever, CommentsRetriever>();
            container.RegisterDecorator<ICommentsRetriever, CachingCommentsRetriever>(Lifestyle.Singleton);
            container.RegisterDecorator<ICommentsRetriever, LoggingCommentsRetriever>(Lifestyle.Singleton);
            container.RegisterSingleton<ISrtWriter, SrtWriter>();
            container.RegisterSingleton<ISrtFileWriter, SrtFileWriter>();
            container.RegisterDecorator<ISrtFileWriter, LoggingSrtFileWriter>(Lifestyle.Singleton);
            container.RegisterSingleton<ICommentsParser, CommentsParser>();
            container.Verify();
            return container.GetInstance<TwitchChatDownloader>();
        }

        private static TwitchAPI CreateTwitchApi(ITwitchConfig twitchConfig, ILogger logger)
        {
            var apiSettings = new ApiSettings
            {
                ClientId = twitchConfig.ClientId
            };
            var loggerFactory = new LoggerFactory()
                .AddSerilog(logger);
            return new TwitchAPI(loggerFactory, settings: apiSettings);
        }
    }
}
