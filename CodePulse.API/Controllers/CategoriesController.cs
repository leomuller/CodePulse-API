using CodePulse.API.Data;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		public ICategoryRespository _categoryRespository { get; }

		public CategoriesController(ICategoryRespository categoryRespository)
		{
			_categoryRespository = categoryRespository;
		}


		//CRUD Functionality

		[HttpPost]
		public async Task<IActionResult> CreateCategory(Models.DTO.CreateCategoryRequestDto request)
		{
			var category = new Models.Domain.Category
			{
				Name = request.Name,
				UrlHandle = request.UrlHandle
			};

			await _categoryRespository.CreateAsync(category);

			//return new item:
			var response = new Models.DTO.CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};


			return Ok(response);
		}

	}
}
