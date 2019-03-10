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
                using (var container = new Container())
                {
                    var twitchChatDownloader = container.ComposeObjectGraph();
                    await Parser.Default.ParseArguments<UserOptions, VideoOptions>(args)
                        .MapResult(
                            (UserOptions userOptions) => twitchChatDownloader.Process(userOptions),
                            (VideoOptions videoOptions) => twitchChatDownloader.Process(videoOptions),
                            errors =>
                            {
                                Console.WriteLine($"\nCommand line parsing failed. Errors:\n{string.Join('\n', errors)}");
                                return Task.CompletedTask;
                            })
                        .ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"\nSomething really bad happened. Application shutting down.\n{exception}");
                Environment.Exit(-1);
            }
        }

        private static TwitchChatDownloader ComposeObjectGraph(this Container container)
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            container.RegisterSingleton<ILogger>(() => logger);
            container.RegisterInstance<ISrtSettings>(configuration.GetValidatableOrThrow<SrtSettings>());
            container.RegisterInstance<ISrtFileSettings>(configuration.GetValidatableOrThrow<SrtFileSettings>());
            container.RegisterInstance<ICommentsCacheSettings>(configuration.GetValidatableOrThrow<CommentsCacheSettings>());
            container.RegisterInstance(CreateTwitchApi(configuration.GetValidatableOrThrow<TwitchSettings>(), logger));
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
            container.RegisterSingleton<ISrtLineWriter, SrtLineWriter>();
            container.Verify();
            return container.GetInstance<TwitchChatDownloader>();
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
