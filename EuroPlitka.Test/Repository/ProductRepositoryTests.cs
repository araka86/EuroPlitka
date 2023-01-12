using EuroPlitka.Controllers;
using EuroPlitka.Test.Data;
using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using FakeItEasy;
using FluentAssertions;
using Moq;
using Syncfusion.EJ2.Linq;
using System.Collections.Immutable;

namespace EuroPlitka.Test.Repository
{
    public class ProductRepositoryTests
    {
        private IProductRepository _productRepo;
        private EuroPlitkaDbContext _EuroPlitkaDbContext;

        public ProductRepositoryTests()
        {
            _EuroPlitkaDbContext = InitializeStartData.GetDbContext().GetAwaiter().GetResult();
            _productRepo = new ProductRepository(_EuroPlitkaDbContext);

        }





        [Fact]
        public async void ProductRerository_GetAll_ReturnsProductList()
        {
            //Arrange


            var prodVm = new ProdoctVM()
            {
                Products = await _productRepo.GetAllProduct(),
                CategorySelectList = await _productRepo.GetAllDropdownList(WebConstanta.CategoryName),
                ProductTypeSelectList = await _productRepo.GetAllDropdownList(WebConstanta.ProductTypeName)

            };

            //Act


            //Assert
            prodVm.Products.Should().NotBeNull();
            prodVm.Products.Should().BeOfType<List<Product>>();
        }





        [Fact]
        public async void ProductRerository_GetByIdAsync_ReturnsProductExist()
        {
            //Arrange
            var id = 1;

            //Act
            var result = _productRepo.FirstOrDefault(x => x.Id == id);

            //Assert
            result.Result.Should().NotBeNull();
            result.Should().BeOfType<Task<Product>>();
            result.Status.Should().Be(TaskStatus.RanToCompletion);
        }


        [Fact]
        public async void ProductRerository_GetByIdAsync_ReturnsProductNotExist()
        {
            //Arrange
            var id = 99999;


            //Act
            var result = _productRepo.FirstOrDefault(x => x.Id == id);

            //Assert
            result.Result.Should().BeNull();
            result.Should().BeOfType<Task<Product>>();
            result.Status.Should().Be(TaskStatus.RanToCompletion);

        }






        //Add product
        [Fact]
        [Trait("ProductRepo", "Add Product")]
        public async void ProductRerository_Add_ReturnsBool()
        {
            //Arrange
            var product = new Product()
            {
                Name = "NewProduct",
                shortDesc = "shDesc",
                Description = "desc",
                Price = 2000,
                Category = new Category()
                {
                    NameUa = "NewUaName",
                    NameEng = "NewEngName"
                },
                ProductType = new ProductType()
                {
                    Name = "New Type of Product"
                }
            };



            //Act
            var result = _productRepo.Add(product);

            var addedProd = _productRepo.FirstOrDefault(x => x.Id == product.Id);

            //Assert
            result.Should().BeTrue();
            _EuroPlitkaDbContext.Product.Count().Should().Be(11);
            addedProd.Result.Name.Should().Be("NewProduct");
        }



        [Fact]
        public async void ProductRerository_Update_ReturnsBool()
        {
            //Arrange
            string UpdateName = "UpdateName";
            Product findProduct = _productRepo.FirstOrDefault(x => x.Id == 2).Result;
            findProduct.Name = UpdateName;
            //Act
            var result = _productRepo.Update(findProduct);
            var findUdpateName = _productRepo.FirstOrDefault(x => x.Name == UpdateName);
            //Assert
            result.Should().BeTrue();
            findUdpateName.Result.Should().NotBeNull();
            findUdpateName.Result.Name.Should().Be("UpdateName");
        }




        [Fact]
        public  void ProductRerository_UpdatePrice_ReturnsBool()
        {
            //Arrange

            EuroPlitkaDbContext _EuroPlitkaDbContextt = InitializeStartData.GetDbContext().GetAwaiter().GetResult();
            IProductRepository _productRepository = new ProductRepository(_EuroPlitkaDbContextt);
            double UpdatePrice = 17.500;
            int id = 2;
            Product findProduct = _productRepository.FirstOrDefault(x => x.Id == id).Result;
            findProduct.Price = UpdatePrice;

            //Act
            var result = _productRepository.Update(findProduct);
            var findUdpateName = _productRepository.FirstOrDefault(x => x.Id == id);

            //Assert
            result.Should().BeTrue();
            findUdpateName.Result.Should().NotBeNull();
            findUdpateName.Result.Price.Should().Be(UpdatePrice);
        }
















        //delete product
        [Fact]
        [Trait("ProductRepo", "Delete Product")]
        public async void ProductRepository_SuccessfulDelete_ReturnsTrue()
        {

            //Act
            var findProd = _EuroPlitkaDbContext.Product.Find(1);
            var result = _productRepo.Delete(findProd);
            var count = await _productRepo.GetCountAsync();

            //Assert
            result.Should().BeTrue();
            count.Should().Be(9);
        }


        //delete Allproduct
        [Fact]
        public async void ProductRepository_SuccessfulDeleteAll_ReturnsTrue()
        {
            //Arrange
            IEnumerable<Product> products = _EuroPlitkaDbContext.Product;

            //Act        
            var result = _productRepo.RemoveRange(products);
            var count = await _productRepo.GetCountAsync();

            //Assert
            result.Should().BeTrue();
            count.Should().Be(0);
        }






        [Fact]
        [Trait("ProductRepo", "Count Product")]
        public async void ProductRepository_GetCount_ReturnsCount()
        {

            //Arrange
          
          


            int countProduct = 10;
            var _prodRepo = A.Fake<IProductRepository>();
            A.CallTo(() => _prodRepo.GetCountAsync()).Returns(countProduct);

            //Act
            var count = await _prodRepo.GetCountAsync();
            //Assert

            count.Should().Be(countProduct);
        }



        

        //інтеграційно-функціональний
        [Fact]
        [Trait("ProductRepo", "ReturnCount")]
        public async Task ProductRepository_GetCountProduct()
        {

            Mock<IProductRepository> Imock = new Mock<IProductRepository>();
            var fakrRepo = A.Fake<IProductRepository>();

            var controller = new Mock<ProductController>(Imock.Object);


          

            var rrr = controller.Object.Upsert(2);

            //Arrange
            var _EuroPlitkaDbContext = await InitializeStartData.GetDbContext();
            var _productRepo =  new ProductRepository(_EuroPlitkaDbContext);
            
            //Act
            var count = await _productRepo.GetCountAsync();

            //Assert
            count.Should().Be(10);
        }











    }
}
