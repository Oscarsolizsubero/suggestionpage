using CsvHelper;
using System;

namespace WishGrid.Models.Seed
{
    public class SeedReplies: SeedBaseTemplate<Reply>
    {
        public SeedReplies(DataContext dataContext)
        {
            _DBContext = dataContext;
            _Models = dataContext.Replies;
        }

        public override Reply GetRecord(CsvReader csvReader)
        {
            return new Reply()
            {
                Description = csvReader.GetField("Description"),
                Author = new User() { Id = csvReader.GetField<int>("AuthorId") },
                Suggestion = new Suggestion() { Id = csvReader.GetField<int>("SuggestionId") },                
                CreatedDate = csvReader.GetField<DateTime>("CreatedDate"),
                UpdatedDate = csvReader.GetField<DateTime>("UpdatedDate")                               
            };
        }
    }
}
