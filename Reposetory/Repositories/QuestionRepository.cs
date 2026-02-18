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
        public Question AddItem(Question item)
        {
            _context.Questions.Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var question = GetById(id);
            if (question != null)
            {
                _context.Questions.Remove(question  );
                _context.save();
            }
        }

        public List<Question> GetAll()
        {
           return _context.Questions.ToList();  
        }

        public Question GetById(int id)
        {
            return _context.Questions.FirstOrDefault(q => q.QuestionId == id);
        }

        public void UpdateItem(int id, Question item)
        {
            var question = GetById(id);
            if (question != null)
            {
                question.ChapterId = item.ChapterId;
                question.Questions = item.Questions;
                _context.save();
            }
        }
    }
}
