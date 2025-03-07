using Microsoft.EntityFrameworkCore;
using RedisExercise.Tables;

namespace RedisExercise.Context
{
	public class DbRedis:DbContext
	{
		public DbSet<Product> Products { get; set; }

		public DbRedis(DbContextOptions<DbRedis> options): base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

		}
	}
}
