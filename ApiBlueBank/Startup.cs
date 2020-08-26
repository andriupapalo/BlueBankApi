using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiBlueBank.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using ApiBlueBank.Models.Dtos;
using ApiBlueBank.Models.Validator;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiBlueBank
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // para que json en su edserializacion permita valores grandes
        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
        // para que json en su edserializacion permita valores grandes

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // para que json en su edserializacion permita valores grandes
            services
            .AddControllersWithViews()
            .AddNewtonsoftJson();
            // para que json en su edserializacion permita valores grandes


            services.AddControllersWithViews();
            // validator con fluent
            services.AddSingleton<IValidator<ClientesDto>, ClientesDtoValidator>();
            services.AddSingleton<IValidator<CuentasDto>, CuentasDtoValidator>();
            services.AddTransient<IValidator<MovimientosDto>, MovimientosDtoValidator>();
            //injection dbcontext            
            services.AddDbContext<BANCOContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DataConection"));
            });
            
            // usanmos web tokens de Json
            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<BANCOContext>()
              .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = "yourdomain.com",
                     ValidAudience = "yourdomain.com",
                     IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["Token_secreto"])),
                     ClockSkew = TimeSpan.Zero
                 });
            // fin uso de tokens de json

            services.AddMvc();

            // para que json en su edserializacion permita valores grandes
            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            });
            // para que json en su edserializacion permita valores grandes

            //Injection Cors
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // enable authentcation
            app.UseAuthentication();

            // Enable Cors
            app.UseCors("MyPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
