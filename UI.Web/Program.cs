using Application.Interfaces.Contexts;
using Application.ViewModel.AttachmentType;
using Application.ViewModel.Customers;
using Application.ViewModel.FileUpload;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Connection Service
string? connection = builder.Configuration.GetConnectionString("ConnectionSql");
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));
#endregion

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IAttachmentTypeService, AttachmentTypeService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FileUpload}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
