using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace RusDictionary
{
    class JSON
    {        
        static string ReturnJSON = null;
        /// <summary>
        /// Метод отправки на сервер запроса
        /// </summary>
        /// <param name="url">Ссылка с запросом</param>
        public static void Send(string url)
        {
            ReturnJSON = null;
            WebRequest req = WebRequest.Create(url.Replace(" ", "%20"));
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            ReturnJSON = sr.ReadToEnd();
            sr.Close();           
        }
        /// <summary>
        /// Метод декодирования JSON-запроса 
        /// </summary>
        /// <returns></returns>
        public static List<JSONArray> Decode()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<JSONArray>>(ReturnJSON);
        }
    }
    /// <summary>
    /// Список для полей из запроса. Для каждого модуля выделен свой регион, где должны храниться поля, 
    /// предназначенные только для работы этого модуля. Регион назван по названию модуля.
    /// </summary>
    public class JSONArray
    {
        #region CardIndexModule       
        public string Marker { get; set; }
        public string CardSeparator { get; set; }
        public string NumberBox { get; set; }
        public string Symbol { get; set; }
        public string Img { get; set; }
        public string ImgText { get; set; }
        public string Notes { get; set; }
        #endregion
        #region IndexModule 
        #endregion
        #region WordSearchModule 
        #endregion
    }
}
