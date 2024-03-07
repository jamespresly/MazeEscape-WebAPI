using MazeEscape.Encoder;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine;
using MazeEscape.Engine.Interfaces;
using MazeEscape.WebAPI.DTO;
using MazeEscape.WebAPI.Interfaces;
using Microsoft.AspNetCore.HttpLogging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace MazeEscape.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });
            

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IMazeManager, MazeManager>();

            builder.Services.AddScoped<IMazeGame, MazeGame>();
            builder.Services.AddScoped<IMazeEncoder, MazeEncoder>();
            builder.Services.AddScoped<IMazeConverter, MazeConverter>();

            builder.Services.AddScoped<IPlayerController, PlayerController>();
            builder.Services.AddScoped<IMazeGenerator, MazeGenerator>();


            builder.Services.AddHttpLogging(x =>
            {
                x.LoggingFields = HttpLoggingFields.All;
                x.CombineLogs = true;
            });

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.File("Logs/MazeEscapeHttpLogs.txt", rollingInterval: RollingInterval.Day));

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var mazeConfig = new MazeManagerConfig();
            builder.Configuration.GetSection("MazeManager").Bind(mazeConfig);
            builder.Services.AddSingleton(mazeConfig);


            var app = builder.Build();


            mazeConfig.FullPresetsPath = builder.Environment.ContentRootPath + mazeConfig.PresetsPath;


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseHttpLogging();
                
                // dev only encryption key - use a secrets manager for production
                mazeConfig.MazeEncryptionKey = "yNiPC0Se/P5fO2ie4mdmpIIk/IQbGg+AYKrOBGGX1q4=";
            }


            app.UseAuthorization();

            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
