using EuroPlitka.Controllers;
using EuroPlitka.Test.Extintion;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace EuroPlitka.Test.Controllers
{
    public class HomeControllerTest
    {

        private readonly IProductRepository _productRepository;
        private readonly INewsRepositoriy _newsRepositoriy;
        private readonly IBasketRepo _basketRepo;


        public HomeControllerTest()
        {
            _basketRepo = A.Fake<IBasketRepo>();
            _newsRepositoriy = A.Fake<INewsRepositoriy>();
            _productRepository = A.Fake<IProductRepository>();
        }

        [Fact]
        [Trait("Home", "Index")]
        public  async Task Index_ShouldReturnVieeResult_VMCount()
        {
            //Arrange
            var news = A.Fake<IEnumerable<News>>();
            var products = A.Fake<IEnumerable<Product>>() ;

            var honeController = new HomeController(
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
                A<Expression<Func<News,bool>>>.Ignored,
                A<Func<IQueryable<News>, IOrderedQueryable<News>>>.Ignored, 
                A<string>.Ignored, 
                A<bool>.Ignored)).Returns(news);



            //Act

            var result = await honeController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<NewsProducstHomeVM>(viewResult.Model);

            //Assert




            result.Should().BeOfType<ViewResult>();
            model.Should().NotBeNull();
            model.Products.Should().HaveCount(4);

        //    Assert.IsType<Task<IActionResult>>(result);
        }



    }
}
