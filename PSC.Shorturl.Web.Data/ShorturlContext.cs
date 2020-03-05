using PSC.Shorturl.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Data
{
	public class ShorturlContext : DbContext
	{
		public ShorturlContext()
			: base("name=Shorturl")
		{   
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Stat>()
                .HasRequired(s => s.ShortUrl)
                .WithMany(u => u.Stats);
				//.Map(m => m.MapKey("shortUrl_id"));
		}

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ShortUrl> ShortUrls { get; set; }
		public virtual DbSet<Stat> Stats { get; set; }
        public virtual DbSet<IPCity> IPCities { get; set; }
	}
}