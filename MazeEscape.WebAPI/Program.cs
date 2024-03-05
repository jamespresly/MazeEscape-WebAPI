using System.Text.Json;
using System.Text.Json.Serialization;
using MazeEscape.Encoder;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine;
using MazeEscape.Engine.Interfaces;
using MazeEscape.WebAPI.Fakes;
using MazeEscape.WebAPI.Interfaces;

namespace MazeEscape.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IMazeManager, MazeManager>();

            builder.Services.AddScoped<IMazeGame, MazeGame>();
            builder.Services.AddScoped<IMazeEncoder, MazeEncoder>();
            builder.Services.AddScoped<IMazeConverter, MazeConverter>();

            builder.Services.AddScoped<IPlayerController, PlayerController>();
            builder.Services.AddScoped<IMazeGenerator, MazeGenerator>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
