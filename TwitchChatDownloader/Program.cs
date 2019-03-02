using System;
using System.Threading.Tasks;
using CommandLine;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using TwitchChatDownloader.Implementations;
using TwitchChatDownloader.Models;
using SimpleInjector;
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
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                Result.Combine(
                    configuration.GetValidatable<SrtSettings>(out var srtSettings), 
                    configuration.GetValidatable<CommentsCacheSettings>(out var commentsCacheSettings), 
                    configuration.GetValidatable<SrtFileSettings>(out var srtFileSettings), 
                    configuration.GetValidatable<TwitchSettings>(out var twitchSettings))
                    .OnFailure(error =>
                    {
                        logger.Fatal($"Settings validation failed: {error}");
                        Environment.Exit(-1);
                    });
                using (var container = new Container())
                {
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
                    var optionsProcessor = container.GetInstance<OptionsProcessor>();
                    await Parser.Default.ParseArguments<Options>(args)
                        .MapResult(options => optionsProcessor.Process(options), errors =>
                        {
                            logger.Fatal($"Command line parsing failed. Errors: {string.Concat(errors)}");
                            return Task.CompletedTask;
                        });
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Something really bad happened: {exception}");
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
            var twitchApi = new TwitchAPI(loggerFactory, settings: apiSettings);
            return twitchApi;
        }
    }
}
