using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Context;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.Repositories.Implementations
{
    public class SequenceRepository : Repository<Sequence>, ISequenceRepository
    {
        public SequenceRepository(AppDbContext context) : base(context) { }
    }
}
