using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Xrm.Sdk;
using System.Net.Http;
using Microsoft.PowerPlatform.Dataverse.Client;
using FuncionStarWars.Modelos;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Runtime.Serialization.Json;

namespace FuncionStarWars
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string clientId = "e529dcfc-40c6-4a26-9107-8216aa183bf0";
            string clientSecret = "tZX8Q~A_xnjtm7oKAYBvYlZN4ne7oa4ZNh4Hrcy4";
            string _url = "https://org720f117b.crm4.dynamics.com/";
            string connectionString = $"Url={_url};ClientId={clientId};ClientSecret={clientSecret};authtype=ClientSecret";

            var serviceClient = new ServiceClient(connectionString);

            //  serviceClient.Retrieve(new Entity("account"));

           // Entity stateEntity = new Entity("account");  
            var url = Environment.GetEnvironmentVariable("swapi_url");

            string reuqestbody = await new StreamReader(req.Body).ReadToEndAsync();

           var context = GetRemoteExecutionContextFromJson(reuqestbody);


            Entity account = (Entity)context.InputParameters["Target"];


            account.GetAttributeValue<int>("numero");


              Input requestJsnon = JsonConvert.DeserializeObject<Input>(reuqestbody);

            string tabla = account.GetAttributeValue<string>("cr8a6_tabla");
            string registro = account.GetAttributeValue<string>("cr8a6_numero");

            if(registro != null || !registro.Equals("") || tabla != null || !tabla.Equals("")) { 

            var test = Environment.GetEnvironmentVariable("swapi_url");
            var ejmplo = new HttpClient();
            var resquest = new HttpRequestMessage(HttpMethod.Get, String.Format("{0}/{1}/{2}", url, tabla, registro));
            var response = await ejmplo.SendAsync(resquest);
            response.EnsureSuccessStatusCode();

            var string_result = await response.Content.ReadAsStringAsync();
            StarWars jsonreusltado = null;
            switch (requestJsnon.table)
            {
                case "people":
                    People people = JsonConvert.DeserializeObject<People>(string_result);            
                    string ppp = people.name;
                    account.Attributes["name"] = ppp;
                    serviceClient.Update(account);
                    jsonreusltado = JsonConvert.DeserializeObject<People>(string_result);
                    break;
                case "planets":                
                    jsonreusltado = JsonConvert.DeserializeObject<Planeta>(string_result);
                    break;
                case "starships":
                    jsonreusltado = JsonConvert.DeserializeObject<StarShips>(string_result);
                    break;
            }
            }
            string responseMessage = "";
            return new OkObjectResult(responseMessage);
        }

        public static RemoteExecutionContext GetRemoteExecutionContextFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(RemoteExecutionContext));
                var context = (RemoteExecutionContext)serializer.ReadObject(ms);
                return context;
            }
        }
    }
}
