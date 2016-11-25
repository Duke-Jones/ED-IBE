using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Globalization;
using System.Collections;

namespace IBE.Web
{
    public class CompanionInterface
    {
        public enum enState
        {
            ci_not_connected,
            ci_error,
            ci_verification,
            ci_logged_in
        }

        public enState ConnectionState { get; set; }


        public CompanionInterface()
        {
            ConnectionState = enState.ci_not_connected;
        }

        //public void LogIn(String Username, String Password)
        //{
        //    try
        //    {
        //        ExtendedWebClient WebClient = new ExtendedWebClient();

            
        //        String Response = WebClient.Post("https://companion.orerve.net/user/login",new
        //                                        {
        //                                            email    = Username,
        //                                            password = Password
        //                                        });     

        //        if(Response.Contains("server error"))
        //            ConnectionState = enState.ci_error;

        //        else if(Response.Contains("Password"))
        //            ConnectionState = enState.ci_not_connected;

        //        else if(Response.Contains("Verification Code"))
        //            ConnectionState = enState.ci_verification;

        //        else
        //            ConnectionState = enState.ci_logged_in;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while connecting the frontier server", ex);
        //    }
       

        //}

        //public void Verify(String Code)
        //{
            
        //}

        private const String Path_Companion   = "https://companion.orerve.net";

        private const String LogInPath        = Path_Companion + "/user/login";
        private const String VerifyPath       = Path_Companion + "/user/confirm";
        private const String DataPath         = Path_Companion + "/profile";

        private CookieContainer EDC_Cookies;
        private CookieContainer EDC_Cookies2;



        HttpWebRequest WebRequest;

        public void Init()
        {
            try
            {
                EDC_Cookies = new CookieContainer(10);
                EDC_Cookies2 = new CookieContainer(10);

                //readCookies(@"C:\temp\EDCookies.txt", EDC_Cookies);
            }
            catch (Exception ex)
            {
                throw new Exception("error when init CompanionInterface", ex);
            }
        }

        public void LogIn(String Username, String Password)
        {
            byte[] byteArray;
            string postData;
            Stream newStream;
            HttpWebResponse response;
            Stream receiveStream;
            StreamReader readStream;
            String answer;

            try
            {


                //WebRequest                      = (HttpWebRequest)HttpWebRequest.Create(LogInPath);
                //WebRequest.UserAgent            = "'Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257'";
                //WebRequest.CookieContainer      = EDC_Cookies;
                //WebRequest.AllowAutoRedirect    = false;
                //WebRequest.Method               = "GET";
                //WebRequest.ContentType          = "application/x-www-form-urlencoded";

                //response = (HttpWebResponse) WebRequest.GetResponse();

                //receiveStream               = response.GetResponseStream();

                //// Pipes the stream to a higher level stream reader with the required encoding format. 
                //readStream                  = new StreamReader (receiveStream, Encoding.UTF8);

                //answer                      = readStream.ReadToEnd();
                //Console.WriteLine("Response stream received.");
                //Console.WriteLine(answer);

                //response.Close();
                //readStream.Close();

                /***********************************************/

                WebRequest                      = (HttpWebRequest)HttpWebRequest.Create(LogInPath);
                WebRequest.UserAgent            = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257";
                WebRequest.CookieContainer      = EDC_Cookies;

                //WebRequest.Credentials      = new NetworkCredential(Username, Password);
                WebRequest.Method           = "POST";
                WebRequest.ContentType      = "application/x-www-form-urlencoded";

                //WebRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
                WebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                WebRequest.Accept           = "*/*"; 

                postData                    = String.Format("email={0}&password={1}", Username, Password);
                //byteArray                   = Encoding.ASCII.GetBytes(postData);
                byteArray                   = Encoding.UTF8.GetBytes(postData);
                WebRequest.ContentLength    = byteArray.Length;   
                newStream                   = WebRequest.GetRequestStream(); //open connection
                newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
                newStream.Close();

                response                    = (HttpWebResponse)WebRequest.GetResponse();

                Console.WriteLine ("Content length is {0}", response.ContentLength);
                Console.WriteLine ("Content type is {0}", response.ContentType);

                                // Get the stream associated with the response.
                receiveStream               = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                readStream                  = new StreamReader (receiveStream, Encoding.UTF8);

                answer                      = readStream.ReadToEnd();

                BugFix_CookieDomain(WebRequest.CookieContainer);


                var CookieCollection = response.Cookies;

                if(true)
                { 
                    Console.WriteLine("Response stream received.");
                    Console.WriteLine(answer);
                    System.IO.File.WriteAllText(@"C:\temp\EDLogin1.html", answer);
                }

                if(CultureInfo.CurrentCulture.CompareInfo.IndexOf(answer, "server error", CompareOptions.IgnoreCase) >= 0)
                {
                    
                }
                else if(CultureInfo.CurrentCulture.CompareInfo.IndexOf(answer, "password", CompareOptions.IgnoreCase) >= 0)
                {
                
                }
                else if(CultureInfo.CurrentCulture.CompareInfo.IndexOf(answer, "verification code", CompareOptions.IgnoreCase) >= 0)
                {
                    writeCookies(@"C:\temp\EDCookies.txt", EDC_Cookies, Path_Companion);
                }

                response.Close();
                readStream.Close();



                
            }
            catch (Exception ex)
            {
                throw new Exception("error when login", ex);
            }
        }



