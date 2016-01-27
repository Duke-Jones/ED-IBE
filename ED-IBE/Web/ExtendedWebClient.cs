//****************************************************************
//****************************************************************
//  source: http://www.mycsharp.de/wbb2/thread.php?threadid=72666
//****************************************************************
//****************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Web;

namespace RegulatedNoise.Web
{
    /// <summary>
    /// Erweitert den System.Net.WebClient um Funktionen zum expliziten Ausführen von HTTP-GET- und HTTP-POST-Anfragen, bei denen Cookies für eine Session bestehen bleiben.
    /// </summary>
    // [System.Diagnostics.DebuggerStepThrough]
    public class ExtendedWebClient : WebClient
    {
        private CookieContainer cookieContainer = new CookieContainer();    // um bei Login-Szenarien auch eingeloggt zu bleiben


        /// <summary>
        /// Tritt ein, wenn der HttpWebRequest für eine Anfrage erstellt wurde.
        /// </summary>
        public event Action<HttpWebRequest> HttpWebRequestCreated;


        /// <summary>
        /// Ruft einen Wert ab, der bestimmt, ob die Gültigkeit von Cookies immer auf Verzeichnisebene gesetzt wird, oder legt diesen fest.
        /// </summary>
        public bool ForceApplyCookiesToDirectories { get; set; }


        /// <summary>
        /// Ruft einen Wert ab, der bestimmt, ob auch vom Betriebssystem als unsicher betrachtete (SSL-)Zertifikate erlaubt werden, oder legt diesen fest,
        /// </summary>
        public static bool IgnoreInvalidCertificates
        {
            get { return (ServicePointManager.ServerCertificateValidationCallback == ExtendedWebClient.ignoreInvalidCertificateValidationCallback); }
            set { ServicePointManager.ServerCertificateValidationCallback = (value) ? ExtendedWebClient.ignoreInvalidCertificateValidationCallback : null; }
        }
        private static RemoteCertificateValidationCallback ignoreInvalidCertificateValidationCallback = delegate { return true; };    // akzeptiert alle Zertifikate



        /// <summary>
        /// Gibt ein WebRequest-Objekt für die angegebene Ressource zurück.
        /// </summary>
        /// <param name="address">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);

            // CookieConainer immer mit einhängen
            if (webRequest is HttpWebRequest)
                lock (this)
                {
                    ((HttpWebRequest)webRequest).CookieContainer = this.cookieContainer;
                    if (this.HttpWebRequestCreated!=null)
                        this.HttpWebRequestCreated((HttpWebRequest)webRequest);
                }

            return webRequest;
        }



        /// <summary>
        /// Gibt die WebResponse für die angegebene WebRequest zurück.
        /// </summary>
        /// <param name="request">Eine WebRequest, mit der die Antwort abgerufen wird. </param>
        /// <returns>Eine WebResponse mit der Antwort auf die angegebene WebRequest. </returns>
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse webResponse = base.GetWebResponse(request);

            if (webResponse is HttpWebResponse)
                lock (this)
                {
                    // Bug umgehen, bei dem der CookieContainer keine Cookies speichert, die eine Beschränkung auf einen Host mit voranstehendem "." hat
                    foreach (Cookie cookie in ((HttpWebResponse)webResponse).Cookies)
                        this.cookieContainer.Add(cookie);

                    // anderen Bug umgehen, bei dem nicht alle nötigen Cookies übertragen werden
                    if (this.ForceApplyCookiesToDirectories)
                        foreach (Cookie cookie in this.cookieContainer.GetCookies(request.RequestUri))
                        {
                            if (cookie.Path.Length>1 && cookie.Path.Contains("/") && !cookie.Path.EndsWith("/"))
                                cookie.Path = cookie.Path.Remove(cookie.Path.LastIndexOf('/')+1);
                            this.cookieContainer.Add(cookie);
                        }
                }

