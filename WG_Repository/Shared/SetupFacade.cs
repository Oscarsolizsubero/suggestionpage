using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WishGrid.IRepositories;
using WishGrid.Models;
using WishGrid.Models.Seed;
using WishGrid.RepositoriesEF;

namespace WishGrid.Setup.Shared
{
    public class SetupFacade
    {
        private IServiceCollection _ServiceCollection;
        private IServiceProvider _ServiceProvider;

        public SetupFacade(IServiceCollection serviceCollection)
        {
            _ServiceCollection = serviceCollection;
        }

        public SetupFacade(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }

        public void SeededData()
        {
            using (var scope = _ServiceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    DataContext dataContext = new DataContext(serviceProvider.GetRequiredService<DbContextOptions<DataContext>>());
                    using (dataContext)
                    {
                        new SeedTenants(dataContext).Initialize("Table_Tenants.csv");
                        new SeedRoles(dataContext).Initialize("Table_Roles.csv");
                        new SeedUser(dataContext).Initialize("Table_User.csv");
                        var seedSuggestion = new SeedSuggestion(dataContext);
                        bool any = seedSuggestion.Initialize("Table_Suggestion.csv");
                        new SeedVote(dataContext).Initialize("Table_Votes.csv");
                        new SeedReplies(dataContext).Initialize("Table_Replies.csv");
                        if (!any)
                        {
                            seedSuggestion.UpdateSumaries();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = _ServiceProvider.GetRequiredService<ILogger<SetupFacade>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
        }

        public void InjectionBuildDatabase()
        {
            _ServiceCollection.AddDbContext<DataContext>(options => AppSettings.BuildDBContext(options));
        }

        public void InjectionBuildRepository()
        {
            _ServiceCollection.AddScoped<IRUser, REFUser>();
            _ServiceCollection.AddScoped<IRSuggestion, REFSuggestion>();
            _ServiceCollection.AddScoped<IRReply, REFReply>();
            _ServiceCollection.AddScoped<IRTenant, REFTenant>();
            _ServiceCollection.AddScoped<IRStatus, REFStatus>();
        }

        /*
         public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CodingBlastDbContext>
{
    public CodingBlastDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
 
        var builder = new DbContextOptionsBuilder<CodingBlastDbContext>();
 
        var connectionString = configuration.GetConnectionString("DefaultConnection");
 
        builder.UseSqlServer(connectionString);
 
        return new CodingBlastDbContext(builder.Options);
    }
}
         */
    }
}
