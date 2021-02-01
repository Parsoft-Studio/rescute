using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using rescute.Infrastructure;
using FluentValidation.AspNetCore;
using FluentValidation;
using rescute.API.Validators;
using rescute.API.Services;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace rescute.API
{
    public class Startup
    {
        private string rootPath;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            rootPath = Path.Combine(Configuration.GetValue<string>(WebHostDefaults.ContentRootKey),"content");

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "rescute.API", Version = "v1" });
            });
            services.AddTransient<rescuteContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IFileStorageService>(provider => new FileStorageService(rootPath, new List<string>() { "jpg", "avi", "mpg", "png" }));

            services.AddMvc().AddFluentValidation();
            services.AddValidatorsFromAssemblyContaining<AnimalValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "rescute.API v1"));
            }

            app.UseRouting();
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(rootPath),
                RequestPath =Configuration["RelativeContentRootPath"]
            }
                );
            env.WebRootPath = rootPath;
            
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