            return webResponse;
        }



        /// <summary>
        /// Gibt das Ergebnis einer HTTP-POST-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Ein Objekt, dessen Properties als POST-Parameter benutzt werden.</param>
        /// <returns>Das Ergebnis einer HTTP-POST-Anfrage an die angegebene Ressource (als Zeichenkettenrepräsentation).</returns>
        public string Post(string uri,object parameters)
        {
            return this.post(uri,ExtendedWebClient.createParamString(ExtendedWebClient.objectToDictionary(parameters)));
        }


        /// <summary>
        /// Gibt ein WebRequest-Objekt für eine HTTP-POST-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Ein Objekt, dessen Properties als POST-Parameter benutzt werden.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        public WebRequest Post(Uri uri,object parameters)
        {
            return this.post(uri,ExtendedWebClient.createParamString(ExtendedWebClient.objectToDictionary(parameters)));
        }


        /// <summary>
        /// Gibt das Ergebnis einer HTTP-POST-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Auflistung der POST-Parameter.</param>
        /// <returns>Das Ergebnis einer HTTP-POST-Anfrage an die angegebene Ressource (als Zeichenkettenrepräsentation).</returns>
        public string Post(string uri,IDictionary<string,string> parameters)
        {
            return this.post(uri,ExtendedWebClient.createParamString(parameters));
        }


        /// <summary>
        /// Gibt ein WebRequest-Objekt für eine HTTP-POST-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Auflistung der POST-Parameter.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        public WebRequest Post(Uri uri,IDictionary<string,string> parameters)
        {
            return this.post(uri,ExtendedWebClient.createParamString(parameters));
        }


        /// <summary>
        /// Gibt das Ergebnis einer HTTP-POST-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Zeichenkettenrepräsentation aller Parameter-Wertepaare.</param>
        /// <returns>Das Ergebnis einer HTTP-POST-Anfrage an die angegebene Ressource (als Zeichenkettenrepräsentation).</returns>
        private string post(string uri,string parameters)
        {
            using (WebResponse webResponse = this.GetWebResponse(this.post(new Uri(uri),parameters)))
                return this.ReadWebResponse(webResponse);
        }


        /// <summary>
        /// Gibt ein WebRequest-Objekt für eine HTTP-POST-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Zeichenkettenrepräsentation aller Parameter-Wertepaare.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        private WebRequest post(Uri uri,string parameters)
        {
            WebRequest webRequest = this.GetWebRequest(uri);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = Encoding.Default.GetBytes(parameters).Length;
            using (StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream(),Encoding.Default))
                streamWriter.Write(parameters);

            return webRequest;
        }



        /// <summary>
        /// Gibt das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Ein Objekt, dessen Properties als GET-Parameter benutzt werden.</param>
        /// <returns>Das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource (als Zeichenkettenrepräsentation).</returns>
        public string Get(string uri,object parameters)
        {
            return this.get(uri,ExtendedWebClient.createParamString(ExtendedWebClient.objectToDictionary(parameters)));
        }


        /// <summary>
        /// Gibt ein WebRequest-Objekt für eine HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Ein Objekt, dessen Properties als GET-Parameter benutzt werden.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        public WebRequest Get(Uri uri,object parameters)
        {
            return this.get(uri,ExtendedWebClient.createParamString(ExtendedWebClient.objectToDictionary(parameters)));
        }


        /// <summary>
        /// Gibt das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Auflistung der GET-Parameter.</param>
        /// <returns>Das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource (als Zeichenkettenrepräsentation).</returns>
        public string Get(string uri,IDictionary<string,string> parameters)
        {
            return this.get(uri,ExtendedWebClient.createParamString(parameters));
        }


        /// <summary>
        /// Gibt ein WebRequest-Objekt für eine HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Auflistung der GET-Parameter.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        public WebRequest Get(Uri uri,IDictionary<string,string> parameters)
        {
            return this.get(uri,ExtendedWebClient.createParamString(parameters));
        }


        /// <summary>
        /// Gibt das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <returns>Das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource (als Zeichenkettenrepräsentation).</returns>
        public string Get(string uri)
        {
            return this.get(uri,null);
        }


        /// <summary>
        /// Gibt ein WebRequest-Objekt für eine HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        public WebRequest Get(Uri uri)
        {
            return this.get(uri,null);
        }


        /// <summary>
        /// Gibt das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Zeichenkettenrepräsentation aller Parameter-Wertepaare.</param>
        /// <returns>Das Ergebnis einer HTTP-GET-Anfrage an die angegebene Ressource (als Zeichenkettenrepräsentation).</returns>
        private string get(string uri,string parameters)
        {
            using (WebResponse response = this.GetWebResponse(this.get(new Uri(uri),parameters)))
                return this.ReadWebResponse(response);
        }


        /// <summary>
        /// Gibt ein WebRequest-Objekt für eine HTTP-GET-Anfrage an die angegebene Ressource zurück.
        /// </summary>
        /// <param name="uri">Ein URI, der die anzufordernde Ressource identifiziert.</param>
        /// <param name="parameters">Eine Zeichenkettenrepräsentation aller Parameter-Wertepaare.</param>
        /// <returns>Ein neues WebRequest-Objekt für die angegebene Ressource.</returns>
        private WebRequest get(Uri uri,string parameters)
        {
            if (!String.IsNullOrEmpty(parameters))
                uri = new Uri(uri.OriginalString+"?"+parameters);
            WebRequest webRequest = this.GetWebRequest(uri);
            webRequest.Method = "GET";
            return webRequest;
        }



        /// <summary>
        /// Liest den Inhalt einer WebResponse als Zeichenkettenrepräsentation aus.
        /// </summary>
        /// <param name="webResponse">Eine WebResponse.</param>
        /// <returns>Den Inhalt einer WebResponse als Zeichenkettenrepräsentation.</returns>
        public string ReadWebResponse(WebResponse webResponse)
        {
            Encoding encoding = (webResponse is HttpWebResponse && !String.IsNullOrEmpty(((HttpWebResponse)webResponse).CharacterSet)) ? Encoding.GetEncoding(((HttpWebResponse)webResponse).CharacterSet) : Encoding.Default;
            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(),encoding))
                return streamReader.ReadToEnd();
        }



        /// <summary>
        /// Erstellt die Zeichenkettenrepräsentation für Parameter-Wertepaare.
        /// </summary>
        /// <param name="parameters">Eine Auflistung der Parameter-Wertepaare.</param>
        /// <returns>Die Zeichenkettenrepräsentation für Parameter-Wertepaare.</returns>
        private static string createParamString(IDictionary<string,string> parameters)
        {
            List<string> result = new List<string>();
            //foreach (string key in parameters.Keys)
            //    result.Add(System.Web.HttpUtility.UrlEncode(key)+"="+HttpUtility.UrlEncode(parameters[key]));

            return String.Join("&",result.ToArray());
        }


        /// <summary>
        /// Erstellt eine Auflistung, die alle öffentlichen Eigenschaften eines Objekts als Schlüssel/Wertpaare enthält
        /// </summary>
        /// <param name="value">Ein Objekt, für das die Auflistung erstellt werden soll.</param>
        /// <returns>Eine Auflistung, die alle öffentlichen Eigenschaften eines Objekts als Schlüssel/Wertpaare enthält.</returns>
        private static Dictionary<string,string> objectToDictionary(object value)
        {
            Dictionary<string,string> result = new Dictionary<string,string>();
            foreach (PropertyInfo propertyInfo in value.GetType().GetProperties())
                if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length==0)
                    result.Add(propertyInfo.Name,propertyInfo.GetValue(value,null).ToString());
            return result;
        }
    }
}
