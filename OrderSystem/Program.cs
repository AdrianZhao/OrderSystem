using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderSystem.Data;
using OrderSystem.Models;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OrderSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderSystemContext") ?? throw new InvalidOperationException("Connection string 'OrderSystemContext' not found.")));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IRepository<Product>), typeof(ProductRepository));
builder.Services.AddScoped(typeof(IRepository<InCartProduct>), typeof(InCartProductRepository));
builder.Services.AddScoped(typeof(IRepository<InOrderProduct>), typeof(InOrderProductRepository));
builder.Services.AddScoped(typeof(IRepository<Cart>), typeof(CartRepository));
builder.Services.AddScoped(typeof(IRepository<Order>), typeof(OrderRepository));
builder.Services.AddScoped(typeof(IRepository<Country>), typeof(CountryRepository));

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderSystemContext>();
    context.Database.EnsureDeleted(); // Drop the database
    context.Database.Migrate(); // Create a new database and apply migrations
}

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}");

app.Run();
