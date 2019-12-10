using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WishGrid.Models;
using WishGrid.Tests.Shared;
using System.Linq;
using WishGrid.ViewModels;
using WishGrid.Models.Seed;

namespace WishGrid.Tests
{
    [TestClass]
    public class UTSuggestion : TestBaseEF
    {
        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void Create()
        {


            User userFound = context.User.Find(2);






            ////list of suggestions this user has
            //var suggestions = new[]
            //{
            //    new Suggestion{ Title = "sugerencia 1",Description = "Esta es la sugerencia 1" },
            //    new Suggestion{ Title = "sugerencia 2",Description = "Esta es la sugerencia 2" },
            //    new Suggestion{ Title = "sugerencia 3",Description = "Esta es la sugerencia 3" }
            //};
            Suggestion suggestion = new Suggestion()
            {
                Title = "sugerencia 2",
                Description = "Esta es la sugerencia 2",

                Author = userFound


            };
            context.Suggestion.Add(suggestion);
            Assert.IsTrue(context.SaveChanges() == 1, "The changes was not made.");
            //Assert.IsNotNull(context.User.Find(user.Id), "El user was not added.");
        }


        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void Update()
        {
            User userFound = context.User.Find(2);
            Suggestion suggestionTest = new Suggestion()
            {
                Id = 4,
                Title = "sugerencia 1",
                Description = $"Sugerencia-{GenerateTitle()}",
                Author = userFound
            };
            context.Update(suggestionTest);
            context.SaveChanges();

            Suggestion suggestionFound = context.Suggestion.Find(suggestionTest.Id);

            Assert.IsNotNull(suggestionFound, "The suggestion was not found.");
            Assert.IsTrue(suggestionFound.Description == suggestionTest.Description, "The datas was not changed.");
        }

        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void Vote()
        {
            User userFound = context.User.Find(2);
            Suggestion suggestionFound = context.Suggestion.Find(1);

            context.Add(new Votes { User = userFound, Suggestion = suggestionFound });

            Assert.IsTrue(context.SaveChanges() == 1, "vote was added");
        }

        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void Select()
        {
            //countDB = cantidad de registros en la base de datos
            int countDB = 9;
            Assert.IsTrue(context.Suggestion.Count() == countDB, "No cumple la cantidad esperada");            
        }

        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void SelectWithVotes()
        {
            IQueryable<VMSuggestion> query = (from s in context.Suggestion
                                             join u in context.User on s.AuthorId equals u.Id
                                             join v in context.Votes on s.Id equals v.SuggestionId into sv
                                             from x in sv.Where(r => r.UserId == 1).DefaultIfEmpty()
                                             orderby s.QuantityVote descending
                                             select new VMSuggestion()
                                             {
                                                 Id = s.Id,
                                                 Title = s.Title,
                                                 Description = s.Description,
                                                 QuantityVote = s.QuantityVote,
                                                 CreatedDate = s.CreatedDate,
                                                 UpdatedDate = s.UpdatedDate,
                                                 Author = new VMUserSimple()
                                                 {
                                                     Id = u.Id,
                                                     FullName = u.UserName
                                                 },
                                                 IsVoted = x.Suggestion == null
                                             }).Skip(1).Take(10);            
            Console.WriteLine("Sugestion Names:");
            foreach (var row in query)
            {
                Console.WriteLine(row.Title);
            }
        }
    }


}
