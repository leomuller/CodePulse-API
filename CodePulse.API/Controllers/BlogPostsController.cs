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

		public BlogPostsController(IBlogPostRepository blogPostRepository)
		{
			_blogPostRepository = blogPostRepository;
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
				IsVisible = request.IsVisible
			};

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
				IsVisible = blogPost.IsVisible	
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
					IsVisible = blogpost.IsVisible
				});
			}
			
			return Ok(response);
		}
	}
}
