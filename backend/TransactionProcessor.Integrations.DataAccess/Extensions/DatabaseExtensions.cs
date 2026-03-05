using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionProcessor.Integrations.DataAccess.Contexts;

namespace TransactionProcessor.Integrations.DataAccess.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ApplyCsvDataProcessorMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TransactionProcessorContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();
        }
    }
}
