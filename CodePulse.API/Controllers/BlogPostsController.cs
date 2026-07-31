using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogPostsController : Controller
	{
		public IBlogPostRepository _blogPostRepository { get; }
		public ICategoryRespository _categoryRepository { get; }

		public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRespository categoryRepository)
		{
			_blogPostRepository = blogPostRepository;
			_categoryRepository = categoryRepository;
		}


		//CRUD Functionality
		// POST - https://localhost:7154/api/blogposts

		[HttpPost]
		public async Task<IActionResult> CreateBlogPost([FromBody] Models.DTO.CreateBlogPostRequestDto request)
		{
			var blogPost = new Models.Domain.BlogPost
			{
				Title = request.Title,
				ShortDescription = request.ShortDescription,
				Content = request.Content,
				FeaturedImageUrl = request.FeaturedImageUrl,
				UrlHandle = request.UrlHandle,
				PublishedDate = request.PublishedDate,
				Author = request.Author,
				IsVisible = request.IsVisible,
				Categories = new List<Category>()
			};

			//assign cat id's:
			foreach(var catGuid in request.Categories)
			{
				var existingCategory = await _categoryRepository.GetByIdAsync(catGuid);
				if(existingCategory != null)
				{
					blogPost.Categories.Add(existingCategory);
				}

			}


			await _blogPostRepository.CreateAsync(blogPost);

			//return new item:
			var response = new Models.DTO.BlogPostDto
			{
				Id = blogPost.Id,
				Title = blogPost.Title,
				ShortDescription = blogPost.ShortDescription,
				Content = blogPost.Content,
				FeaturedImageUrl = blogPost.FeaturedImageUrl,
				UrlHandle = blogPost.UrlHandle,
				PublishedDate = blogPost.PublishedDate,
				Author = blogPost.Author,
				IsVisible = blogPost.IsVisible,
				Categories = blogPost.Categories.Select(x => new CategoryDto
				{
					Id = x.Id,
					Name = x.Name,
					UrlHandle = x.UrlHandle
				}).ToList()
			};


			return Ok(response);
		}


		// GET - https://localhost:7154/api/blogposts
		[HttpGet]
		public async Task<IActionResult> GetAllBlogPosts()
		{
			var blogposts = await _blogPostRepository.GetAllAsync();

			//map domain model to dto
			var response = new List<BlogPostDto>();
			foreach (var blogpost in blogposts)
			{
				response.Add(new BlogPostDto
				{
					Id = blogpost.Id,
					Title = blogpost.Title,
					ShortDescription = blogpost.ShortDescription, 
					Content = blogpost.Content,
					FeaturedImageUrl = blogpost.FeaturedImageUrl,
					UrlHandle = blogpost.UrlHandle,
					PublishedDate = blogpost.PublishedDate,
					Author =blogpost.Author,
					IsVisible = blogpost.IsVisible,
					Categories = blogpost.Categories.Select(x => new CategoryDto
					{
						Id = x.Id,
						Name = x.Name,
						UrlHandle = x.UrlHandle
					}).ToList()
				});
			}
			
			return Ok(response);
		}


		// GET - https://localhost:7154/api/blogposts/{id}
		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
		{
			var blogpost = await _blogPostRepository.GetByIdAsync(id);

			if (blogpost == null)
			{
				return NotFound();
			}

			var response = new Models.DTO.BlogPostDto
			{
				Id = blogpost.Id,
				Title = blogpost.Title,
				ShortDescription = blogpost.ShortDescription,
				Content = blogpost.Content,
				FeaturedImageUrl = blogpost.FeaturedImageUrl,
				UrlHandle = blogpost.UrlHandle,
				PublishedDate = blogpost.PublishedDate,
				Author = blogpost.Author,
				IsVisible = blogpost.IsVisible,
				Categories = blogpost.Categories.Select(x => new CategoryDto
				{
					Id = x.Id,
					Name = x.Name,
					UrlHandle = x.UrlHandle
				}).ToList()
			};

			return Ok(response);
		}

		// PUT - https://localhost:7154/api/blogposts/{id}
		[HttpPut]
		[Route("{id:guid}")]
		public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, [FromBody] Models.DTO.UpdateBlogPostRequestDto request)
		{
			//from dto to domain model
			var blogpost = new Models.Domain.BlogPost
			{
				Id = id,
				Title = request.Title,
				ShortDescription = request.ShortDescription,
				Content = request.Content,
				FeaturedImageUrl = request.FeaturedImageUrl,
				UrlHandle = request.UrlHandle,
				PublishedDate = request.PublishedDate,
				Author = request.Author,
				IsVisible = request.IsVisible,
				Categories = new List<Category>()
			};

			//assign cat id's:
			foreach (var catGuid in request.Categories)
			{
				var existingCategory = await _categoryRepository.GetByIdAsync(catGuid);
				if (existingCategory != null)
				{
					blogpost.Categories.Add(existingCategory);
				}

			}


			var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogpost);

			if (updatedBlogPost == null)
			{
				return NotFound();
			}

			var response = new Models.DTO.BlogPostDto
			{
				Id = updatedBlogPost.Id,
				Title = updatedBlogPost.Title,
				ShortDescription = updatedBlogPost.ShortDescription,
				Content = updatedBlogPost.Content,
				FeaturedImageUrl = updatedBlogPost.FeaturedImageUrl,
				UrlHandle = updatedBlogPost.UrlHandle,
				PublishedDate = updatedBlogPost.PublishedDate,
				Author = updatedBlogPost.Author,
				IsVisible = updatedBlogPost.IsVisible,
				Categories = updatedBlogPost.Categories.Select(x => new CategoryDto
				{
					Id = x.Id,
					Name = x.Name,
					UrlHandle = x.UrlHandle
				}).ToList()
			};
			return Ok(response);
		}

		// DELETE - https://localhost:7154/api/blogposts/{id}
		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
		{
			var blogpost = await _blogPostRepository.DeleteByIdAsync(id);
			if (blogpost == null)
			{
				return NotFound();
			}

			//// Return 204 No Content for a successful delete
			//return NoContent();

			var response = new Models.DTO.BlogPostDto
			{
				Id = blogpost.Id,
				Title = blogpost.Title,
				ShortDescription = blogpost.ShortDescription,
				Content = blogpost.Content,
				FeaturedImageUrl = blogpost.FeaturedImageUrl,
				UrlHandle = blogpost.UrlHandle,
				PublishedDate = blogpost.PublishedDate,
				Author = blogpost.Author,
				IsVisible = blogpost.IsVisible,
				Categories = blogpost.Categories.Select(x => new CategoryDto
				{
					Id = x.Id,
					Name = x.Name,
					UrlHandle = x.UrlHandle
				}).ToList()
			};
			return Ok(response);
		}


	}
}
