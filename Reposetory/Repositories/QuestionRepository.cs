using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class QuestionRepository : IRepository<Question>
    {
        private readonly IContext _context;
        public QuestionRepository(IContext context)
        {
               this._context = context;
        }
        public async Task<Question> AddAsync(Question item)
        {
            await _context.Set<Question>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var question = await getByIdAsync(id);
            if (question != null)
            {
                _context.Set<Question>().Remove(question);
                await _context.saveAsync();
            }
        }

        public async Task<List<Question>> getAllAsync()
        {
           return await Task.FromResult(_context.Set<Question>().ToList());  
        }

        public async Task<Question> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<Question>().FirstOrDefault(q => q.QuestionId == id));
        }

        public async Task UpdateAsync(Question item)
        {
            var question = await getByIdAsync(item.QuestionId);
            if (question != null)
            {
                question.ChapterId = item.ChapterId;
                question.Questions = item.Questions;
                question.Level = item.Level;
                await _context.saveAsync();
            }
        }
    }
}
