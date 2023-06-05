using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using webapi;
using webapi.Controllers;

namespace webapi_test
{
    public class WeatherForecastControllerTests
    {

        [Fact]
        public async Task DbTest()
        {
            // Create DB Context
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection_tests"]);

            var context = new DataContext(optionsBuilder.Options);

            // Delete all existing weatherforecasts in DB
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Create Controller
            var repository = new WeatherForecastRepository(context);
            var controller = new WeatherForecastController(repository);

            // Add weatherforecast
            await controller.AddWeatherForecast(new WeatherForecast { Id = new Guid(), Date = DateTime.UtcNow, TemperatureC = 16, Summary = "Solskin" });

            var result = await controller.DbTest();
            
            // Assert.NotNull(result.Result);
            Assert.Null(result.Result);

        }


    }
}