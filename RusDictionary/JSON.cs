using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace RusDictionary
{
    class JSON
    {        
        static string ReturnJSON = null;        
        /// <summary>
        /// Метод отправки на сервер запроса
        /// </summary>
        /// <param name="flag">Тип запроса</param>
        /// <param name="query">Ссылка с запросом</param>
        public static void Send(string query, JSONFlags flag)
        {
            SendURL(flag);
            var request = (HttpWebRequest)WebRequest.Create(MainForm.URL);

            var postData = "property=" + Uri.EscapeDataString(SendURL(flag));
            postData += "&query=" + HttpUtility.UrlEncode(query);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            Stream responseString = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseString);
            ReturnJSON = sr.ReadToEnd();
        }
        
        public static string SendURL(JSONFlags flag)
        {            
            if (MainForm.URL == null)
            {
                if (MainForm.Port == null || MainForm.Port == "")
                {
                    MainForm.URL = "http://" + MainForm.IP;
                }
                else
                {
                    MainForm.URL = "http://" + MainForm.IP + ":" + MainForm.Port;
                }
            }
            switch (flag)
            {
                case JSONFlags.Select:
                    {
                        return "select";
                    }
                case JSONFlags.Update:
                    {
                        return "update";
                    }
                case JSONFlags.Delete:
                    {
                        return "delete";
                    }
                case JSONFlags.Insert:
                    {
                        return "insert";
                    }
                case JSONFlags.Truncate:
                    {
                        return "truncate";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }            
        }
        /// <summary>
        /// Метод декодирования JSON-запроса 
        /// </summary>
        /// <returns></returns>
        public static List<JSONArray> Decode()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            return serializer.Deserialize<List<JSONArray>>(ReturnJSON);
        }
    }
    /// <summary>
    /// Список для полей из запроса. Для каждого модуля выделен свой регион, где должны храниться поля, 
    /// предназначенные только для работы этого модуля. Регион назван по названию модуля.
    /// </summary>
    public class JSONArray
    {
        public string ID { get; set; }
        #region MainModule       
        public string FIO { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public string CanSelect { get; set; }
        public string CanUpdate { get; set; }
        public string CanInsert { get; set; }
        public string CanDelete { get; set; }
        #endregion
        #region CardIndexModule     
        public string Card { get; set; }
        public string Marker { get; set; }
        public string CardSeparator { get; set; }
        public string NumberBox { get; set; }
        public string Symbol { get; set; }
        public string Img { get; set; }        
        public string ImgText { get; set; }
        public string Word { get; set; }
        public string RelatedCombinations { get; set; }
        public string Value { get; set; }
        public string SourceCode { get; set; }
        public string SourceClarification { get; set; }
        public string Pagination { get; set; }
        public string SourceDate { get; set; }
        public string SourceDateClarification { get; set; }
        public string Notes { get; set; }
        #endregion
        #region IndexModule         
        #endregion
        #region WordSearchModule
        public string Name { get; set; }
        public string Semantic { get; set; }
        public string Partofspeech { get; set; }
        public string Rod { get; set; }
        public string Num { get; set; }
        public string Definition { get; set; }
        public string Example { get; set; }
        public string Compare { get; set; }
        public string Mainword { get; set; }
        #endregion
    }
}