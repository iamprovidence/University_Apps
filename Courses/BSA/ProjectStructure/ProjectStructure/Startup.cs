using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectStructure
{
    public class Startup
    {
        // CONSTRUCTORS
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // PROPERTIES
        public IConfiguration Configuration { get; }

        // METHODS
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfiguration>(provider => Configuration);

            DataAccessLayer.Infrastructure.AspInfrastructure.ConfigureServices(services, Configuration);
            BusinessLayer.Infrastructure.AspInfrastructure.ConfigureServices(services, Configuration);
            // RabbitMQ
            QueueService.Infrastructure.AspInfrastructure.ConfigureServices(services, Configuration);

            // service, which asynchronous checks if FileWorker
            // had enqueue some data
            services.AddHostedService<HostedServices.MessageService>();
            services.AddSignalR();

            services.AddMvc(options =>
            {
                // filter, that before each methods
                // enqueues Controller and Action's names for File Worker
                options.Filters.Add<Filters.ExecutedActionFilterAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(name: "v1", info: new Swashbuckle.AspNetCore.Swagger.Info { Title = "BinaryProject", Version = "v1" });
            });

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

            // swagger
            app.UseSwagger(opt =>
            {
                opt.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint(url: "v1/swagger.json", name: "full");
            });


            app.UseHttpsRedirection();
            app.UseMvc();

            // signarR
            app.UseSignalR(routes =>
            {
                routes.MapHub<Hubs.MessageHub>("/message");
            });
        }
    }
}
