using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
	public class BlogPostRepository : IBlogPostRepository
	{

		public ApplicationDbContext _dbContext { get; }

		public BlogPostRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<BlogPost> CreateAsync(BlogPost blogpost)
		{
			await _dbContext.BlogPosts.AddAsync(blogpost);
			await _dbContext.SaveChangesAsync();

			return blogpost;
		}

		public async Task<BlogPost?> DeleteByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<BlogPost>> GetAllAsync()
		{
			return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
		}

		public async Task<BlogPost?> GetByIdAsync(Guid id)
		{
			return await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<BlogPost?> UpdateAsync(BlogPost blogpost)
		{
			var existing = await _dbContext.BlogPosts.Include(c => c.Categories).FirstOrDefaultAsync(x => x.Id == blogpost.Id);


			if (existing == null)
			{
				return null;	//doesn't exist
			}

			// copy/update properties as needed, or use Update() if you replace the whole entity
			_dbContext.Entry(existing).CurrentValues.SetValues(blogpost);   //update blogpost
			existing.Categories = blogpost.Categories;	//update categories!
			await _dbContext.SaveChangesAsync();


			return blogpost;
		}
	}
}
