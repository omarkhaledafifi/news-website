using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsWebsite.DAL.Etities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.DAL.Configurations
{
    internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            
            //builder.HasOne(P => P.ProductBrand)
            //    .WithMany()
            //    .HasForeignKey(P => P.BrandId);

            //builder.HasOne(P => P.ProductType)
            //    .WithMany()
            //    .HasForeignKey(P => P.TypeId);

            //builder.Property(P => P.Price).HasColumnType("decimal(18,3)");
        }
    }
}
