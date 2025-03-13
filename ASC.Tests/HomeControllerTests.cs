using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using ASC.Utilities;
using ASC.Web.Configuration;
using ASC.Web.Controllers;

namespace ASC.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IOptions<ApplicationSettings>> optionsMock;
        private readonly Mock<HttpContext> mockHttpContext;

        public HomeControllerTests()
        {
            // Tạo một instance của Mock IOptions
            optionsMock = new Mock<IOptions<ApplicationSettings>>();
            mockHttpContext = new Mock<HttpContext>();

            // Thiết lập FakeSession cho HttpContext Session.
            mockHttpContext.Setup(p => p.Session).Returns(new FakeSession());

            // Thiết lập thuộc tính Values của IOptions<> để trả về đối tượng ApplicationSettings
            optionsMock.Setup(ap => ap.Value).Returns(new ApplicationSettings
            {
                ApplicationTitle = "ASC"
            });
        }
        [Fact]
        public void HomeController_Index_View_Test()
        {
            var controller = new HomeController(optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            Assert.IsType(typeof(ViewResult), controller.Index());
        }

        [Fact]
        public void HomeController_Index_NoModel_Test()
        {
            var controller = new HomeController(optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            // Assert Model for Null
            Assert.Null((controller.Index() as ViewResult).ViewData.Model);
        }
        [Fact]
        public void HomeController_Index_Validation_Test()
        {
            var controller = new HomeController(optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            // Assert ModelState Error Count to 0
            Assert.Equal(0, (controller.Index() as ViewResult).ViewData.ModelState.ErrorCount);
        }
        [Fact]
        public void HomeController_Index_Session_Test()
        {
            var controller = new HomeController(optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            controller.Index();

            // Giá trị Session với khóa "Test" không được null.
            Assert.NotNull(controller.HttpContext.Session.GetSession<ApplicationSettings>("Test"));
        }
    }
}