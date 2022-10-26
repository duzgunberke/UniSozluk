using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Application.Interfaces.Repositories;
using UniSozluk.Api.Domain.Models;

namespace UniSozluk.Api.Infrastructure.Persistence.Repositories
{
    public class EntryRepository : GenericRepository<Entry>, IEntryRepository
    {
        public EntryRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
