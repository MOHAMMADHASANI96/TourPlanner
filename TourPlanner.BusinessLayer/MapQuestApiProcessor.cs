using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class MapQuestApiProcessor
    {
        public HttpClient ApiClient { get; set; }
        private string RoutPhotoFolder;
        private string key;
        public MapQuestApiProcessor()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            key = ConfigurationManager.AppSettings["ApiKey"];
            RoutPhotoFolder = ConfigurationManager.AppSettings["RoutPhotoFolder"];
        }


        //return the direction url
        public string DirectionUrlCreate(string from, string to, string tourName)
        {
            string url = $"http://www.mapquestapi.com/directions/v2/route?key={ key }&from={ from }&to={ to }";
            return url;
        }

        //return two strings
        public async Task<Tuple<string, string>> DirectionApi(string url, string tourName)
        {
            using (HttpResponseMessage response = await ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string directionApiModel = await response.Content.ReadAsStringAsync();

                    Root rootObject = JsonConvert.DeserializeObject<Root>(directionApiModel);

                    //StaticMapApi(boundingBox, sessionId, tourName);
                    Tuple<string, string> tuple = RootInfo(rootObject);
                    return tuple;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public Tuple<string, string> RootInfo(Root rootObject)
        {

            string sessionId = rootObject.route.sessionId;
            string boundingBox = rootObject.route.boundingBox.ul.lat + "," +
                rootObject.route.boundingBox.ul.lng + "," +
                rootObject.route.boundingBox.lr.lat + "," +
                rootObject.route.boundingBox.lr.lng;

            Tuple<string, string> tuple = new Tuple<string, string>(sessionId, boundingBox);
            return tuple;
        }

        public string StaticUrlCreate(string sessionId, string boundingBox)
        {
            string size = "640,480";
            string zoom = "11";
            string url = $"https://www.mapquestapi.com/staticmap/v5/map?key={ key }&size={ size }&zoom={ zoom }&session={ sessionId }&boundingBox={ boundingBox }";
            return url;
        }

        public void StaticMapApi(string url, string tourName)
        {
            //create a new file with tour name
            string path = FilePathCreate(tourName);

            //save the route image in file
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        //save as Png
                        yourImage.Save(path, ImageFormat.Png);
                    }
                }

            }

        }

        public string FilePathCreate(string tourName)
        {
            string path = RoutPhotoFolder + "\\" + tourName + ".png";
            return path;
        }

        public void DeleteImage(TourItem touritem)
        {
            string path = FilePathCreate(touritem.Name);
            File.Delete(path);
        }
    }
}
