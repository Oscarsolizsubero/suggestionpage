using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using WishGrid.Models;

namespace WishGrid.Setup.Shared
{
    public abstract class AppSettings
    {
        public static IConfiguration Configuration { get; set; }

        static AppSettings()
        {            
            void DeletePath(string folder, ref string directory)
            {
                if (directory.Contains($"\\{folder}"))
                {
                    directory = Path.GetFullPath(Path.Combine(directory, @"..\"));
                }
            }
            string path = Directory.GetCurrentDirectory();
            string projectPrimaryPath = "\\WG_ControllerRS\\";
            DeletePath("netcoreapp2.0",ref path);
            DeletePath("Debug", ref path);
            DeletePath("bin", ref path);
            if (!path.Contains(projectPrimaryPath))
            {
                path = path.Replace("\\WG_Test\\", projectPrimaryPath);
                Build(path);
            }
        }

        public static void Build(string path)
        {
            var builder = new ConfigurationBuilder().
                          SetBasePath(path).
                          AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public static void Build()
        {
            Build(Directory.GetCurrentDirectory());
        }

        public static string GetConnectionString()
        {
            return Configuration.GetConnectionString("DefaultConnection");
        }

        public static DbContextOptionsBuilder BuildDBContext(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseSqlServer(GetConnectionString());
        }

        public static DataContext CreateDbContext()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new DataContext(builder.Options);
        }
    }
}
