using CodePulse.API.Data;
using CodePulse.API.Models.DTO;
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

		[HttpGet]
		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _categoryRespository.GetAllAsync();

			//map domain model to dto
			var response = new List<CategoryDto>();
			foreach(var category in categories)
			{
				response.Add(new CategoryDto
				{
					Id = category.Id,
					Name = category.Name,
					UrlHandle = category.UrlHandle	
				});
			}

			//var response = categories.Select(category => new Models.DTO.CategoryDto
			//{
			//	Id = category.Id,
			//	Name = category.Name,
			//	UrlHandle = category.UrlHandle
			//});
			return Ok(response);
		} 


		[HttpGet("{id:guid}")]	
		public async Task<IActionResult> GetCategory(Guid id)
		{
			var category = await _categoryRespository.GetAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			var response = new Models.DTO.CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};

			return Ok(response);
		} 
		
		[HttpPut]
		public async Task<IActionResult> UpdateCategory(Models.DTO.UpdateCategoryRequestDto request)
		{
			var category = new Models.Domain.Category
			{
				Id = request.Id,
				Name = request.Name,
				UrlHandle = request.UrlHandle
			};
			await _categoryRespository.UpdateAsync(category);
			var response = new Models.DTO.CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};
			return Ok(response);
		}

		[HttpDelete("{id:guid}")] 	
		public async Task<IActionResult> DeleteCategory(Guid id)
		{
			var category = await _categoryRespository.DeleteAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			// Return 204 No Content for a successful delete
			return NoContent();
		}



	}
}
