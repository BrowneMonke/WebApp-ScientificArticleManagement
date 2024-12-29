// See https://aka.ms/new-console-template for more information

using ArticleManagement.BL;
using ArticleManagement.DAL;
using ArticleManagement.DAL.EF;
using ArticleManagement.UI.CA;
using Microsoft.EntityFrameworkCore;

// database storage
DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
dbContextOptionsBuilder.UseSqlite("Data Source=../../../../ArticleDatabase.db");

ArticleDbContext articleDbContext = new ArticleDbContext(dbContextOptionsBuilder.Options);
IRepository repository = new EfRepository(articleDbContext);

IManager manager = new Manager(repository);
ConsoleUi ui = new ConsoleUi(manager);


// DB Seeding
const bool doDropDatabase = true;
bool isDbCreated = articleDbContext.CreateDatabase(doDropDatabase);
if (isDbCreated) DataSeeder.Seed(articleDbContext);

// start app
ui.Run();
