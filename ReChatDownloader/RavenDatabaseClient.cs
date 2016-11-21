using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void StoreVideo(Video video)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(video);
                session.SaveChanges();
            }
        }

        public void StoreTimespan(VideoTimespan timespan)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(timespan);
                session.SaveChanges();
            }

        }

        public List<Video> GetVideos()
        {
            using (var session = _documentStore.OpenSession())
            {
                var video =
                    session.Query<Video>().ToList();
                return video;
            }
        }

        public Video GetVideo(int videoId)
        {
            using (var session = _documentStore.OpenSession())
            {
                var video =
                    session.Load<Video>(videoId);
                return video;
            }
        }

        public VideoTimespan GetTimespan(int id)
        {
            using (var session = _documentStore.OpenSession())
            {
                var video =
                    session.Load<VideoTimespan>(id);
                return video;
            }
        }

    }

}
