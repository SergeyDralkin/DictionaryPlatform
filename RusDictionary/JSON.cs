using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

namespace RusDictionary
{
    class JSON
    {        
        static string ReturnJSON = null;
        public static List<Movie> Result = new List<Movie>();
        public static void Send(string url)
        {
            ReturnJSON = null;
            WebRequest req = WebRequest.Create(url.Replace(" ", "%20"));
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            ReturnJSON = sr.ReadToEnd();
            sr.Close();
            Result = Decode();            
        }
        public static List<Movie> Decode()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<Movie>>(ReturnJSON);
        }
    }
    public class Movie
    {        
        public string Marker { get; set; }
        public string CardSeparator { get; set; }
        public string NumberBox { get; set; }
        public string Symbol { get; set; }
        public string img { get; set; }
        public string imgText { get; set; }
        public string Notes { get; set; }        
    }
}
