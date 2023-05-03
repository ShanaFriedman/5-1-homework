using May1Homework.Data;
using May1Homework.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace May1Homework.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            var repo = new QuestionAnswerRepository(_connectionString);
            HomeViewModel vm = new()
            {
                Questions = repo.GetQuestions()

            };
            return View(vm);
        }
        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddQuestion(Question question, List<string> tags)
        {
            var userRepo = new UserRepository(_connectionString);
            var QARepo = new QuestionAnswerRepository(_connectionString);

            var currentUserEmail = User.Identity.Name;
            User currentUser = userRepo.GetByEmail(currentUserEmail);
            question.UserId = currentUser.Id;
            QARepo.AddQuestion(question, tags);
            return Redirect("/");
        }
        public IActionResult ViewQuestion(int id)
        {
            var QARepo = new QuestionAnswerRepository(_connectionString);
            ViewQuestionViewModel vm = new()
            {
                Question = QARepo.GetQuestion(id)
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer(Answer a)
        {
            var userRepo = new UserRepository(_connectionString);
            var QARepo = new QuestionAnswerRepository(_connectionString);

            var currentUserEmail = User.Identity.Name;
            User currentUser = userRepo.GetByEmail(currentUserEmail);
            a.UserId = currentUser.Id;
            QARepo.AddAnswer(a);
            return Redirect("/");
        }
        public IActionResult ViewByTag(int tagId)
        {
            var QARepo = new QuestionAnswerRepository(_connectionString);
            HomeViewModel vm = new()
            {
                Questions = QARepo.GetByTag(tagId)
            };
            return View(vm);
        }
    }
}