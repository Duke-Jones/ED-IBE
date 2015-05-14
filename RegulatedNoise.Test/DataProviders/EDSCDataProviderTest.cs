using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RegulatedNoise.Test.DataProviders
{
    [TestClass]
    public class EdscDataProviderTest
    {
        [TestMethod]
        public void i_can_fetch_system_data()
        {
            //{"id":26200,"name":"Shui Wei Sector FC-U B3-3","x":14.96875,"y":-80.6875,"z":31.9375,"faction":"","population":null,"government":null,"allegiance":null,"state":null,"security":null,"primary_economy":null,"needs_permit":null,"updated_at":1430467261}
            var escd = new EdscDataProvider();
            System[] systems = escd.RetrieveSystems("1 G. Caeli").Result;
            if (systems.Length == 0)
            {
                Debug.WriteLine("No System.");
            }
            else
            {
                foreach (System system in systems)
                {
                    Debug.WriteLine(system);
                }                
            }
        }
    }

    public class EdscDataProvider
    {
        protected class EdscGetSystemResponse
        {
            public class EdscGetSystemResponseContent
            {
                [JsonProperty(PropertyName = "ver")]
                public double EdscVersion { get; set; }
                [JsonProperty(PropertyName = "date")]
                public string Date { get; set; }
                [JsonProperty(PropertyName = "status")]
                public Status Status { get; set; }
                [JsonProperty(PropertyName = "systems")]
                public System[] Systems { get; set; }
            }

            [JsonProperty(PropertyName = "d")]
            public EdscGetSystemResponseContent Content { get; set; }
        }

        protected class Status
        {
            public class Input
            {
                public class InputStatus
                {
                    [JsonProperty(PropertyName = "statusnum")]
                    public int StatusId { get; set; }
                    [JsonProperty(PropertyName = "msg")]
                    public string Message { get; set; }
                }

                [JsonProperty(PropertyName = "status")]
                public InputStatus Status { get; set; }
            }
            [JsonProperty(PropertyName = "input")]
            public Input[] Inputs { get; set; }
        }

        private const string EDSC_DEFAULT_URI = "http://edstarcoordinator.com/api.asmx/GetSystems";

        public Uri EdscUri { get; private set; }

        public EdscDataProvider(Uri edscUri)
        {
            EdscUri = edscUri;
        }

        public EdscDataProvider()
            : this(new Uri(EDSC_DEFAULT_URI))
        {
        }

        public async Task<System[]> RetrieveSystems(string system)
        {
            return await RetrieveSystems(BuildGetSystemRequestContent(BuildEdscRequest(system)));
        }

        public async Task<System[]> RetrieveSystems(DateTime newerThan)
        {
            return await RetrieveSystems(BuildGetSystemRequestContent(BuildEdscRequest(newerThan)));
        }

        private async Task<System[]> RetrieveSystems(StringContent requestContent)
        {
            HttpResponseMessage httpResponse;

            using (var client = new HttpClient())
            {
                httpResponse =
                    await
                        client.PostAsync(EdscUri,
                            requestContent);
            }
            string data = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                EdscGetSystemResponse response = JsonConvert.DeserializeObject<EdscGetSystemResponse>(data);
                return response.Content.Systems;
            }
            catch (Exception ex)
            {
                Trace.TraceError("edsc parsing failure: " + ex);
                return new System[0];
            }
        }

        private static StringContent BuildGetSystemRequestContent(string edscRequest)
        {
            return new StringContent(edscRequest, Encoding.UTF8, "application/json");
        }

        private static string BuildEdscRequest(string system)
        {
            //data: {
            //    ver:2,
            //    test: true, 
            //    outputmode:1, 
            //    filter:{
            //        knownstatus:0,
            //        systemname: "sol",
            //        cr:5,
            //        date:"2014-09-18 12:34:56",
            //        coordcube: [[-10,10],[-10,10],[-10,10]],
            //        coordsphere: {radius: 123.45, origin: [10,20,30]}
            //    }
            var post = new JObject(
                                new JProperty("data",
                                        new JObject(
                                            new JProperty("ver", 2)
                                            , new JProperty("outputmode", 2)
                                            , new JProperty("filter", new JObject(
                                                new JProperty("systemname", system)
                                                    ))
                                            )));
            return post.ToString(Formatting.None);
        }

        private static string BuildEdscRequest(DateTime newerThan)
        {
            //data: {
            //    ver:2,
            //    test: true, 
            //    outputmode:1, 
            //    filter:{
            //        knownstatus:0,
            //        systemname: "sol",
            //        cr:5,
            //        date:"2014-09-18 12:34:56",
            //        coordcube: [[-10,10],[-10,10],[-10,10]],
            //        coordsphere: {radius: 123.45, origin: [10,20,30]}
            //    }
            JObject post;
            if (newerThan == DateTime.MinValue)
            {
                post = new JObject(
                    new JProperty("data",
                        new JObject(
                              new JProperty("ver", 2)
                            , new JProperty("outputmode", 2)
                            )
                            ));
            }
            else
            {
                post = new JObject(
                    new JProperty("data",
                        new JObject(
                            new JProperty("ver", 2)
                            , new JProperty("outputmode", 2)
                            , new JProperty("filter", new JObject(
                                new JProperty("date", newerThan)
                                ))
                            )));                
            }
            return post.ToString(Formatting.None);
        }
    }

    public class System
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "coord")]
        public double[] Coordinates { get; set; }
        [JsonProperty(PropertyName = "cr")]
        public int ConfidenceRating { get; set; }
        [JsonProperty(PropertyName = "commandercreate")]
        public string CommanderCreate { get; set; }
        [JsonProperty(PropertyName = "createdate")]
        public string CreateDate { get; set; }
        [JsonProperty(PropertyName = "commanderupdate")]
        public string CommanderUpdate { get; set; }
        [JsonProperty(PropertyName = "updatedate")]
        public string UpdateDate { get; set; }

        public override string ToString()
        {
            return Name + " [" + Id + "]" + DisplayCoordinates();
        }

        private string DisplayCoordinates()
        {
            if (Coordinates == null || Coordinates.Length < 3)
            {
                return String.Empty;
            }
            else
            {
                return String.Format("({0},{1},{2})", Coordinates[0], Coordinates[1], Coordinates[2]);
            }
        }
    }
}