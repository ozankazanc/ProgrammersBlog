using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammersBlog.Data.Concrete.EntityFramework.Mappings
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Text).IsRequired();
            builder.Property(c => c.Text).HasMaxLength(500);
            
            builder.Property(c => c.CretedByName).IsRequired();
            builder.Property(c => c.CretedByName).HasMaxLength(50);
            builder.Property(c => c.ModifiedByName).IsRequired();
            builder.Property(c => c.ModifiedByName).HasMaxLength(50);
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.ModifiedDate).IsRequired();
            builder.Property(c => c.IsActive).IsRequired();
            builder.Property(c => c.IsDeleted).IsRequired();
            builder.Property(c => c.Note).HasMaxLength(500);

            builder.HasOne<Article>(a => a.Article).WithMany(c => c.Comments).HasForeignKey(a => a.ArticleId);

            builder.ToTable("Comments");
           
            //3 adet kayıt
            #region
            /*  builder.HasData(
           new Comment
           {
               Id = 1,
               ArticleId = 1,
               Text = "Güzel bir konuya değinmişsiniz, teşekkürler.",
               IsActive = true,
               IsDeleted = false,
               CretedByName = "InitialCreate",
               CreatedDate = DateTime.Now,
               ModifiedByName = "InitialCreate",
               ModifiedDate = DateTime.Now,
               Note = "C# Makale Yorumu."
           },
           new Comment
           {
               Id = 2,
               ArticleId = 2,
               Text = "Güzel bir konuya değinmişsiniz, sagolun.",
               IsActive = true,
               IsDeleted = false,
               CretedByName = "InitialCreate",
               CreatedDate = DateTime.Now,
               ModifiedByName = "InitialCreate",
               ModifiedDate = DateTime.Now,
               Note = "JavaScript Makale Yorumu."
           },
           new Comment
           {
               Id = 3,
               ArticleId = 3,
               Text = "Bilgilerizi paylaştıgınız için teşekkürler.",
               IsActive = true,
               IsDeleted = false,
               CretedByName = "InitialCreate",
               CreatedDate = DateTime.Now,
               ModifiedByName = "InitialCreate",
               ModifiedDate = DateTime.Now,
               Note = "JavaScript Makale Yorumu."
           });*/
            #endregion


        }
    }
}
