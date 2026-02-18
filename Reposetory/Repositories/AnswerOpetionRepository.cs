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
        public AnswerOptions AddItem(AnswerOptions item)
        {
            _context.AnswerOptions.Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var answerOption = GetById(id);
            if (answerOption != null)
            {
                _context.AnswerOptions.Remove(answerOption);
                _context.save();
            }
        }

        public List<AnswerOptions> GetAll()
        {
           return _context.AnswerOptions.ToList();  
        }

        public AnswerOptions GetById(int id)
        {
            return _context.AnswerOptions.FirstOrDefault(a => a.AnswerOptionsId == id);
        }

        public void UpdateItem(int id, AnswerOptions item)
        {
            var answerOption = GetById(id);
            if (answerOption != null)
            {      
                 answerOption.QuestionId = item.QuestionId;
                 answerOption.Option = item.Option;
                 answerOption.IsCorrect = item.IsCorrect;
                 answerOption.Description = item.Description;       
                _context.save();
            }
        }
    }
}
