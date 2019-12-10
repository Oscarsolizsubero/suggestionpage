using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.Models;
using WishGrid.ViewModels;
using WishGrid.ViewModels.Shared;

namespace WishGrid.IRepositories
{
    public abstract class IRSuggestion : IRepositoryCRUDTemplate<VMSuggestion, int>
    {
        public abstract override string TInsert(VMSuggestion viewModel);

        public abstract override void TUpdate(VMSuggestion viewModel);

        public abstract override void TDelete(VMSuggestion viewModel);

        public abstract override VMSuggestion Select(int id);

        public abstract int CountPrivate(string tenant, int statusId, string filters = null);

        public abstract int CountPublic(string tenant, int statusId, string filters = null);

        public abstract IEnumerable<VMSuggestion> SelectPublic(int pageSize, int pageNumber, string filters, int idUser, string tenant, int statusId);

        public abstract IEnumerable<VMSuggestion> SelectPrivate(int pageSize, int pageNumber, string filters, int idUser, string tenant, int statusId);

        public abstract VMMessage Vote(VMVote votes);

        public abstract bool IsVote(VMVote vmvote);

        public abstract bool IsAuthor(VMVote vmvote);

        public abstract bool isFullfilled(int suggestionId);

        public abstract void VoteDelete(VMVote vote);

        public abstract VMSuggestion Select(VMVote votes,string tenant);

        public abstract bool Moderation(int IdAuthor);

        public abstract bool AdminorModerator(int IdUser);

        public abstract VMMessage UpdateStatus(VMSuggestionEditStatus model);
    }
}
