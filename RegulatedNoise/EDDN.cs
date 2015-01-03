using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using ZeroMQ;

namespace RegulatedNoise
{
    public class EDDN
    {
        private Form1 _caller;

        public void Subscribe(Form1 caller)
        {
            _caller = caller;
            using (var ctx = ZmqContext.Create())
            {
                using (var socket = ctx.CreateSocket(SocketType.SUB))
                {
                    socket.SubscribeAll();

                    socket.Connect("tcp://eddn-relay.elite-markets.net:9500");

                    _caller.SetListening();

                    while (true)
                    {
                        var byteArray = new byte[10240];

                        int i = socket.Receive(byteArray, TimeSpan.FromTicks(50));

                        var decompressedFileStream = new MemoryStream();
                        if(i != -1)
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

                                _caller.OutputEddnRawData(myStr);
                                decompressedFileStream.Close();
                            }
                            Thread.Sleep(10);
                    }
                }
            }
// ReSharper disable once FunctionNeverReturns
        }
    }
}
