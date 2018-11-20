using AutoMapper;
using FakeItEasy;
using Guldtand.Data.Entities;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Models;
using Guldtand.Domain.Services;
using GuldtandApi.Controllers;
using GuldtandApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Guldtand.UnitTests
{
    public class UsersControllerTests
    {
        [Fact]
        public void Authenticate_ServiceReturnAUser_AuthenticateReturnsAnOkObjectResult()
        {
            //Arrange
            var expectedType = typeof(OkObjectResult);
            var expectedStatusCode = (int)HttpStatusCode.OK;

            var fakeService = A.Fake<IUserService>();
            var fakeMapper = A.Fake<IMapper>();
            var fakeSettings = A.Fake<IOptions<AppSettings>>();
            var fakeLogger = A.Fake<ILogger<UsersController>>();
            var fakeJWTHelper = A.Fake<IJWTHelper>();

            var fakeUser = A.Fake<User>();
            var fakeUserDTO = A.Fake<UserDTO>();

            var fakeUserName = "";
            var fakePassword = "";
            var fakeRole = "";
            var fakeToken = "";

            A.CallTo(() => fakeService.Authenticate(fakeUserName, fakePassword)).Returns((fakeUserDTO, fakeRole));
            A.CallTo(() => fakeJWTHelper.GenerateTokenString(fakeUserDTO, fakeRole, fakeSettings)).Returns(fakeToken);

            var sut = new UsersController(fakeService, fakeSettings, fakeLogger, fakeJWTHelper);

            //Act
            var result = sut.Authenticate(fakeUserDTO) as OkObjectResult;

            //Assert
            Assert.Equal(expectedType, result.GetType());
            Assert.Equal(expectedStatusCode, result.StatusCode);
        }
    }
}
