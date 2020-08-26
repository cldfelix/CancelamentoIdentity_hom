using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;

namespace CancelamentoIdentity.Services
{
    public class PortalescalaToken
    {
        public Result Result { get; set; }
    }
    public class Result
    {
        public string AccessToken { get; set; }
    }

    public class ResultByData
    {
        public int legNumber { get; set; }
        public string carrier { get; set; }
        public string flightNumber { get; set; }
        public string aircraftSubtype { get; set; }
        public int aircraftRotation { get; set; }
        public string aircraftRegistration { get; set; }
        public string legState { get; set; }
        public string legType { get; set; }
        public DateTime dayOfOrigin { get; set; }
        public string scheduledAirportDeparture { get; set; }
        public string scheduledAirportArrival { get; set; }
        public string actualAirportDeparture { get; set; }
        public string actualAirportArrival { get; set; }
        public DateTime scheduledDeparture { get; set; }
        public DateTime scheduledArrival { get; set; }
        public DateTime offblock { get; set; }
        public DateTime airborne { get; set; }
        public DateTime landing { get; set; }
        public DateTime onblock { get; set; }
        public int sequenceNumber { get; set; }
        public string flightTime { get; set; }
        public string blockTime { get; set; }
        public string blockTimeScheduled { get; set; }
        public bool delayedOnDeparture { get; set; }
        public bool delayedOnArrival { get; set; }
        public List<object> delayCodes { get; set; }
    }

    public class RootObject
    {
        public List<ResultByData> result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __eaf { get; set; }
    }


    public class PortalEscalaService
    {
        private readonly HttpClient httpClient = new HttpClient();

  

        private static string GettokenPOrtalEescala()
        {

            var client = new RestClient("https://portalescalaapi-hom.voegol.com.br/api/TokenAuth/Authenticate");

            //client.Proxy =new WebProxy(@"http://clfelix:Quake3arena@proxygol.sede.gol.com:80");

            var request = new RestRequest(Method.POST);
        
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Content-Length", "288");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "portalescalaapi-hom.voegol.com.br");
            request.AddHeader("Postman-Token", "2435609b-d165-4ea0-b115-defaeb5a1e2c,48c15fbe-f520-433e-b5c9-94a8e3ee3d13");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.20.1");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\n    \"userNameOrEmailAddress\": \"jeppesen_jta\",\n    \"password\": \"jepp123*\",\n    \"twoFactorVerificationCode\": \"TwoFactorRememberClientToken\",\n    \"rememberClient\": \"True\",\n    \"twoFactorRememberClientToken\": \"TwoFactorRememberClientToken\",\n    \"singleSignIn\": \"True\",\n    \"returnUrl\": \"\"\n}", ParameterType.RequestBody);
            var ret = client.Execute(request).Content;

            /*
            var client = new RestClient("https://portalescala-api.voegol.com.br/api/TokenAuth/Authenticate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("userNameOrEmailAddress", "jeppesen_jta");
            request.AddParameter("password", "jepp123*");
            request.AddParameter("rememberClient", "True");
            request.AddParameter("twoFactorRememberClientToken", "twoFactorRememberClientToken");
            request.AddParameter("singleSignIn", "True");
            request.AddParameter("returnUrl", "");
            */
            //var ret = client.Execute(request).Content;
            var t = JsonConvert.DeserializeObject<PortalescalaToken>(ret);
            return "Bearer " + t.Result.AccessToken;
        }
        public static List<ResultByData> GetDadosDoVoo(DateTime dataVoo, string flightNumber)
        {
            // https://portalescala-api.voegol.com.br/api/services/app/Operations/GetFlightsStartingInPeriodUTC?IncludeCnlFlights=true&Begin=2020-04-01&End=2020-04-02

            var d = dataVoo.Year + "-" + dataVoo.Month + "-" + dataVoo.Day;
            const string url = @"https://portalescala-api.voegol.com.br/api/services/app/Operations/GetFlightsStartingInPeriodUTC";
            var token = GettokenPOrtalEescala();
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", token);
            request.AddParameter("IncludeOverflow", true);
            request.AddParameter("IncludeCnlFlights", true);
            request.AddParameter("begin", d);
            request.AddParameter("end", d);
            request.AddParameter("timeZone", "utc");
            var response = client.Execute<RootObject>(request);

            // var res = JsonConvert.DeserializeObject<RootObject>(response.Content);
            // return res.result.FindAll(v => (v.flightNumber == flightNumber) && (v.aircraftRegistration.ToUpper() != "XXXXX"));

                    var res = JsonConvert.DeserializeObject<RootObject>(response.Content);
            return res.result.FindAll(v => (v.flightNumber == flightNumber));
        }
    }
}
