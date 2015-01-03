using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;


namespace RegulatedNoise
{
    // taken from http://www.codeproject.com/Articles/452052/Build-Your-Own-Web-Server
    class SimpleWebserver
    {
        public bool Running = false; // Is it running?

        private const int timeout = 8; // Time limit for data transfers.
        private readonly Encoding _charEncoder = Encoding.UTF8; // To encode string
        private Socket _serverSocket; // Our server socket
        private string _contentPath; // Root path of our contents
        private Form1 _callingForm;
        private SingleThreadLogger _logger;

        // Content types that are supported by our server
        // You can add more...
        // To see other types: http://www.webmaster-toolkit.com/mime-types.shtml
        private readonly Dictionary<string, string> _extensions = new Dictionary<string, string>()
{ 
    //{ "extension", "content type" }
    { "htm", "text/html" },
    { "html", "text/html" },
    { "xml", "text/xml" },
    { "txt", "text/plain" },
    { "css", "text/css" },
    { "png", "image/png" },
    { "bmp", "image/bmp" },
    { "gif", "image/gif" },
    { "jpg", "image/jpg" },
    { "jpeg", "image/jpeg" },
    { "zip", "application/zip"}
};

        private Thread _requestListenerT;

        public bool Start(IPAddress ipAddress, int port, int maxNOfCon, string contentPath, Form1 callingForm)
        {
            _callingForm = callingForm;

            _logger = new SingleThreadLogger(ThreadLoggerType.Webserver);

            if (Running) return false; // If it is already running, exit.
            Running = true;
            //try
            {
                // A tcp/ip socket (ipv4)
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                               ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(ipAddress, port));
                _serverSocket.Listen(maxNOfCon);
                _serverSocket.ReceiveTimeout = timeout;
                _serverSocket.SendTimeout = timeout;
                
                this._contentPath = contentPath;
            }
            //catch { return false; }

