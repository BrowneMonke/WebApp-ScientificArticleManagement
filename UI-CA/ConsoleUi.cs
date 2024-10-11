using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.BL.Domain.Extensions;

namespace ArticleManagement.UI.CA;

public class ConsoleUi
{
    private bool _quitConsoleApp = false;
    private readonly IManager _manager;

    public ConsoleUi(IManager mgr)
    {
        _manager = mgr;
    }
    
    private void PrintHeader(string header)
    {
        Console.WriteLine($"\n{header}\n{new String('=', header.Length)}");
    }

    private void PrintList(IEnumerable<object> objectList)
    {
        int index = 0;
        foreach (var obj in objectList)
        {
            Console.WriteLine($"{++index}. {obj}\n");
        }
        if (index == 0) Console.WriteLine("None Found\n");
    }
    
    private void ShowAllArticles()
    {
        PrintHeader("All articles");
  
        PrintList(_manager.GetAllArticles());
    }

    private void ShowArticlesPerCategory(int categoryChoice)
    {
        ArticleCategory selectedCategory = (ArticleCategory)categoryChoice;
        PrintHeader($"Articles on {selectedCategory.GetString()}");
        
        PrintList(_manager.GetArticlesByCategory(categoryChoice));
    }

    private void ShowCategories()
    {
        Console.WriteLine("\n");
        foreach (ArticleCategory category in Enum.GetValues<ArticleCategory>())
        {
            Console.WriteLine($"{(int) category} = {category}");
        }
        Console.Write("Choose category number: ");
        string categoryChoiceString = Console.ReadLine();
        int categoryChoice;
        while (!(Int32.TryParse(categoryChoiceString, out categoryChoice) && categoryChoice <= Enum.GetValues(typeof(ArticleCategory)).Cast<int>().Max()))
        {
            Console.Write("\nPlease enter a valid value.\nChoose category number: ");
            categoryChoiceString = Console.ReadLine();
        }
        ShowArticlesPerCategory(categoryChoice);
    }

    private void ShowAllScientists()
    {
        PrintHeader("All scientists");

        PrintList(_manager.GetAllScientists());
    }

    private void ShowFilteredScientists()
    {
        Console.Write("\nEnter (part of) a name or leave blank: ");
        string nameString = Console.ReadLine();
        Console.Write("Enter a full date (yyyy/mm/dd) or leave blank: ");
        string dobString = Console.ReadLine();
        
        PrintHeader("Scientists By Name / DoB");
        PrintList(_manager.GetScientistsByNameAndDateOfBirth(nameString, dobString));
    }
    
    private void ActionCreateArticle()
    {
        throw new NotImplementedException();
    }
    
    private void ActionCreateScientist()
    {
        throw new NotImplementedException();
    }
    
    private static void WaitToContinue(int inputChoice)
    {
        if (inputChoice == 0) return;
        Console.Write("\nPress ENTER to continue...");
        Console.ReadLine();
    }
    
    private void MainMenuAction(string inputChoice)
    {
        int choice;
        if (Int32.TryParse(inputChoice, out choice))
        {
            switch (choice)
            {
                case 0:
                    _quitConsoleApp = true;
                    break;
                case 1:
                    ShowAllArticles();
                    break;
                case 2:
                    ShowCategories();
                    break;
                case 3:
                    ShowAllScientists();
                    break;
                case 4:
                    ShowFilteredScientists();
                    break;
                case 5:
                    ActionCreateArticle();
                    break;
                case 6:
                    ActionCreateScientist();
                    break;
                default:
                    Console.WriteLine("\nPlease select a valid option.");
                    break;
            }
        }
        else
        {
            choice = Int16.MaxValue;
            Console.WriteLine("\nPlease enter a valid value.");
        }

        WaitToContinue(choice);
    }

    private void ShowMenu()
    {
        List<string> options =
        [
            "Quit", "Show all articles", "Show articles of category", "Show all scientists",
            "Show scientists with name and/or date of birth", "Add an article", "Add a scientist"
        ];
        PrintHeader("What would you like to do?");
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i}) {options[i]}");
        }
        Console.Write("Choice: ");
        string inputChoice = Console.ReadLine();
        MainMenuAction(inputChoice);
    }
    
    public void Run()
    {
        while (!_quitConsoleApp)
        {
            ShowMenu();
        }
    }
}