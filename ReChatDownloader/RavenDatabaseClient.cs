using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ApiIntegrations.Models.Twitch;
using Raven.Client.Document;

namespace ReChatDownloader
{
    public class RavenDatabaseClient
    {
        private readonly DocumentStore _documentStore;

        public RavenDatabaseClient()
        {
            _documentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = ConfigurationManager.AppSettings["RavenDatabaseName"]
            };
            _documentStore.Initialize();
        }

        public void StoreVideo(VideoWithChat video)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(video, video.Video._id);
                session.SaveChanges();
            }
        }

        public List<T> Get<T>()
        {
            using (var session = _documentStore.OpenSession())
            {
                var video =
                    session.Query<T>().ToList();
                return video;
            }
        }
    }
}