            // Our thread that will listen connection requests
            // and create new threads to handle them.
            _requestListenerT = new Thread(() =>
            {
                while (Running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = _serverSocket.Accept();
                        // Create new thread to handle the request and continue to listen the socket.
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try { HandleRequest(clientSocket); }
                            catch (Exception)
                            {


                                try { clientSocket.Close(); }
                                catch (Exception ex2)
                                {
                                _logger.Log("Error in webserver start 2:", true);
                                _logger.Log(ex2.ToString(), true);
                                _logger.Log(ex2.Message, true);
                                _logger.Log(ex2.StackTrace, true);
                                if (ex2.InnerException != null)
                                    _logger.Log(ex2.InnerException.ToString(), true);

                                }
                            }
                        });
                        requestHandler.Start();
                    }
                    catch (Exception ex)
                    {
                        _logger.Log("Error in webserver start 1:", true);
                        _logger.Log(ex.ToString(), true);
                        _logger.Log(ex.Message, true);
                        _logger.Log(ex.StackTrace, true);
                        if (ex.InnerException != null)
                            _logger.Log(ex.InnerException.ToString(), true);
                    }
                }
            });
            _requestListenerT.Start();

            return true;
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
                try
                {
                    _serverSocket.Close();
                }
                catch (Exception ex)
                {
                    _logger.Log("Error in webserver start 1:", true);
                    _logger.Log(ex.ToString(), true);
                    _logger.Log(ex.Message, true);
                    _logger.Log(ex.StackTrace, true);
                    if (ex.InnerException != null)
                        _logger.Log(ex.InnerException.ToString(), true);
                }
                _serverSocket = null;
                if(_requestListenerT != null)
                    _requestListenerT.Abort();
            }
        }

        private void HandleRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[10240]; // 10 kb, just in case
            int receivedBCount = clientSocket.Receive(buffer); // Receive the request
            string strReceived = _charEncoder.GetString(buffer, 0, receivedBCount);

            // Parse method of the request
            string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));

            int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            int length = strReceived.LastIndexOf("HTTP") - start - 1;
            string requestedUrl = strReceived.Substring(start, length);

            string requestedFile;
            
            if (httpMethod.Equals("GET") || httpMethod.Equals("POST"))
                requestedFile = requestedUrl.Split('?')[0];
            else // You can implement other methods...
            {
                notImplemented(clientSocket);
                return;
            }



            requestedFile = requestedFile.Replace("/", @"\").Replace("\\..", "");
            start = requestedFile.LastIndexOf('.') + 1;
            length = requestedFile.Length - start;
            string extension = requestedFile.Substring(start, length);

            switch(requestedFile.ToLower())
            {
                // http://192.168.1.16:8080/resortlistview.html?grid=lbPrices&col=7

                case "\\resortlistview.html":
                    _callingForm.GenericSingleParameterMessage(requestedUrl, AppDelegateType.ChangeGridSort);
                    SendTradeDataPage(clientSocket);
                    break;
                case "\\": sendResponse(clientSocket, "<HTML>" + BodyTag + "<font size=\"12\">RegulatedNoise</font><br><br><form action=\"/ocrpoll\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"OCR Corrections\"></form><br><form action=\"/tradedata\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Trade Data\"></form><br><form action=\"/enternotedata\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Add Note\"></form></BODY></HTML>", "200 OK", "text/html"); break;
                case "\\latestocrimage.bmp":        ReturnCurrentOcrImage(clientSocket, extension); break;
                case "\\createnote":
                case "\\createnote.html":
                    ParseNoteParameters(requestedUrl);
                    ReturnEnterNoteDataHtml(clientSocket);
                    break;
                case "\\tradedata": 
                case "\\tradedata.html": 
                    SendTradeDataPage(clientSocket);
                    break;
                case "\\ocr":
                case "\\ocr.html":                  ReturnOcrHtml(clientSocket); break;
                case "\\returnocrvalues.html": _callingForm.SetOcrValueFromWeb(requestedUrl.Replace("/returnocrvalues.html?fname=", "").Replace("+", " ")); ReturnOcrHtml(clientSocket); break;
                case "\\import.html":
                    _callingForm.ImportCurrentOcrData();
                    sendResponse(clientSocket, "<HTML>"+BodyTag+"<meta http-equiv=\"Refresh\" content=\"0; url=ocrpoll.html\"></BODY></HTML>", "200 OK", "text/html");
                    break;
                case "\\ocrpoll":
                case "\\ocrpoll.html":

                    var ocrValueFromWeb = _callingForm.GetOcrValueForWeb()[0];

                    if (!_callingForm.ReturnOcrMonitoringStatus())
                    {
                        sendResponse(clientSocket, "<HTML>"+BodyTag+"<meta http-equiv=\"Refresh\" content=\"5; url=ocrpoll.html\"><font size=\"12\">Start monitoring first!  Go to OCR => Monitor Directory.<BR><BR>  Refreshing every 5 seconds for a new screenshot...</font><br>"+ReturnToHome+"</BODY></HTML>", "200 OK", "text/html");
                    }
                    else if (ocrValueFromWeb == "<FINISHED>")
                    {
                        sendResponse(clientSocket, "<HTML>"+BodyTag+"<meta http-equiv=\"Refresh\" content=\"5; url=ocrpoll.html\"><font size=\"12\">Refreshing every 5 seconds for a new screenshot...</font><br><form action=\"/\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Back to front page\"></form></BODY></HTML>", "200 OK", "text/html");
                    }
                    else if (ocrValueFromWeb.Contains("Cached"))
                    {
                        var cached = ocrValueFromWeb.Replace("Cached", "");
                        sendResponse(clientSocket, "<HTML>" + BodyTag + "<meta http-equiv=\"Refresh\" content=\"5; url=ocrpoll.html\"><font size=\"12\">"+cached+" screenshots cached.  Refreshing every 5 seconds.</font><br><form action=\"/\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Back to front page\"></form></BODY></HTML>", "200 OK", "text/html");
                    }
                    else
                    {
                        sendResponse(clientSocket, "<HTML>"+BodyTag+"<meta http-equiv=\"Refresh\" content=\"0; url=ocr.html\"></BODY></HTML>", "200 OK", "text/html");
                    }
                    break;
                case "\\updatestationandsystem":
                case "\\updatestationandsystem.html":

                    var returnParams1 = requestedUrl.Replace("/updatestationandsystem.html?station=", "").Replace("+", " ").Replace("&system=","!").Split(new char[1] { '!' });
                    _callingForm.SetStationAndSystem(returnParams1[0], returnParams1[1]);
                    ReturnOcrHtml(clientSocket); 
                    break;
                case "\\enternotedata":
                case "\\enternotedata.html":
                    ReturnEnterNoteDataHtml(clientSocket);
                    break;
                default:                            notFound(clientSocket); break;

            }
        }

        private void SendTradeDataPage(Socket clientSocket)
        {
            StringBuilder s = new StringBuilder();
            s.Append("<HTML>" + BodyTag +
                     "<FONT SIZE=\"8\"><FONT FACE=\"arial\">Trade Data<br><form action=\"/\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Back to front page\"></form>");
            var items = _callingForm.GetLvAllCommsItems();
            s.Append(items);
            s.Append("</BODY></HTML>");
            sendResponse(clientSocket, s.ToString(), "200 OK", "text/html");
        }

        private void ParseNoteParameters(string requestedUrl)
        {
            var parameterString = requestedUrl.Replace("/createnote.html?","").Replace("+"," ");
            var x = parameterString.Split('&');

            var newEvent = new CommandersLogEvent();

            foreach (var parameter in x)
            {
                var paramName = parameter.Split('=')[0];
                var paramValue = parameter.Split('=')[1];

                PropertyInfo prop = newEvent.GetType().GetProperty(paramName, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    if (prop.PropertyType.UnderlyingSystemType.Name == "Int32")
                    { 
                        int intValue;
                        if (int.TryParse(paramValue, out intValue))
                         prop.SetValue(newEvent, intValue, null);
                    }
                    else if (prop.PropertyType.UnderlyingSystemType.Name == "Decimal")
                    {
                        decimal intValue;
                        if (decimal.TryParse(paramValue, out intValue))
                            prop.SetValue(newEvent, intValue, null);
                    }
                    else
                        prop.SetValue(newEvent, paramValue, null);
                }

            }
            _callingForm.GenericSingleParameterMessage(newEvent, AppDelegateType.AddEventToLog);
        }

        private string ReturnToHome
        {
            get
            {
                return
                    "<form action=\"/\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Back to front page\"></form>";
            }
        }

        private void ReturnOcrHtml(Socket clientSocket)
        {
            var currentTextBoxValue = _callingForm.GetOcrValueForWeb();

            if (currentTextBoxValue[0] == "<FINISHED>")
            {
                sendResponse(clientSocket, "<HTML>" + BodyTag + "<meta http-equiv=\"Refresh\" content=\"5; url=ocrpoll.html\"><font size=\"12\">Refreshing every 5 seconds for a new screenshot...</font><br><form action=\"/\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Back to front page\"></form></BODY></HTML>", "200 OK", "text/html");
            }
            else if (currentTextBoxValue[0].Contains("Cached"))
            {
                var cached = currentTextBoxValue[0].Replace("Cached", "");
                sendResponse(clientSocket, "<HTML>" + BodyTag + "<meta http-equiv=\"Refresh\" content=\"5; url=ocrpoll.html\"><font size=\"12\">Working; " + cached + " screenshots cached.  Refreshing every 5 seconds.</font><br><form action=\"/\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Back to front page\"></form></BODY></HTML>", "200 OK", "text/html");
            }
            else if (currentTextBoxValue[0] == "Working...")
            {
                sendResponse(clientSocket, "<HTML>" + BodyTag + "<meta http-equiv=\"Refresh\" content=\"5; url=ocrpoll.html\"><font size=\"12\">Working.  Refreshing every 5 seconds.</font><br><form action=\"/\"><input type=\"submit\" style=\"font-size: 44pt\" value=\"Back to front page\"></form></BODY></HTML>", "200 OK", "text/html");
            }
            else
            {
                var valuesFromForm = _callingForm.GetOcrValueForWeb();

                var inputStyle = "text";
                if (valuesFromForm[3] == "1" || valuesFromForm[3] == "2" || valuesFromForm[3] == "3")
                    inputStyle = "number";

                sendResponse(clientSocket, @"<HTML>
"+BodyTag+@"<font size=""12"">OCR Correction</font><BR>
<form action=""updatestationandsystem.html"">
    <input type=""text"" style=""font-size: 44pt"" value=""" + _callingForm.GetOcrValueForWeb()[1] + @""" name=""station"">
    <br>
<input type=""text"" style=""font-size: 44pt"" value=""" + _callingForm.GetOcrValueForWeb()[2] + @""" name=""system"">
    <input type=""submit"" style=""font-size: 24pt""  value=""Amend"">
</form>
<IMG SRC=""latestocrimage.bmp"" id=""ocrimg""></IMG><button type=""button"" style=""font-size: 24pt"" onclick=""document.getElementById('ocrimg').src = 'latestocrimage.bmp?random='+new Date().getTime();"">Reload Image</button><br>
<form action=""returnocrvalues.html"">
    <input id=""inputValue"" type=""" + inputStyle + @""" style=""font-size: 44pt"" value=""" + _callingForm.GetOcrValueForWeb()[0] + @""" name=""fname"">"
                    +

                    @"<button type=""button"" style=""font-size: 24pt"" onclick=""document.getElementById('inputValue').value=''"">Clear</button><br>"
                    +
    @"<input type=""submit"" style=""font-size: 44pt"" value=""Correct and Continue"">
</form></BODY></HTML>", "200 OK", "text/html");
            }
        }
        
        private void ReturnEnterNoteDataHtml(Socket clientSocket)
        {
            sendResponse(clientSocket, 
                HtmlTag("Create Log Event")+
/*@"<form style=""font-size: 24pt"" action=""createnote.html"">"
+NoteInputFieldHtml("EventType")
+NoteInputFieldHtml("Station")
+NoteInputFieldHtml("System")
+NoteInputFieldHtml("Cargo")
+NoteInputFieldHtml("CargoAction")
+NoteInputFieldHtml("CargoVolume")
+NoteInputFieldHtml("Notes")+
@"<input type=""submit"" style=""font-size: 44pt"" value=""Create"">"+
"</form>"*/"Web interface for event logging is currently b0rked.  I will *totally* fix this one day :(<br>" + ReturnToHome + HtmlCloseTag, "200 OK", "text/html");
        }

        private string NoteInputFieldHtml(string label)
        {
            return (@"<label for=""cargoVolume"">" + label + "  " +
                    @"</label><input type=""text"" style=""font-size: 24pt"" name=""" + label +
                    @"""> <br>");
        }

        private string HtmlTag(string pageName)
        {
            return @"<HTML>" + BodyTag + @"<font size=""12"">"+pageName+"</font><BR>";
        }

        private string HtmlCloseTag
        {
            get
            {
                return @"</BODY></HTML>";
            }
        }

        private void ReturnCurrentOcrImage(Socket clientSocket, string extension)
        {
            var notResizedOutput = _callingForm.ReturnTrimmedImage();
            Bitmap output = new Bitmap(notResizedOutput,notResizedOutput.Width*2,notResizedOutput.Height*2);
            var outBytes = ImageToByte2(output);
            sendOkResponse(clientSocket, outBytes, _extensions[extension]);
        }

        //http://stackoverflow.com/questions/7350679/convert-a-bitmap-into-a-byte-array
        public static byte[] ImageToByte2(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }


        private void notImplemented(Socket clientSocket)
{

    sendResponse(clientSocket, "<html>"+BodyTag+"<font size=\"12\">501 - Method Not Implemented</font></body></html>", "501 Not Implemented", "text/html");

}

    private void notFound(Socket clientSocket)
    {

        sendResponse(clientSocket, "<html>"+BodyTag+"<font size=\"12\">404 - Not Found</font></body></html>", "404 Not Found", "text/html");
    }

    private void sendOkResponse(Socket clientSocket, byte[] bContent, string contentType)
    {
        sendResponse(clientSocket, bContent, "200 OK", contentType);
    }

            // For strings
    private void sendResponse(Socket clientSocket, string strContent, string responseCode,
                              string contentType)
    {
        byte[] bContent = _charEncoder.GetBytes(strContent);
        sendResponse(clientSocket, bContent, responseCode, contentType);
    }

    // For byte arrays
    private void sendResponse(Socket clientSocket, byte[] bContent, string responseCode,
                              string contentType)
    {
        try
        {
            byte[] bHeader = _charEncoder.GetBytes(
                                "HTTP/1.1 " + responseCode + "\r\n"
                              + "Server: RegulatedNoise\r\n"
                              + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                              + "Connection: close\r\n"
                              + "Content-Type: " + contentType + "\r\n\r\n");
            clientSocket.Send(bHeader);
            clientSocket.Send(bContent);
            clientSocket.Close();
        }
        catch (Exception ex)
        {
            _logger.Log("Error in webserver start 1:", true);
            _logger.Log(ex.ToString(), true);
            _logger.Log(ex.Message, true);
            _logger.Log(ex.StackTrace, true);
            if (ex.InnerException != null)
                _logger.Log(ex.InnerException.ToString(), true);
        }
    }

    public string BackgroundColour { get; set; }
    public string ForegroundColour { get; set; }

    public string BodyTag { get { return @"<body " + StyleTag + " alink=" + ForegroundColour + " vlink=" + ForegroundColour + " link=" + ForegroundColour + ">"; } }

    public string StyleTag { get { return @"style=""color:" + ForegroundColour + @"; background-color:" + BackgroundColour + @""""; } }
    }
}
