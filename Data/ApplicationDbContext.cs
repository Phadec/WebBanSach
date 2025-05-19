using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Week2.Models;

namespace Week2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookImages> BookImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<BookImages>()
                .HasOne(bi => bi.Book)
                .WithMany(b => b.Images)
                .HasForeignKey(bi => bi.BookId);
                
            // Configure Order relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);
                
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);
                
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Book)
                .WithMany()
                .HasForeignKey(od => od.BookId);
                
            // Configure decimal precision
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasColumnType("decimal(18,2)");

            // Seed data for categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Non-Fiction" },
                new Category { Id = 3, Name = "Science Fiction" },
                new Category { Id = 4, Name = "Medical" }
            );

            // Seed data for books
            modelBuilder.Entity<Book>().HasData(
                new Book { 
                    Id = 1, 
                    Title = "Death in the Clan", 
                    Author = "Carly Reid", 
                    PublishYear = 2023, 
                    Price = 2.5, 
                    Cover = "images/book1.jpg",
                    CategoryId = 1
                },
                // Medical books
                new Book { 
                    Id = 2, 
                    Title = "Gray's Anatomy", 
                    Author = "Henry Gray", 
                    PublishYear = 2020, 
                    Price = 89.99, 
                    Cover = "images/medical1.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 3, 
                    Title = "Harrison's Principles of Internal Medicine", 
                    Author = "J. Larry Jameson", 
                    PublishYear = 2022, 
                    Price = 149.99, 
                    Cover = "images/medical2.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 4, 
                    Title = "Robbins Basic Pathology", 
                    Author = "Vinay Kumar", 
                    PublishYear = 2021, 
                    Price = 79.99, 
                    Cover = "images/medical3.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 5, 
                    Title = "Guyton and Hall Textbook of Medical Physiology", 
                    Author = "John E. Hall", 
                    PublishYear = 2021, 
                    Price = 94.99, 
                    Cover = "images/medical4.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 6, 
                    Title = "Clinical Pharmacology", 
                    Author = "Morris J. Brown", 
                    PublishYear = 2019, 
                    Price = 69.99, 
                    Cover = "images/medical5.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 7, 
                    Title = "Davidson's Principles and Practice of Medicine", 
                    Author = "Stuart H. Ralston", 
                    PublishYear = 2022, 
                    Price = 84.99, 
                    Cover = "images/medical6.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 8, 
                    Title = "Netter's Atlas of Human Anatomy", 
                    Author = "Frank H. Netter", 
                    PublishYear = 2019, 
                    Price = 74.99, 
                    Cover = "images/medical7.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 9, 
                    Title = "Oxford Handbook of Clinical Medicine", 
                    Author = "Ian B. Wilkinson", 
                    PublishYear = 2020, 
                    Price = 49.99, 
                    Cover = "images/medical8.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 10, 
                    Title = "Kumar and Clark's Clinical Medicine", 
                    Author = "Parveen Kumar", 
                    PublishYear = 2021, 
                    Price = 89.99, 
                    Cover = "images/medical9.jpg",
                    CategoryId = 4
                },
                new Book { 
                    Id = 11, 
                    Title = "Diseases of the Human Body", 
                    Author = "Carol D. Tamparo", 
                    PublishYear = 2022, 
                    Price = 64.99, 
                    Cover = "images/medical10.jpg",
                    CategoryId = 4
                }
            );
        }
    }
}
