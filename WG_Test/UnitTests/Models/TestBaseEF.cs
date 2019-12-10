using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WishGrid.Models;
using WishGrid.Setup.Shared;

namespace WishGrid.Tests
{
    [TestClass]
    public abstract class TestBaseEF
    {
        protected DataContext context;

        public static DbContextOptionsBuilder<DataContext> GetOptionBuilder()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            AppSettings.BuildDBContext(optionsBuilder);
            return optionsBuilder;
        }

        [TestInitialize()]
        public void TestInitialize()
        {            
            context = new DataContext(GetOptionBuilder().Options);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }

        protected string GenerateTitle()
        {
            return DateTime.Now.ToString("ffff-HHmmssddMM");
        }
    }
}
