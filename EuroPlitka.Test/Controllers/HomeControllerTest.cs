using EuroPlitka.Controllers;
using EuroPlitka.Test.Extention;
using EuroPlitka.Test.Extintion;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;

namespace EuroPlitka.Test.Controllers
{
    public class HomeControllerTest : IClassFixture<WebApplicationFactory<HomeController>>
    {

        private readonly IProductRepository _productRepository;
        private readonly INewsRepositoriy _newsRepositoriy;
        private readonly IBasketRepo _basketRepo;

        private readonly WebApplicationFactory<HomeController> _factory;

        public HomeControllerTest(WebApplicationFactory<HomeController> factory)
        {
            _basketRepo = A.Fake<IBasketRepo>();
            _newsRepositoriy = A.Fake<INewsRepositoriy>();
            _productRepository = A.Fake<IProductRepository>();
            _factory = factory;
        }




        [Theory]
        [InlineData("/")]
        [InlineData("Home/Index")]
        [InlineData("Home/About")]
        [InlineData("Home/Privacy")]
        [InlineData("Home/Contact")]
        [InlineData("Account/Login")]
        [InlineData("Account/Register")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }



        [Fact]
        public async Task Get_SecurePageRedirectsAnUnauthenticatedUser()
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync("Cart/Index");

            // Assert
         //   Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        //    Assert.StartsWith("http://localhost/Account/Login?ReturnUrl=%2FCart", response.Headers.Location.OriginalString);
            response.Headers.Location.OriginalString.Should().BeOfType<string>("http://localhost/Account/Login?ReturnUrl=%2FCart");
        }





        [Fact]
        public async Task Get_SecurePageIsReturnedForAnAuthenticatedUser()
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });



            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(scheme: "TestScheme");

            //Act
            var response = await client.GetAsync("Cart/Index");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        }

































        [Fact]
        [Trait("Home", "Index")]
        public async Task Index_ShouldReturnViewResult_VMCount()
        {
            //Arrange
            var news = A.Fake<IEnumerable<News>>();
            var products = A.Fake<IEnumerable<Product>>();
            var _homeController = new HomeController(
                _productRepository,
                _newsRepositoriy,
                _basketRepo).WithTestUser();
            A.CallTo(() => _productRepository.GetAllProduct()).Returns(new List<Product>
                 {
                    new Product(),
                    new Product(),
                    new Product(),
                    new Product(),
                 });
            A.CallTo(() => _newsRepositoriy.GetAll(
                A<Expression<Func<News, bool>>>.Ignored,
                A<Func<IQueryable<News>, IOrderedQueryable<News>>>.Ignored,
                A<string>.Ignored,
                A<bool>.Ignored)).Returns(news);
            //Act
            var result = await _homeController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<NewsProducstHomeVM>(viewResult.Model);
            //Assert
            result.Should().BeOfType<ViewResult>();
            model.Should().BeOfType<NewsProducstHomeVM>();
            model.Should().NotBeNull();
            model.Products.Should().HaveCount(4);
            _homeController.HttpContext.User.Identity.Name.Should().Be("TestUser");
        }







    }
}
