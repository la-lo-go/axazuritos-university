/**
Para subir este plugin al CRM y hacer que se ejecute como PostOperation, debes seguir los siguientes pasos:

    - Compila el proyecto que contiene el plugin en Visual Studio.
    - Abre el CRM y navega a la sección de Personalización.
    - Selecciona la entidad en la que deseas que se ejecute el plugin.
    - Haz clic en "Agregar Nuevo Plugin".
    - Completa los detalles del plugin, incluyendo el nombre, la descripción y la clase del plugin.
    - Selecciona "Post-Operation" como el evento de ejecución del plugin.
    - Haz clic en "Agregar Paso de Plugin".
    - Completa los detalles del paso del plugin, incluyendo el nombre, la descripción y la configuración de filtrado.
    - Selecciona la clase del plugin que compilaste en el paso 1.
    - Haz clic en "Guardar" para guardar el plugin y el paso del plugin.
**/

using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUniversity_AXZ.Plugings
{
    public class TaskPlugin : IPlugin
    {
        // Este método se llama cuando se activa el plugin
        public void Execute(IServiceProvider serviceProvider)
        {
            // Recupera los servicios necesarios y la información de contexto del parámetro IServiceProvider
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // Verifica si los parámetros de entrada contienen una entidad de destino
            if(serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            // Registra que el plugin ha comenzado
            tracingService.Trace("Plugin Started");

            // Si los parámetros de entrada contienen una entidad de destino, llama al método ExecuteTaskPlugin
            if(context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                ExecuteTaskPlugin(service, tracingService, context);
            }
        }

        // Este método crea un nuevo registro de tarea y lo asocia con la entidad de destino
        private void ExecuteTaskPlugin(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            try
            {
                // Recupera la entidad de destino de los parámetros de entrada
                var entity = (Entity)context.InputParameters["Target"];

                // Crea una nueva entidad de tarea y establece sus atributos
                var task = new Entity("task");
                task["subject"] = "Seguimiento";
                task["description"] = "Seguimiento de la tarea abierta.";
                task["scheduledstart"] = DateTime.Now;
                task["scheduledend"] = DateTime.Now.AddDays(2);
                task["category"] = context.PrimaryEntityName;

                // Asocia la entidad de tarea con la entidad de destino
                if(context.OutputParameters.Contains("id"))
                {
                    Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                    string regardingobjectidType = context.OutputParameters["id"].GetType().Name;
                    task["regardingobjectid"] = new EntityReference(regardingobjectidType, regardingobjectid);
                }

                // Crea la entidad de tarea en el sistema CRM utilizando IOrganizationService
                service.Create(task);

                // Registra que se ha creado la tarea
                tracingService.Trace("Task Created");
            }
            catch(Exception ex)
            {
                // Registra cualquier error que ocurra durante la ejecución del plugin
                tracingService.Trace("Plugin Error: {0}", ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        } 
    }
}
