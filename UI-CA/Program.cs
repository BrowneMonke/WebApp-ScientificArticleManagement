// See https://aka.ms/new-console-template for more information

using ArticleManagement.BL;
using ArticleManagement.DAL;
using ArticleManagement.UI.CA;

IRepository repository = new InMemoryRepository();
IManager manager = new Manager(repository);

InMemoryRepository.Seed();

ConsoleUi ui = new ConsoleUi(manager);
ui.Run();
