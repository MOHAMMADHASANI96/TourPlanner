using NUnit.Framework;
using System;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.Test
{
    [TestFixture]
    public class MapQuestApiProcessTest
    {
        [Test]
        public void TestUrlCreate()
        {
            MapQuestApiProcessor mapQuest = new MapQuestApiProcessor();
            string url = "http://www.mapquestapi.com/directions/v2/route?key=&from=wien&to=linz";
            string urltest = mapQuest.DirectionUrlCreate("wien", "linz", "wienToLinz");

            Assert.AreEqual(url, urltest);
        }

        [Test]
        public void TestUrlCreate1()
        {
            MapQuestApiProcessor mapQuest = new MapQuestApiProcessor();
            string url = "http://www.mapquestapi.com/directions/v2/route?key=&from=wien&to=linz";
            string urltest = mapQuest.DirectionUrlCreate("berlin", "hamburg", "wienToLinz");

            Assert.AreNotEqual(url, urltest);
        }
        [Test]
        public void TestStaticUrlCreate()
        {
            MapQuestApiProcessor mapQuest = new MapQuestApiProcessor();
            string box = "48.9876";
            string sessionId = "test";
            string url = $"https://www.mapquestapi.com/staticmap/v5/map?key=&size=640,480&zoom=11&session=test&boundingBox=48.9876";
            string urltest = mapQuest.StaticUrlCreate(sessionId, box);

            Assert.AreEqual(url, urltest);
        }

        [Test]
        public void FileInfoTest()
        {
            MapQuestApiProcessor mapQuest = new MapQuestApiProcessor();
            Root root = new Root();
            Route route1 = new Route();
            BoundingBox box1 = new BoundingBox();
            Ul ul1 = new Ul();
            Lr lr1 = new Lr();
            root.route = route1;
            root.route.boundingBox = box1;
            root.route.boundingBox.ul = ul1;
            root.route.boundingBox.lr = lr1;

            double lrLat = root.route.boundingBox.lr.lat = 100;
            double lrLng = root.route.boundingBox.lr.lng = 200;
            double ulLat = root.route.boundingBox.ul.lat = 300;
            double ulLng = root.route.boundingBox.ul.lng = 400;
            string box = ulLat + "," + ulLng + "," + lrLat + "," + lrLng;
            string session = root.route.sessionId = "tour";
            Tuple<string, string> t = new Tuple<string, string>(session, box);

            Tuple<string, string> tuple = mapQuest.RootInfo(root);

            Assert.AreEqual(t, tuple);
        }

        [Test]
        public void FilePathCreateTest()
        {
            MapQuestApiProcessor mapQuest = new MapQuestApiProcessor();
            string url = "\\tourName.png";

            string testUrl = mapQuest.FilePathCreate("tourName");

            Assert.AreEqual(url, testUrl);
        }

        [Test]
        public void FilePathCreateTest1()
        {
            MapQuestApiProcessor mapQuest = new MapQuestApiProcessor();
            string url = "\\tour.png";

            string testUrl = mapQuest.FilePathCreate("tourName");

            Assert.AreNotEqual(url, testUrl);
        }
    }
}
