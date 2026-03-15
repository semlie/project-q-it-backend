using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AnswerOptionRepository : IRepository<AnswerOptions>
    {
        private readonly IContext _context;
        public AnswerOptionRepository(IContext context)
        {
               this._context = context;
        }
        public async Task<AnswerOptions> AddAsync(AnswerOptions item)
        {
            await _context.Set<AnswerOptions>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var answerOption = await getByIdAsync(id);
            if (answerOption != null)
            {
                _context.Set<AnswerOptions>().Remove(answerOption);
                await _context.saveAsync();
            }
        }

        public async Task<List<AnswerOptions>> getAllAsync()
        {
           return await Task.FromResult(_context.Set<AnswerOptions>().ToList());  
        }

        public async Task<AnswerOptions> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<AnswerOptions>().FirstOrDefault(a => a.AnswerOptionsId == id));
        }

        public async Task UpdateAsync(AnswerOptions item)
        {
            var answerOption = await getByIdAsync(item.AnswerOptionsId);
            if (answerOption != null)
            {      
                 answerOption.QuestionId = item.QuestionId;
                 answerOption.Option = item.Option;
                 answerOption.IsCorrect = item.IsCorrect;
                 answerOption.Description = item.Description;       
                await _context.saveAsync();
            }
        }
    }
}
