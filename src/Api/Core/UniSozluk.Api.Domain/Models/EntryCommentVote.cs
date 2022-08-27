using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Common.ViewModels;

namespace UniSozluk.Api.Domain.Models
{
    public class EntryCommentVote:BaseEntity
    {
        public Guid EntryId { get; set; }
        public VoteType VoteType { get; set; }
        public Guid CreateById { get; set; }
        public virtual EntryComment EntryComment { get; set; }
    }
}
