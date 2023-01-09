using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
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

        [TestMethod]
        public async Task IfUserIsNotAdmin_Get2LinksUsingMoq()
        {
            //Preparacion
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
               It.IsAny<ClaimsPrincipal>(),
               It.IsAny<object>(),
               It.IsAny<string>()
               )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var rootController = new RootController(mockAuthorizationService.Object);
            var mockURLHelper = new Mock<IUrlHelper>();
            mockURLHelper.Setup(x => 
                x.Link(
                    It.IsAny<string>(), 
                    It.IsAny<object>()))
                .Returns(string.Empty);
            rootController.Url = mockURLHelper.Object;
            //Ejecucion
            var result = await rootController.Get();
            //Verificacion
            Assert.AreEqual(2, result?.Value?.Count());
        }
    }
}
