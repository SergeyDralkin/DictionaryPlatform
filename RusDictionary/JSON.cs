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
        /// <param name="flag">Тип запроса</param>
        /// <param name="query">Ссылка с запросом</param>
        public static void Send(string query, JSONFlags flag)
        {
            ReturnJSON = null;
            WebRequest req = WebRequest.Create(SendURL(flag) + WebUtility.UrlEncode(query));
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            ReturnJSON = sr.ReadToEnd();
            sr.Close();           
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
                        return MainForm.URL + "/?property=select&query=";
                    }
                case JSONFlags.Update:
                    {
                        return MainForm.URL + "/?property=update&query=";
                    }
                case JSONFlags.Delete:
                    {
                        return MainForm.URL + "/?property=delete&query=";
                    }
                case JSONFlags.Insert:
                    {
                        return MainForm.URL + "/?property=insert&query=";
                    }
                case JSONFlags.Truncate:
                    {
                        return MainForm.URL + "/?property=truncate&query=";
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
            return serializer.Deserialize<List<JSONArray>>(ReturnJSON);
        }
    }
    /// <summary>
    /// Список для полей из запроса. Для каждого модуля выделен свой регион, где должны храниться поля, 
    /// предназначенные только для работы этого модуля. Регион назван по названию модуля.
    /// </summary>
    public class JSONArray
    {
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
        public string Marker { get; set; }
        public string CardSeparator { get; set; }
        public string NumberBox { get; set; }
        public string Symbol { get; set; }
        public string Img { get; set; }
        public string Word { get; set; }
        public string ImgText { get; set; }
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
        public string Pomet { get; set; }
        public string Definition { get; set; }
        #endregion
    }
}
