// See https://aka.ms/new-console-template for more information

using ArticleManagement.BL;
using ArticleManagement.DAL;
using ArticleManagement.DAL.EF;
using ArticleManagement.DAL.InMemory;
using ArticleManagement.UI.CA;
using Microsoft.EntityFrameworkCore;

// database storage
DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder<CatalogueDbContext>();
dbContextOptionsBuilder.UseSqlite("Data Source=../../../../CatalogueDatabase.db");

CatalogueDbContext catalogueDbContext = new CatalogueDbContext(dbContextOptionsBuilder.Options);
IRepository repository = new EfRepository(catalogueDbContext);

// IRepository inMemrepository = new InMemoryRepository();
IManager manager = new Manager(repository);
// InMemoryRepository.Seed();
ConsoleUi ui = new ConsoleUi(manager);


// database storage
const bool doDropDatabase = true;
bool isDbCreated = catalogueDbContext.CreateDatabase(doDropDatabase);
if (isDbCreated) DataSeeder.Seed(catalogueDbContext);

// start app
ui.Run();
