using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommunIT.Entities.Context;
using Microsoft.EntityFrameworkCore;
using CommunIT.Models.Interfaces;
using CommunIT.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

namespace CommunIT.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddAzureAdBearer(options => Configuration.Bind("AzureAd", options));


            services.AddDbContext<CommunITContext>(options => options
               .UseLazyLoadingProxies()
               .UseSqlServer(Configuration.GetConnectionString("Dev")));

            services.AddScoped<ICommunITContext, CommunITContext>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommunityRepository, CommunityRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IThreadRepository, ThreadRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            services.AddRouting();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddSwaggerGen(swag =>
                swag.SwaggerDoc("v1", new Info { Title = "CommunIT WEB API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(swag =>
                swag.SwaggerEndpoint("/swagger/v1/swagger.json", "CommunITapi"));

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseAuthentication();
        }
    }
}
