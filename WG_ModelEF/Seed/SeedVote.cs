using CsvHelper;

namespace WishGrid.Models.Seed
{
    public class SeedVote : SeedBaseTemplate<Votes>
    {
        public SeedVote(DataContext dataContext)
        {
            _DBContext = dataContext;
            _Models = dataContext.Votes;
        }

        public override Votes GetRecord(CsvReader csvReader)
        {
            return new Votes()
            {
                UserId = csvReader.GetField<int>("UserId"),
                SuggestionId = csvReader.GetField<int>("SuggestionId")
            };
        }
    }
}