        public void Verify(String Username, String Password, String code)
        {
            byte[] byteArray;
            string postData;
            Stream newStream;
            HttpWebResponse response;
            Stream receiveStream;
            StreamReader readStream;
            String answer;

            try
            {
                //WebRequest                      = (HttpWebRequest)HttpWebRequest.Create(VerifyPath);
                //WebRequest.UserAgent            = "'Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257'";
                //WebRequest.CookieContainer      = EDC_Cookies;
                //WebRequest.AllowAutoRedirect    = false;
                //WebRequest.Method               = "GET";
                //WebRequest.ContentType          = "application/x-www-form-urlencoded";

                //response = (HttpWebResponse) WebRequest.GetResponse();

                //receiveStream               = response.GetResponseStream();

                //// Pipes the stream to a higher level stream reader with the required encoding format. 
                //readStream                  = new StreamReader (receiveStream, Encoding.UTF8);

                //answer                      = readStream.ReadToEnd();
                //Console.WriteLine("Response stream received.");
                //Console.WriteLine(answer);

                //response.Close();
                //readStream.Close();

                /***********************************************/


                WebRequest                  = (HttpWebRequest)HttpWebRequest.Create(VerifyPath);
                WebRequest.UserAgent        = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257";
                WebRequest.CookieContainer  = EDC_Cookies;

                WebRequest.Method           = "POST";
                WebRequest.ContentType      = "application/x-www-form-urlencoded";
                //WebRequest.Credentials      = new NetworkCredential(Username, Password);
                WebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                WebRequest.Accept           = "*/*"; 

                postData                    = String.Format("code={0}", code);
                //byteArray                   = Encoding.ASCII.GetBytes(postData);
                byteArray                   = Encoding.UTF8.GetBytes(postData);
                 
                WebRequest.ContentLength    = byteArray.Length;   
                newStream                   = WebRequest.GetRequestStream(); //open connection
                newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
                newStream.Close();

                response                    = (HttpWebResponse)WebRequest.GetResponse();

                var MeineCookies            = response.Cookies;

                Console.WriteLine ("Content length is {0}", response.ContentLength);
                Console.WriteLine ("Content type is {0}", response.ContentType);

                                // Get the stream associated with the response.
                receiveStream               = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                readStream                  = new StreamReader (receiveStream, Encoding.UTF8);

                answer                      = readStream.ReadToEnd();   

                if(true)
                { 
                    Console.WriteLine("Response stream received.");
                    Console.WriteLine(answer);
                    System.IO.File.WriteAllText(@"C:\temp\EDLogin2.html", answer);
                }

                writeCookies(@"C:\temp\EDCookies.txt", EDC_Cookies, Path_Companion);
                readStream.Close();
                receiveStream.Close();

                
            }
            catch (Exception ex)
            {
                throw new Exception("error when login", ex);
            }
        }

