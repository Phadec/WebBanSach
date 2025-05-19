using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Week2.Data;
using Week2.Models;
using Week2.Repositories;
using Week2.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add database context - fix the DbContext options type
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Fix Identity services registration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    
    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cấu hình chính sách đăng nhập và cookie xác thực
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Add session services and configure them
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure JSON serialization
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Add services for repositories
builder.Services.AddScoped<IBookRepository, EFBookRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// Configure app
var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Đảm bảo bảng Orders và OrderDetails được tạo
        if (!context.Database.GetDbConnection().Database.Contains("Orders"))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Database.ExecuteSqlRawAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' and xtype='U')
                BEGIN
                    CREATE TABLE Orders (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        UserId NVARCHAR(450) NOT NULL,
                        OrderDate DATETIME2 NOT NULL,
                        TotalPrice DECIMAL(18,2) NOT NULL,
                        ShippingAddress NVARCHAR(MAX) NULL,
                        Notes NVARCHAR(MAX) NULL,
                        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
                    );

                    CREATE TABLE OrderDetails (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        OrderId INT NOT NULL,
                        BookId INT NOT NULL,
                        Quantity INT NOT NULL,
                        Price DECIMAL(18,2) NOT NULL,
                        FOREIGN KEY (OrderId) REFERENCES Orders(Id),
                        FOREIGN KEY (BookId) REFERENCES Books(Id)
                    );
                END
            ");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi xảy ra khi khởi tạo cơ sở dữ liệu.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add session before UseRouting and after UseStaticFiles
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
