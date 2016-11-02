using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Threading;
using ZeroMQ;
using IBE.Enums_and_Utility_Classes;
using System.Globalization;
using System.Net;
using CodeProject.Dialog;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using IBE.Enums_and_Utility_Classes;
using System.Threading;
using IBE.SQL;

namespace IBE.EDDN
{
    public class EDDNReciever : IDisposable
    {

        private Boolean                             m_Active = false;
        private String                              m_Adress;
        private Thread                              m_EDDNSubscriberThread;
        private static readonly object              m_RecieveLocker = new object();

#region dispose region

        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();

                // Free any other managed objects here.
                StopListen();
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

#endregion

#region Event


        // The delegate procedure we are assigning to our object
        public delegate void RecievedEDDNHandler(object sender, EDDNRecievedArgs e);

        public event RecievedEDDNHandler           DataRecieved;

#endregion

        public EDDNReciever(string adress)
        {
            m_Adress = adress;
        }

        /// <summary>
        /// starts listening
        /// </summary>
        public void StartListen()
        {
                m_EDDNSubscriberThread = new Thread(() => Subscribe());
                m_EDDNSubscriberThread.IsBackground = true;
                m_EDDNSubscriberThread.Start();
        }

        /// <summary>
        /// stops EDDN-listening if started
        /// </summary>
        public void StopListen()
        {
            try
            {
                if (m_EDDNSubscriberThread != null)
                {
                    m_Active = false;

                    if(!m_EDDNSubscriberThread.Join(10000))
                        throw new Exception("Couldn't stop the EDDN-Listener !");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while stopping the EDDN-Listener", ex);
            }
        }

        /// <summary>
        /// returns "true" if the listener is working
        /// </summary>
        public Boolean IsListening
        {
            get
            {
                return (m_EDDNSubscriberThread != null) && ((m_EDDNSubscriberThread.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0);
            }
        }

        /// <summary>
        /// subscriberthread-worker
        /// </summary>
        public void Subscribe()
        {

            m_Active = true;

            using (var ctx = ZmqContext.Create())
            {
                using (var socket = ctx.CreateSocket(SocketType.SUB))
                {
                    socket.SubscribeAll();

                    socket.Connect(m_Adress);

                    while (m_Active)
                    {
                        var byteArray = new byte[10240];

                        int i = socket.Receive(byteArray, TimeSpan.FromTicks(50));

                        var decompressedFileStream = new MemoryStream();
                        if (i != -1)
                            using (decompressedFileStream)
                            {
                                Stream stream = new MemoryStream(byteArray);

                                // Don't forget to ignore the first two bytes of the stream (!)
                                stream.ReadByte();
                                stream.ReadByte();
                                using (var decompressionStream = new DeflateStream(stream, CompressionMode.Decompress))
                                {
                                    decompressionStream.CopyTo(decompressedFileStream);
                                }

                                decompressedFileStream.Position = 0;
                                var sr = new StreamReader(decompressedFileStream);
                                var myStr = sr.ReadToEnd();

                                //_caller.OutputEddnRawData(myStr);
                                ParseEDDNRawData(myStr);

                                decompressedFileStream.Close();
                            }
                        Thread.Sleep(100);
                    }
                }
            }
        }

        /// <summary>
        /// parses the incoming eddn data
        /// </summary>
        /// <param name="RawData"></param>
        private void ParseEDDNRawData(String RawData)
        {
            try
            {
                EDDNRecievedArgs ArgsObject;

                if (RawData.Contains(@"commodity/1"))
                {
                    // new v2 schema
                    Debug.Print("recieved v1 commodities message");

                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved data commodities message (v1)",
                        InfoType = EDDNRecievedArgs.enMessageInfo.Commodity_v1_Recieved,
                        RawData = RawData,
                        Data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(RawData),
                        Adress = m_Adress
                    };

                }
                if (RawData.Contains(@"commodity/2"))
                {
                    // new v2 schema
                    Debug.Print("recieved v2 commodities message");

                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved data commodities message (v2)",
                        InfoType = EDDNRecievedArgs.enMessageInfo.Commodity_v2_Recieved,
                        RawData = RawData,
                        Data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(RawData),
                        Adress = m_Adress
                    };

                }
                else if (RawData.Contains(@"commodity/3"))
                {
                    // new v2 schema
                    Debug.Print("recieved v3 commodities message");

                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved data commodities message (v3)",
                        InfoType = EDDNRecievedArgs.enMessageInfo.Commodity_v3_Recieved,
                        RawData = RawData,
                        Data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(RawData),
                        Adress = m_Adress
                    };
                }
                else if (RawData.Contains(@"outfitting/1"))
                {
                    // outfitting schema
                    Debug.Print("recieved outfitting v1 message");
                    
                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved data outfitting message (v1)",
                        InfoType = EDDNRecievedArgs.enMessageInfo.Outfitting_v1_Recieved,
                        RawData = RawData,
                        Data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(RawData),
                        Adress = m_Adress
                    };

                }
                else if (RawData.Contains(@"outfitting/2"))
                {
                    // outfitting schema
                    Debug.Print("recieved outfitting v2 message");
                    
                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved data outfitting message (v2)",
                        InfoType = EDDNRecievedArgs.enMessageInfo.Outfitting_v2_Recieved,
                        RawData = RawData,
                        Data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(RawData),
                        Adress = m_Adress
                    };

                }
                else if (RawData.Contains(@"shipyard/1"))
                {
                    // shipyardItem schema
                    Debug.Print("recieved shipyard v1 message");

                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved data shipyard message (v1)",
                        InfoType = EDDNRecievedArgs.enMessageInfo.Shipyard_v1_Recieved,
                        RawData = RawData,
                        Data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(RawData),
                        Adress = m_Adress
                    };

                }
                else if (RawData.Contains(@"shipyard/2"))
                {
                    // shipyardItem schema
                    Debug.Print("recieved shipyard v2 message");

                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved data shipyard message (v2)",
                        InfoType = EDDNRecievedArgs.enMessageInfo.Shipyard_v2_Recieved,
                        RawData = RawData,
                        Data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(RawData),
                        Adress = m_Adress
                    };

                }
                else
                {
                    // other unknown data

                    ArgsObject = new EDDNRecievedArgs()
                    {
                        Message = "recieved unknown data message",
                        InfoType = EDDNRecievedArgs.enMessageInfo.UnknownData,
                        RawData = RawData,
                        Data = null,
                        Adress = m_Adress
                    };
                }

                if(ArgsObject != null)
                {
                    // only for one listener per time this is allowed
                    lock (m_RecieveLocker)
                    { 
                        DataRecieved(this, ArgsObject);
                    }
                }
            }
            catch (Exception ex)
            {
                DataRecieved(this, new EDDNRecievedArgs()
                {
                    Message = "Error while parsing recieved EDDN data :" + Environment.NewLine + ex.GetBaseException().Message.ToString() + Environment.NewLine + ex.StackTrace,
                    InfoType = EDDNRecievedArgs.enMessageInfo.ParseError,
                    RawData = RawData,
                    Data = null
                });
            }
        }
    }
}
