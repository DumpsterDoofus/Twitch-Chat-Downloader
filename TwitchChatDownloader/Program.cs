using System;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using TwitchChatDownloader.Implementations;
using TwitchChatDownloader.Models;
using SimpleInjector;
using TwitchChatDownloader.Extensions;
using TwitchChatDownloader.Interfaces;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using ILogger = Serilog.ILogger;

namespace TwitchChatDownloader
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                var twitchChatDownloader = ComposeObjectGraph();
                await Parser.Default.ParseArguments<Options>(args)
                    .MapResult(options => twitchChatDownloader.Process(options), errors =>
                    {
                        Console.WriteLine($"Command line parsing failed. Errors:\n{string.Join('\n', errors)}");
                        return Task.CompletedTask;
                    });
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Something really bad happened. Application shutting down.\n{exception}");
                Environment.Exit(-1);
            }
        }

        private static TwitchChatDownloader ComposeObjectGraph()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            var srtSettings = configuration.GetValidatableOrThrow<SrtSettings>();
            var commentsCacheSettings = configuration.GetValidatableOrThrow<CommentsCacheSettings>();
            var srtFileSettings = configuration.GetValidatableOrThrow<SrtFileSettings>();
            var twitchSettings = configuration.GetValidatableOrThrow<TwitchSettings>();
            using (var container = new Container())
            {
                container.RegisterInstance<ILogger>(logger);
                container.RegisterInstance<ISrtSettings>(srtSettings);
                container.RegisterInstance<ISrtFileSettings>(srtFileSettings);
                container.RegisterInstance<ITwitchSettings>(twitchSettings);
                container.RegisterInstance<ICommentsCacheSettings>(commentsCacheSettings);
                container.RegisterInstance(CreateTwitchApi(twitchSettings, logger));
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
                container.RegisterSingleton<ISrtLineWriter, SrtLineWriter>();
                container.Verify();
                return container.GetInstance<TwitchChatDownloader>();
            }
        }

        private static TwitchAPI CreateTwitchApi(ITwitchSettings twitchSettings, ILogger logger)
        {
            var apiSettings = new ApiSettings
            {
                ClientId = twitchSettings.ClientId
            };
            var loggerFactory = new LoggerFactory()
                .AddSerilog(logger);
            return new TwitchAPI(loggerFactory, settings: apiSettings);
        }
    }
}
