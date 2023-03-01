using BookingApi.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Extensions
{
    public static class DbMigrationExtensions
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            return app;
        }
    }
}