        public void getData(String Username, String Password)
        {
            byte[] byteArray;
            string postData;
            Stream newStream;
            HttpWebResponse response;
            Stream receiveStream;
            StreamReader readStream;
            String answer;

            try
            {


                WebRequest                      = (HttpWebRequest)HttpWebRequest.Create(DataPath);
                WebRequest.UserAgent            = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257";
                WebRequest.CookieContainer      = EDC_Cookies;
                WebRequest.AllowAutoRedirect    = false;
                WebRequest.Method               = "GET";
                WebRequest.ContentType          = "application/x-www-form-urlencoded";
                WebRequest.Credentials          = new NetworkCredential(Username, Password);

                response = (HttpWebResponse) WebRequest.GetResponse();

                receiveStream               = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                readStream                  = new StreamReader (receiveStream, Encoding.UTF8);

                answer                      = readStream.ReadToEnd();
                Console.WriteLine("Response stream received.");
                Console.WriteLine(answer);

                response.Close();
                readStream.Close();



                response.Close();
                readStream.Close();



                
            }
            catch (Exception ex)
            {
                throw new Exception("error when login", ex);
            }
        }

        private void readCookies(String Filename, CookieContainer Cookies)
        {
            try 
	        {	        
                String line;
                String[] parts;

                // Read the file and display it line by line.
                if(File.Exists(Filename))
                { 
                    System.IO.StreamReader file = new System.IO.StreamReader(Filename);
                    while((line = file.ReadLine()) != null)
                    {
                        parts = line.Split(new char[] {';'});

                        if(parts.GetUpperBound(0) == 6)
                        {
                            Cookie NewCookie = new Cookie() { Path      = parts[0], 
                                                              Domain    = parts[1], 
                                                              Secure    = Boolean.Parse(parts[2]), 
                                                              Expires   = DateTime.Parse(parts[3]), 
                                                              Version   = Int32.Parse(parts[4]), 
                                                              Name      = parts[5], 
                                                              Value     = parts[6]};
                            Cookies.Add(NewCookie);
                        }
                    }

                    file.Close();
                }

	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while loading cookies", ex);
	        }
        }

        private void writeCookies(String Filename, CookieContainer Cookies, String Domain)
        {
            try 
	        {	        
                String line;

                // Read the file and display it line by line.
                System.IO.StreamWriter file = new System.IO.StreamWriter(Filename, false);

                foreach (Cookie currentCookie in Cookies.GetCookies(new Uri(Domain)))
                {
                    line = String.Format("{0};{1};{2};{3};{4};{5};{6}", currentCookie.Path.ToString(), 
                                                                        currentCookie.Domain.ToString(),    
                                                                        currentCookie.Secure.ToString(),    
                                                                        currentCookie.Expires.ToString(),   
                                                                        currentCookie.Version.ToString(),   
                                                                        currentCookie.Name.ToString(),      
                                                                        currentCookie.Value.ToString());     
                    file.WriteLine(line);
                }

                file.Close();

	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while loading cookies", ex);
	        }
        }

        private void BugFix_CookieDomain(CookieContainer cookieContainer)
        {
            System.Type _ContainerType = typeof(CookieContainer);
            Hashtable table = (Hashtable)_ContainerType.InvokeMember("m_domainTable",
                                       System.Reflection.BindingFlags.NonPublic |
                                       System.Reflection.BindingFlags.GetField |
                                       System.Reflection.BindingFlags.Instance,
                                       null,
                                       cookieContainer,
                                       new object[] { });
            ArrayList keys = new ArrayList(table.Keys);
            foreach (string keyObj in keys)
            {
                string key = (keyObj as string);
                if (key[0] == '.')
                {
                    string newKey = key.Remove(0, 1);
                    table[newKey] = table[keyObj];
                }
            }
        }
    }
}
