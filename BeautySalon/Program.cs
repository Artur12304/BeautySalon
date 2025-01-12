namespace BeautySalon
{
    using BeautySalon.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting up the application");

                // Configure the application
                var startup = new Startup(builder.Configuration);
                startup.ConfigureServices(builder.Services);

                var app = builder.Build();
                startup.Configure(app, builder.Environment);

                try
                {
                    using (var scope = app.Services.CreateScope())
                    {
                        Log.Information("Applying migrations");
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        dbContext.Database.Migrate();
                        Log.Information("Migrations applied successfully");
                        dbContext.Seed();
                        Log.Information("Seeds applied successfully");
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Error during migration");
                    throw;
                }

                app.Run();
            }
            catch (Exception ex) when (ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design")
            {
                Log.Fatal(ex, "Web host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
