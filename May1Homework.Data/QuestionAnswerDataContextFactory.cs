using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace May1Homework.Data
{
    public class QuestionAnswerDataContextFactory : IDesignTimeDbContextFactory<QuestionAnswerDbContext>
    {
        public QuestionAnswerDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}May1Homework.Web"))
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new QuestionAnswerDbContext(config.GetConnectionString("ConStr"));
        }
    }
}
