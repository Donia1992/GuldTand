using System.Collections.Generic;
using Xunit;
using FakeItEasy;
using Guldtand.Domain.Services;
using Guldtand.Domain.Models;
using GuldtandApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Guldtand.UnitTests
{
    public class EmployeeControllerTests
    {
        [Fact]
        public async void GetAllEmployeesAsync_AllDependenciesFaked_ReturnsAnOkObjectResultAndCorrectStatusCode()
        {
            //Arrange
            var expectedType = typeof(OkObjectResult);
            var expectedStatusCode = (int) HttpStatusCode.OK;

            var fakeService = A.Fake<IEmployeeService>();
            var fakeEmployeeList = A.Fake<List<IEmployee>>();

            A.CallTo(() => fakeService.GetAllEmployeesAsync()).Returns(fakeEmployeeList);

            var sut = new EmployeeController(fakeService);

            //Act
            var result = await sut.GetAllEmployeesAsync() as OkObjectResult;
            
            //Assert
            Assert.Equal(expectedType, result.GetType());
            Assert.Equal(expectedStatusCode, result.StatusCode);
        }
    }
}
