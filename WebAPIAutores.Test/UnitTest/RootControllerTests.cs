using Microsoft.AspNetCore.Authorization;
using WebAPIAutores.Controllers.V1;
using WebAPIAutores.Test.Mocks;

namespace WebAPIAutores.Test.UnitTest
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task IfUserIsAdmin_Get4Links()
        {
            //Preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Result = AuthorizationResult.Success();
            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();
            //Ejecucion
            var result = await rootController.Get();
            //Verificacion
            Assert.AreEqual(4, result?.Value?.Count());
        }
        [TestMethod]
        public async Task IfUserIsNotAdmin_Get2Links()
        {
            //Preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Result = AuthorizationResult.Failed();
            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();
            //Ejecucion
            var result = await rootController.Get();
            //Verificacion
            Assert.AreEqual(2, result?.Value?.Count());
        }
    }
}
