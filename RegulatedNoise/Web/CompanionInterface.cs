using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;


namespace RegulatedNoise.Web
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

        public void LogIn(String Username, String Password)
        {
            ExtendedWebClient WebClient = new ExtendedWebClient();

            
            String Response = WebClient.Post("https://companion.orerve.net/user/login",new
                                            {
                                                email    = Username,
                                                password = Password
                                            });     

            if(Response.Contains("server error"))
                ConnectionState = enState.ci_error;

            else if(Response.Contains("Password"))
                ConnectionState = enState.ci_not_connected;

            else if(Response.Contains("Verification Code"))
                ConnectionState = enState.ci_verification;

            else
                ConnectionState = enState.ci_logged_in;
       

        }

        public void Verify(String Code)
        {
            
        }

        //HttpWebRequest WebRequest;

        //public void Init()
        //{
        //    try
        //    {
        //        WebRequest                  = (HttpWebRequest)HttpWebRequest.Create("https://companion.orerve.net/user/login");
        //        WebRequest.UserAgent        = "'Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257'";
        //        WebRequest.CookieContainer  = new CookieContainer(16);

        //        // Set some reasonable limits on resources used by this request
        //        WebRequest.MaximumAutomaticRedirections = 4;
        //        WebRequest.MaximumResponseHeadersLength = 4;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("error when init CompanionInterface", ex);
        //    }
        //}

        //public void LogIn(String Username, String Password)
        //{
        //    try
        //    {

        //        WebRequest.Credentials      = new NetworkCredential(Username, Password);
        //        WebRequest.Method           = "POST";

        //        string postData             = String.Format("email={0}&password={1}", Username, Password);
        //        byte[] byteArray            = Encoding.ASCII.GetBytes(postData);
        //        WebRequest.ContentLength    = byteArray.Length;   
        //        Stream newStream            = WebRequest.GetRequestStream(); //open connection
        //        newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
        //        newStream.Close();

        //        HttpWebResponse response = (HttpWebResponse)WebRequest.GetResponse();

        //        Console.WriteLine ("Content length is {0}", response.ContentLength);
        //        Console.WriteLine ("Content type is {0}", response.ContentType);

        //                        // Get the stream associated with the response.
        //        Stream receiveStream = response.GetResponseStream();

        //        // Pipes the stream to a higher level stream reader with the required encoding format. 
        //        StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);

        //        String answer = readStream.ReadToEnd();

        //        if(false)
        //        { 
        //            Console.WriteLine("Response stream received.");
        //            Console.WriteLine(answer);
        //        }

        //        response.Close ();
        //        readStream.Close ();
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("error when login", ex);
        //    }
        //}

        //public void Verify(String code)
        //{
        //    try
        //    {
        //        WebRequest.Method           = "POST";

        //        string postData             = String.Format("code={0}", code);
        //        byte[] byteArray            = Encoding.ASCII.GetBytes(postData);
        //        WebRequest.ContentLength    = byteArray.Length;   
        //        Stream newStream            = WebRequest.GetRequestStream(); //open connection
        //        newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
        //        newStream.Close();

        //        HttpWebResponse response    = (HttpWebResponse)WebRequest.GetResponse();

        //        if(false)
        //        { 
        //            Console.WriteLine ("Content length is {0}", response.ContentLength);
        //            Console.WriteLine ("Content type is {0}", response.ContentType);
        //        }

        //        // Get the stream associated with the response.
        //        Stream receiveStream = response.GetResponseStream();

        //        // Pipes the stream to a higher level stream reader with the required encoding format. 
        //        StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);

        //        String answer = readStream.ReadToEnd();

        //        if(false)
        //        { 
        //            Console.WriteLine("Response stream received.");
        //            Console.WriteLine(answer);
        //        }

        //        response.Close ();
        //        readStream.Close ();
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("error when login", ex);
        //    }
        //}

    }
}
