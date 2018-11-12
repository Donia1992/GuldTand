using AutoMapper;
using FakeItEasy;
using Guldtand.Data.Entities;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Models;
using Guldtand.Domain.Models.DTOs;
using Guldtand.Domain.Services;
using GuldtandApi.Controllers;
using GuldtandApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Guldtand.UnitTests
{
    public class UserControllerTests
    {
        [Fact]
        public async void Authenticate_ServiceReturnAUser_AuthenticateReturnsAnOkObjectResult()
        {
            //Arrange
            var expectedType = typeof(OkObjectResult);
            var expectedStatusCode = (int)HttpStatusCode.OK;

            var fakeService = A.Fake<IUserService>();
            var fakeMapper = A.Fake<IMapper>();
            var fakeSettings = A.Fake<IOptions<AppSettings>>();
            var fakeLogger = A.Fake<ILogger<UserController>>();
            var fakeJWTHelper = A.Fake<IJWTHelper>();

            var fakeUser = A.Fake<User>();
            var fakeUserDTO = A.Fake<UserDTO>();

            var fakeUserName = "";
            var fakePassword = "";
            var fakeToken = "";

            A.CallTo(() => fakeService.Authenticate(fakeUserName, fakePassword)).Returns(fakeUser);
            A.CallTo(() => fakeJWTHelper.GenerateTokenString(fakeUser, fakeSettings)).Returns(fakeToken);

            var sut = new UserController(fakeService, fakeMapper, fakeSettings, fakeLogger, fakeJWTHelper);

            //Act
            var result = await sut.Authenticate(fakeUserDTO) as OkObjectResult;

            //Assert
            Assert.Equal(expectedType, result.GetType());
            Assert.Equal(expectedStatusCode, result.StatusCode);
        }
    }
}
