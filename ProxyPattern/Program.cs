using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyPattern
{
    public interface IVideoService
    {
        List<int> GetListVideo();
        int GetVideoInfo(int id);
        void DownloadVideo(int id);
    }

    class VideoService : IVideoService
    {
        // дорогое создание объекта
        // нет проверки на дублирование кэширования видео
        // может вернуть пустой список
        List<int> videos = new List<int>();
        public void DownloadVideo(int id)
        {
            videos.Add(id);
        }

        public List<int> GetListVideo()
        {
            return videos;
        }

        public int GetVideoInfo(int id)
        {
            return videos.Find(i => i == id);
        }
    }
    class ProxyVideoService : IVideoService
    {
        VideoService videoService;
        public void DownloadVideo(int id)
        {
            if (videoService == null)
            {
                videoService = new VideoService();
            }

            if (!videoService.GetListVideo().Exists(x => x == id))
            {
                videoService.DownloadVideo(id);
            } else
            {
                Console.WriteLine("Видео уже загружено");
            }
        }

        public List<int> GetListVideo()
        {
            try
            {
                if (videoService == null)
                {
                    throw new NullReferenceException("Сервис не инициализирован");
                }

                return videoService.GetListVideo();
            } catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public int GetVideoInfo(int id)
        {
            try
            {
                if (videoService == null)
                {
                    throw new NullReferenceException("Сервис не инициализирован");
                }

                if (videoService.GetListVideo().Exists(x => x == id))
                {
                    return videoService.GetVideoInfo(id);
                } else
                {
                    Console.WriteLine("Нет данного видео в списке загруженных");
                    return -1;
                }
            } catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Прокси Сервиса");
            IVideoService service = new ProxyVideoService();
            Console.WriteLine(service.GetVideoInfo(5)); 
            service.DownloadVideo(45);
            service.DownloadVideo(2);
            service.DownloadVideo(45);
            service.DownloadVideo(8);
            Console.WriteLine(service.GetVideoInfo(8));
            service.GetListVideo().ForEach(item => Console.WriteLine("item: " + item));

            Console.WriteLine("Сервис");
            service = new VideoService();
            Console.WriteLine(service.GetVideoInfo(5)); 

            Console.Read();
        }
    }
}
