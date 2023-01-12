using EuroPlitka.Controllers;
using EuroPlitka.Test.Extintion;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EuroPlitka.Test.Controllers
{
    public class CategoryControllerTest
    {

        private readonly ICategoryRepository _IcategoryRepository;
        private readonly IProductRepository _IproductRepository;
        private readonly CategoryController _categoryController;



        public CategoryControllerTest()
        {
            _IcategoryRepository = A.Fake<ICategoryRepository>();
            _IproductRepository = A.Fake<IProductRepository>();
            _categoryController = new CategoryController(_IcategoryRepository, _IproductRepository);
        }


        [Fact]
        public async Task GetIndex()
        {

            //Arrange
            IEnumerable<Category> categories = A.Fake<IEnumerable<Category>>();



            //Act
            A.CallTo(() => _IcategoryRepository.GetAll(
                   A<Expression<Func<Category, bool>>>.Ignored,
                   A<Func<IQueryable<Category>, IOrderedQueryable<Category>>>.Ignored,
                   A<string>.Ignored,
                   A<bool>.Ignored)).Returns(categories);




            var result = await _categoryController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            //Assert
            Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.Model);
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task CreateCategory()
        {
            //Arrange
            Category obj = A.Fake<Category>();

            //Act
            A.CallTo(() => _IcategoryRepository.Add(obj)).Returns(true);

            var result = _categoryController.Create(obj);

            //Assert
            result.Should().BeOfType<RedirectResult>();
        }


        [Fact]
        public async Task EditCategory()
        {

            //Arrange
            int id =1;
            Category obj = A.Fake<Category>();

            //act
            A.CallTo(() => _IcategoryRepository.Find(id)).Returns(obj);

           var result  = _categoryController.Edit(obj);

            //assert
            result.Should().BeOfType<RedirectToActionResult>();



        }




        [Fact]
        public async Task GetIndex2()
        {


          
            //Arrange
            IEnumerable<Category> categories = A.Fake<IEnumerable<Category>>();



            //Act
         A.CallTo(() => _IcategoryRepository.GetAll(
                   A<Expression<Func<Category, bool>>>.Ignored,
                   A<Func<IQueryable<Category>, IOrderedQueryable<Category>>>.Ignored,
                   A<string>.Ignored,
                   A<bool>.Ignored)).Returns(Task.FromResult(categories));



         
            var result = await _categoryController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            //Assert
            Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.Model);
            result.Should().BeOfType<ViewResult>();
        }












    }
}
