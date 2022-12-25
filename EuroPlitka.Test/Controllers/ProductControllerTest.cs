using EuroPlitka.Controllers;
using EuroPlitka.Test.Data;
using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Linq;

namespace EuroPlitka.Test.Controllers
{
    public class ProductControllerTest
    {

        private IProductRepository _productRepo;
        private ProductController _productController;
        private EuroPlitkaDbContext _EuroPlitkaDbContext;

        public ProductControllerTest()
        {
            _EuroPlitkaDbContext = InitializeStartData.GetDbContext().GetAwaiter().GetResult();
            _productRepo = new ProductRepository(_EuroPlitkaDbContext);
            _productController = new ProductController(_productRepo);
        }
        [Fact]
        public async void ProductCantroller_Index_Returns_Success()
        {
            //Arrange
            var FakeproductVm = A.Fake<ProdoctVM>();
            //Act
          
            var viewResult = await _productController.Index(FakeproductVm) as ViewResult;
            var model = (ProdoctVM?)viewResult?.ViewData.Model;

            //Assert

            model.Products.Should().NotBeNull();
            model.Products.Should().NotBeEmpty();
            viewResult.Model.Should().BeOfType<ProdoctVM>();
            model.Products.Count().Should().Be(10);







            // Assert.Equal(_EuroPlitkaDbContext.Product.Count(), model.Products.Count());
            //  Assert.True(model.Products.Any());
        }


 

        [Fact]
        public async void productController_Upsert_Product_id_Exsist()
        {
            //Arrange
            int id = 1;

            //act

            var viewResult = await _productController.Upsert(id) as ViewResult; // or   var viewResult2 = Assert.IsType<ViewResult>(result);
            var model = (ProdoctVM?)viewResult?.ViewData.Model;
            //Assert
            model.Product.Should().NotBeNull();
        }


        [Fact]
        public async void productController_Upsert_Product_id_NotExsist()
        {
            //Arrange
            int id = 999;

            //act 
            var result = await _productController.Upsert(id);

           


            //Assert
            result.Should().BeOfType(typeof(NotFoundResult));  // or  Assert.IsType<NotFoundResult>(result);
          
        }




        [Fact]
        public async Task IndexTestReturnViewResultV2()
        {
            //Arrange

            var FakeproductVm = A.Fake<ProdoctVM>();

            //Act

            var viewResult = await _productController.Index(FakeproductVm);

            //Assert

            var viewResultNew =  Assert.IsType<ViewResult>(viewResult);
            var modelNew = Assert.IsAssignableFrom<ProdoctVM>(viewResultNew.Model);
            Assert.Equal(10, modelNew.Products.Count());

        }






    }
}
