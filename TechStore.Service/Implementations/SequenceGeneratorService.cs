using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Data.UnitOfWork;

namespace TechStore.Service.Implementations
{
    public class SequenceGeneratorService
    {
        private readonly IUnitOfWork _uow;
        public SequenceGeneratorService(IUnitOfWork uow) 
        { 
            _uow = uow;
        }

        public async Task<string> GetNextCategoryIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Category);
            var Id = $"CATE-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<long> GetNextSequenceValueAsync(string sequenceName)
        {
            var sequence = await _uow.Sequences.FindOneAsync(sequence => sequence.Id == sequenceName);

            if (sequence == null)
            {
                await _uow.Sequences.AddAsync(new Data.Entities.Sequence { Id = sequenceName, Value = 1 });
                return 1;
            }

            sequence.Value += 1;

            _uow.Sequences.Update(sequence);

            return sequence.Value;
        }
    }
}
