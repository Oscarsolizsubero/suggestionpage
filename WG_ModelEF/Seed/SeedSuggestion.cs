using System;
using CsvHelper;
using System.Linq;

namespace WishGrid.Models.Seed
{
    public class SeedSuggestion : SeedBaseTemplate<Suggestion>
    {
        private DataContext _DataContext;

        public SeedSuggestion(DataContext dataContext)
        {
            _DBContext = _DataContext = dataContext;
            _Models = dataContext.Suggestion;
        }

        public override Suggestion GetRecord(CsvReader csvReader)
        {
            return new Suggestion()
            {
                Title = csvReader.GetField("Title"),
                Description = csvReader.GetField("Description"),
                AuthorId = csvReader.GetField<int>("AuthorId"),
                CreatedDate = csvReader.GetField<DateTime>("CreatedDate"),
                UpdatedDate = csvReader.GetField<DateTime>("UpdatedDate"),
                Deleted = csvReader.GetField<bool>("Deleted"),
                StatusId = csvReader.GetField<int>("StatusId")
            };
        }

        public void UpdateSumaries()
        {
            foreach (Suggestion row in _Models)
            {
                row.QuantityVote = _DataContext.Votes.Count(x => x.SuggestionId == row.Id);                
            }
            _Models.UpdateRange(_Models);
            _DBContext.SaveChanges();
        }
    }
}
