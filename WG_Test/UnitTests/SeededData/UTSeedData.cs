using Microsoft.VisualStudio.TestTools.UnitTesting;
using WishGrid.Models;
using WishGrid.Tests.Shared;
using WishGrid.Models.Seed;

namespace WishGrid.Tests
{
    [TestClass]
    public class UTSeedData : TestBaseEF
    {
        [TestCategory(Categories.Suggestion)]
        [TestMethod]
        public void ExecuteSeed()
        {
            DataContext dataContext = new DataContext(GetOptionBuilder().Options);
            DataContext dataContextUser = new DataContext(GetOptionBuilder().Options);
            DataContext dataContextSuggestion = new DataContext(GetOptionBuilder().Options);
            using (dataContext)
            {
                //OJO NO AGREGA LOS DATOS DE FORMA ORDENADA, VERIFICAR LOS VOTOS DE LA SUGERENCIA
                new SeedTenants(dataContext).Initialize("Table_Tenants.csv");
                new SeedStatus(dataContext).Initialize("Table_Status.csv");
                new SeedRoles(dataContext).Initialize("Table_Roles.csv");
                new SeedUser(dataContextUser).Initialize("Table_User.csv");
                var seedSuggestion = new SeedSuggestion(dataContextSuggestion);
                bool any = seedSuggestion.Initialize("Table_Suggestion.csv");
                new SeedVote(dataContext).Initialize("Table_Votes.csv");
                //new SeedReplies(dataContext).Initialize("Table_Replies.csv");
                if (!any)
                {
                    seedSuggestion.UpdateSumaries();
                }
            }
        }
    }
}
