using System.Runtime.ConstrainedExecution;
using Ñloudfactory.MessageBroker;
using Ñloudfactory.MessageBroker.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //  CreateHostBuilder(args).Build().Run();

        builder.Services.AddScoped<IStorage, Storage>();
        builder.Services.AddScoped<IClients, Clients>();
        builder.Services.AddScoped<IBroker, Broker>();
       
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        
    }
    public static IHostBuilder CreateBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
           });
}