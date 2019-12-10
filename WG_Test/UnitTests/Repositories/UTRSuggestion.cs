using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.Tests.Shared;
using WishGrid.IRepositories;
using WishGrid.RepositoriesEF;
using WishGrid.Models;
using WishGrid.ViewModels;
using WishGrid.ViewModels.Shared;
using System.Linq;

namespace WishGrid.Tests
{
    [TestClass]
    public class UTRSuggestion
    {
        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void Create()
        {
            IRSuggestion r = new REFSuggestion(new DataContext(TestBaseEF.GetOptionBuilder().Options));
            VMSuggestion vm = new VMSuggestion()
            {
                Title = "My Test",
                Description = "My Description",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                QuantityVote = 0,
                Author = new VMUserSimple(1)
            };
            VMMessage msj = r.Insert(vm);
            Assert.IsTrue(msj.IsSuccessful());
        }

        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void Search()
        {
            IRSuggestion r = new REFSuggestion(new DataContext(TestBaseEF.GetOptionBuilder().Options));
            //var list = r.Select(10, 1, "", 1);
            //Assert.IsTrue(list.Count() > 0, "No existe filas.");
        }
    }
}
