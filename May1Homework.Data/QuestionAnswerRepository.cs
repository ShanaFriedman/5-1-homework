using Microsoft.EntityFrameworkCore;

namespace May1Homework.Data
{
    public class QuestionAnswerRepository
    {
        private string _connectionString;
        public QuestionAnswerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddQuestion(Question question, List<string> tags)
        {
            var context = new QuestionAnswerDbContext(_connectionString);
            question.DatePosted = DateTime.Now;
            context.Questions.Add(question);
            context.SaveChanges();
            foreach(string tag in tags)
            {
                int tagId;
                Tag t = GetTag(tag);
                if(t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }
                context.QuestionTags.Add(new QuestionTag
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }
            context.SaveChanges();
        }
        public Tag GetTag(string tagName)
        {
            var context = new QuestionAnswerDbContext(_connectionString);
            return context.Tags.FirstOrDefault(t => t.Name == tagName);
        }
        public int AddTag(string tagName)
        {
            var context = new QuestionAnswerDbContext(_connectionString);
            Tag t = new Tag { Name = tagName };
            context.Tags.Add(t);
            context.SaveChanges();
            return t.Id;
        }

        public List<Question> GetQuestions()
        {
            var context = new QuestionAnswerDbContext(_connectionString);
            return context.Questions.Include(q => q.Answers)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag).ToList();
        }

        public Question GetQuestion(int id)
        {
            var context = new QuestionAnswerDbContext(_connectionString);
            return context.Questions.Include(q => q.Answers).Include(q => q.User)
                .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag).FirstOrDefault(q => q.Id == id);
        }
        public void AddAnswer(Answer a)
        {
            var context = new QuestionAnswerDbContext(_connectionString);
            a.DatePosted = DateTime.Now;
            context.Answers.Add(a);
            context.SaveChanges();
        }
        public List <Question> GetByTag(int tagId)
        {
            var context = new QuestionAnswerDbContext(_connectionString);
            return context.Questions.Where(q => q.QuestionTags.Any(x => x.Tag.Id == tagId))
                .Include(q => q.Answers)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag).ToList();
        }
    }
}