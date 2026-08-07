using CodePulse.API.Data;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
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
		// POST - https://localhost:7154/api/categories

		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody] Models.DTO.CreateCategoryRequestDto request)
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

		// GET - https://localhost:7154/api/categories
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _categoryRespository.GetAllAsync();

			//map domain model to dto
			var response = new List<CategoryDto>();
			foreach (var category in categories)
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

		// GET - https://localhost:7154/api/categories/{id}
		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
		{
			var category = await _categoryRespository.GetByIdAsync(id);

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

		// PUT - https://localhost:7154/api/categories/{id}
		[HttpPut]
		[Route("{id:guid}")]
		public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] Models.DTO.UpdateCategoryRequestDto request)
		{
			var category = new Models.Domain.Category
			{
				Id = id,
				Name = request.Name,
				UrlHandle = request.UrlHandle
			};

			var updatedCategory = await _categoryRespository.UpdateAsync(category);

			if(updatedCategory == null)
			{
				return NotFound();
			}

			var response = new Models.DTO.CategoryDto
			{
				Id = updatedCategory.Id,
				Name = updatedCategory.Name,
				UrlHandle = updatedCategory.UrlHandle
			};
			return Ok(response);
		}

		// DELETE - https://localhost:7154/api/categories/{id}
		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
		{
			var category = await _categoryRespository.DeleteByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			//// Return 204 No Content for a successful delete
			//return NoContent();

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
