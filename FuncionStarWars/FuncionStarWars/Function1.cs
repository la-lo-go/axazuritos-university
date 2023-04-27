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
using Microsoft.Win32;

namespace FuncionStarWars
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Se mete al princincipio");

            string clientId = "e529dcfc-40c6-4a26-9107-8216aa183bf0";
            string clientSecret = "tZX8Q~A_xnjtm7oKAYBvYlZN4ne7oa4ZNh4Hrcy4";
            string _url = "https://org720f117b.crm4.dynamics.com/";
            string connectionString = $"Url={_url};ClientId={clientId};ClientSecret={clientSecret};authtype=ClientSecret";

            var serviceClient = new ServiceClient(connectionString);

            //  serviceClient.Retrieve(new Entity("account"));

           // Entity stateEntity = new Entity("account");  
            var url = Environment.GetEnvironmentVariable("swapi_url");

            string reuqestbody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(reuqestbody);

            var context = GetRemoteExecutionContextFromJson(reuqestbody);
            //context.getMessageName("" , "");

            // context.MessageName
            //Create o Udpate 

            log.LogInformation("Recupera el context " + context.MessageName.ToString());

            Entity account = (Entity)context.InputParameters["Target"];

            if (context.MessageName.ToString().Equals("Create"))
            {
                log.LogInformation("Recupera el context " + account);


                int numero = account.GetAttributeValue<int>("cr8a6_numero");

                string tabla = account.GetAttributeValue<string>("cr8a6_tabla");
                string registro = "" + numero;

                if (registro != null || !registro.Equals("") || tabla != null || !tabla.Equals(""))
                {
                    log.LogInformation("Se mete en el if");


                    var test = Environment.GetEnvironmentVariable("swapi_url");
                    var ejmplo = new HttpClient();
                    var resquest = new HttpRequestMessage(HttpMethod.Get, String.Format("{0}/{1}/{2}", url, tabla, registro));
                    var response = await ejmplo.SendAsync(resquest);
                    response.EnsureSuccessStatusCode();

                    var string_result = await response.Content.ReadAsStringAsync();
                    StarWars jsonreusltado = null;
                    switch (tabla)
                    {
                        case "people":
                            People people = JsonConvert.DeserializeObject<People>(string_result);
                            string ppp = people.name;
                            Entity starWarspersonaje = new Entity("account", account.Id);


                            starWarspersonaje.Attributes["name"] = ppp;
                            serviceClient.Update(starWarspersonaje);
                            jsonreusltado = JsonConvert.DeserializeObject<People>(string_result);
                            break;
                        case "planets":
                            Planeta planeta = JsonConvert.DeserializeObject<Planeta>(string_result);
                            string pp = planeta.name;
                            Entity starWarsplaneta = new Entity("account", account.Id);


                            starWarsplaneta.Attributes["name"] = pp;
                            serviceClient.Update(starWarsplaneta);
                            jsonreusltado = JsonConvert.DeserializeObject<People>(string_result);
                            break;
                        case "starships":
                            Planeta starship = JsonConvert.DeserializeObject<Planeta>(string_result);
                            string p = starship.name;
                            Entity starWarsstarship = new Entity("account", account.Id);


                            starWarsstarship.Attributes["name"] = p;
                            serviceClient.Update(starWarsstarship);
                            jsonreusltado = JsonConvert.DeserializeObject<People>(string_result);
                            break;
                    }
                }
            }
            else {


                log.LogInformation("Se mete en el else ");
                Entity PreImage = (Entity)context.PreEntityImages["IdStarWars"];
                log.LogInformation("Pilla el preimagen ");


                int numero = PreImage.GetAttributeValue<int>("cr8a6_numero");
                string viejoTabla = PreImage.GetAttributeValue<string>("cr8a6_tabla");
                string viejoNumero = "" + numero;

                // var oldAccountName = (string)context.PreEntityImages["IdStarWars"]["cr8a6_numero"];


                //string viejoNumero = (string)context.Pre
                //EntityImages["idd"]["cr8a6_numero"];

                Entity PostImgen = (Entity)context.PostEntityImages["IdStarWars"];
                int numero2 = PostImgen.GetAttributeValue<int>("cr8a6_numero");
                string nuevoTabla = PostImgen.GetAttributeValue<string>("cr8a6_tabla");
                string nuevoNumero = "" + numero2;
                //|| !viejoTabla.Equals(nuevoTabla)


                if (!viejoNumero.Equals(nuevoNumero) || !viejoTabla.Equals(nuevoTabla))
                {

                    var test = Environment.GetEnvironmentVariable("swapi_url");
                    var ejmplo = new HttpClient();
                    var resquest = new HttpRequestMessage(HttpMethod.Get, String.Format("{0}/{1}/{2}", test, nuevoTabla, numero2));        
                         var response = await ejmplo.SendAsync(resquest);
                        response.EnsureSuccessStatusCode();

               

                    var string_result = await response.Content.ReadAsStringAsync();
                    StarWars jsonreusltado = null;
                    switch (nuevoTabla)
                    {
                        case "people":
                            People people = JsonConvert.DeserializeObject<People>(string_result);
                            string ppp = people.name;
                            Entity starWarspersonaje = new Entity("account", account.Id);


                            starWarspersonaje.Attributes["name"] = ppp;
                            serviceClient.Update(starWarspersonaje);
                            jsonreusltado = JsonConvert.DeserializeObject<People>(string_result);
                            break;
                        case "planets":
                            Planeta planeta = JsonConvert.DeserializeObject<Planeta>(string_result);
                            string planea = planeta.name;
                            Entity starWarsplaneta = new Entity("account", account.Id);


                            starWarsplaneta.Attributes["name"] = planea;
                            serviceClient.Update(starWarsplaneta);
                            jsonreusltado = JsonConvert.DeserializeObject<People>(string_result);
                            break;
                        case "starships":
                            Planeta starship = JsonConvert.DeserializeObject<Planeta>(string_result);
                            string p = starship.name;
                            Entity starWarsstarship = new Entity("account", account.Id);


                            starWarsstarship.Attributes["name"] = p;
                            serviceClient.Update(starWarsstarship);
                            jsonreusltado = JsonConvert.DeserializeObject<People>(string_result);
                            break;
                    }
            }




            }


            /*

            var viejoTabla = (string)context.PreEntityImages["a"]["name"];
            var viejoNumero = (string)context.PreEntityImages["a"]["name"];

            var nuevoTabla = (string)context.PostEntityImages["a"]["name"];
            var nuevoNumero = (string)context.PostEntityImages["a"]["name"];

            /*

            if (context.PostEntityImages(""))
            {

            }

            */


            //context.postimagen


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
