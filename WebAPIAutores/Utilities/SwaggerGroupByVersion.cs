using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebAPIAutores.Utilities
{
    public class SwaggerGroupByVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceContoller = controller.ControllerType.Namespace; //Controller.V1
            var versionAPI = namespaceContoller.Split('.').Last().ToLower();
            controller.ApiExplorer.GroupName = versionAPI;
        }
    }
}
