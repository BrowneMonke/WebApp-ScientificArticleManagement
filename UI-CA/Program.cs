// See https://aka.ms/new-console-template for more information

using ArticleManagement.BL;
using ArticleManagement.DAL;
using ArticleManagement.DAL.EF;
using ArticleManagement.DAL.InMemory;
using ArticleManagement.UI.CA;
using Microsoft.EntityFrameworkCore;

// database storage
DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
dbContextOptionsBuilder.UseSqlite("Data Source=../../../../ArticleDatabase.db");

ArticleDbContext articleDbContext = new ArticleDbContext(dbContextOptionsBuilder.Options);
IRepository repository = new EfRepository(articleDbContext);

// IRepository inMemrepository = new InMemoryRepository();
IManager manager = new Manager(repository);
// InMemoryRepository.Seed();
ConsoleUi ui = new ConsoleUi(manager);


// database storage
const bool doDropDatabase = true;
bool isDbCreated = articleDbContext.CreateDatabase(doDropDatabase);
if (isDbCreated) DataSeeder.Seed(articleDbContext);

// start app
ui.Run();
