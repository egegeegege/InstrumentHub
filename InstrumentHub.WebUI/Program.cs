using Instrument.Business.Abstract;
using Instrument.Business.Concrate;
using Instrument.WebUI.Identity;
using InstrumentHub.DataAccess.Abstract;
using InstrumentHub.DataAccess.Concrate.EfCore;
using InstrumentHub.WebUI.MidleWares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
);

builder.Services.AddIdentity<AplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>()
.AddDefaultTokenProviders();

// Seed Identity �al��m�yor
var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<AplicationUser>>();
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;

	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.AllowedForNewUsers = true;

	options.User.RequireUniqueEmail = true;
	options.SignIn.RequireConfirmedEmail = true;
	options.SignIn.RequireConfirmedPhoneNumber = false;
});


// Cookie Options
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/account/login";
	options.LogoutPath = "/account/logout";
	options.AccessDeniedPath = "/account/accessdenied";
	options.SlidingExpiration = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
	options.Cookie = new CookieBuilder
	{
		HttpOnly = true,
		Name = "InstrumentHub.Security.Cookie",
		SameSite = SameSiteMode.Strict
	};
});

builder.Services.AddScoped<IProductDal, EfCoreEProductDal>();
builder.Services.AddScoped<IEProductServices, ProductManager>();
builder.Services.AddScoped<IDivisionDal, EfCoreDivisionDal>();
builder.Services.AddScoped<IDivisonsServices, DivisionManager>();
builder.Services.AddScoped<ICommentDal, EfCoreCommentDal>();
builder.Services.AddScoped<ICommentServices, CommentManager>();
builder.Services.AddScoped<ICartDal, EfCoreCartDal>();
builder.Services.AddScoped<ICartServices, CartManager>();
builder.Services.AddScoped<IOrderDal, EfCoreOrderDal>();
builder.Services.AddScoped<IOrderServices, OrderManager>();

builder.Services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}


app.UseStaticFiles();
app.CustomStaticFiles(); // node_modules => modules 
app.UseHttpsRedirection();
app.UseAuthentication(); // kimlik do�rulama
app.UseAuthorization(); // yetkilendirme
app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");

	endpoints.MapControllerRoute(
		name: "eproducts",
		pattern: "eproducts/{division?}",
		defaults: new { controller = "Sales", action = "Liste" }
	);


	endpoints.MapControllerRoute(
		name: "adminProducts",
		pattern: "admin/products",
		defaults: new { controller = "Admin", action = "EProductList" }
	);
	endpoints.MapControllerRoute(
		name: "adminProducts",
		pattern: "admin/products/{id}",
		defaults: new { controller = "Admin", action = "EditEProduct" }
	);
	endpoints.MapControllerRoute(
		 name: "adminProducts",
		 pattern: "admin/category",
		 defaults: new { controller = "Admin", action = "DivisionList" }
	);
	endpoints.MapControllerRoute(
		name: "adminProducts",
		pattern: "admin/categories/{id}",
		defaults: new { controller = "Admin", action = "EditCategory" }
	);
	endpoints.MapControllerRoute(
		name: "cart",
		pattern: "cart",
		defaults: new { controller = "Basket", action = "Home" }
	);
	endpoints.MapControllerRoute(
		name: "checkout",
		pattern: "checkout",
		defaults: new { controller = "Basket", action = "Checkout" }
	);
	endpoints.MapControllerRoute(
	   name: "orders",
	   pattern: "orders",
	   defaults: new { controller = "Basket", action = "GetOrders" }
   );

}
);

//SeedIdentity.Seed(userManager, roleManager, app.Configuration).Wait(); �al��mad��� i�in yorum sat�r�nda b�rakt�m

app.Run();