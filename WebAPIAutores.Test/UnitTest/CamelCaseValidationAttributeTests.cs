using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Test.UnitTest
{
    [TestClass]
    public class CamelCaseValidationAttributeTests
    {
        [TestMethod]
        public void FirstLetterLower_ReturnError()
        {
            //Preparacion
            var camelCase = new CamelCaseValidationAttribute();
            var value = "yair";
            var valContext = new ValidationContext(new { Name = value });
            //Ejecucion
            var result = camelCase.GetValidationResult(value, valContext);
            //Verificacion
            Assert.AreEqual("The first letter must be upper", result.ErrorMessage);
        }
    }
}