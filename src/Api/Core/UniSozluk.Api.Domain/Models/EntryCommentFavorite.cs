using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Common.ViewModels;

namespace UniSozluk.Api.Domain.Models
{
    public class EntryCommentFavorite : BaseEntity
    {
        public Guid EntryCommentId { get; set; }
        public Guid CreateById { get; set; }
        public virtual EntryComment EntryComment { get; set; }
        public virtual User CreatedUser { get; set; }
    }
}
