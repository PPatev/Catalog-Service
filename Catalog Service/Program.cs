using Catalog_Service.Data;
using Catalog_Service.Filters;
using Catalog_Service.Interfaces;
using Catalog_Service.Repositories;
using Catalog_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

namespace Catalog_Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options => 
            {
                options.Filters.Add<JsonExceptionFilter>();
            });

            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            builder.Services.AddApiVersioning(options => 
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            builder.Services.AddResponseCaching();

            builder.Services.AddDbContext<CatalogDbContext>(opt =>
                opt.UseInMemoryDatabase("Catalog"));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryItemService, CategoryItemService>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();
           
            app.MapControllers();

            var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            await SeedData.CreateCategories(serviceScopeFactory);

            app.Run();
        }
    }
}