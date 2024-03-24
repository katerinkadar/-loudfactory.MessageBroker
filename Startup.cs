using Сloudfactory.MessageBroker.Models;

namespace Сloudfactory.MessageBroker
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Добавление сервисов, необходимых в проекте (например, хранилище, клиенты и т. д.)
            services.AddScoped<IStorage, Storage>();
            services.AddScoped<IClients, Clients>();

            // Добавление контроллера в сервисы
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Настройка маршрутизации для контроллера
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action}/{id?}");
            });
        }
    }
}
