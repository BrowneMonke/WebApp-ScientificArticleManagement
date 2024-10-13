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

    private void ShowCategories()
    {
        Console.WriteLine();
        ArticleCategory[] categories = Enum.GetValues<ArticleCategory>();
        foreach (ArticleCategory category in categories)
        {
            Console.Write($"{(int) category} = {category}");
            if ((int) category == categories.Length - 1) continue;
            Console.Write(", ");
        }
        Console.WriteLine();
    }

    private int InputCategoryChoice()
    {
        Console.Write("Choose category number: ");
        string categoryChoiceString = Console.ReadLine();
        int categoryChoice;
        while (!(Int32.TryParse(categoryChoiceString, out categoryChoice) && categoryChoice <= Enum.GetValues(typeof(ArticleCategory)).Cast<int>().Max()))
        {
            Console.Write("\nPlease enter a valid value.\nChoose category number: ");
            categoryChoiceString = Console.ReadLine();
        }

        return categoryChoice;
    }

    private void ShowArticlesPerCategory()
    {
        ShowCategories();
        int categoryChoice = InputCategoryChoice();
        
        ArticleCategory selectedCategory = (ArticleCategory)categoryChoice;
        PrintHeader($"Articles on {selectedCategory.GetString()}");
        
        PrintList(_manager.GetArticlesByCategory(selectedCategory));
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

    private string InputScientistName(string name)
    {
        string scientistName = null;
        if (name != null)
        {
            scientistName = name;
        }
        else
        {
            Console.Write("Enter scientist name (leave blank to exit): ");
            scientistName = Console.ReadLine();
        }

        return scientistName;
    }

    private bool CheckScientistName(string scientistName, bool articleAuthorEntry = false)
    {
        if (scientistName == null || scientistName.Trim() == "")
        {
            if (!articleAuthorEntry) Console.WriteLine("--Operation Terminated--");
            return true;
        }

        if (!articleAuthorEntry) {
            if (!_manager.TryParseScientist(scientistName, out Scientist existingScientist)) return false;

            Console.WriteLine($"{existingScientist.Name} already in the list!");
            return true;
        }

        return false;
    }

    private void ActionCreateScientist(string name = null)
    {
        string scientistName = InputScientistName(name);
        if (CheckScientistName(scientistName)) return;
        
        Console.Write("Enter scientist university: ");
        string scientistUniversity = Console.ReadLine();
        Console.Write("Enter scientist faculty: ");
        string scientistFaculty = Console.ReadLine();
        Console.Write("Enter scientist date of birth [yyyy/mm/dd] (optional): ");
        string scientistDobString = Console.ReadLine();

        DateOnly? nullableDob = null;
        if (DateOnly.TryParse(scientistDobString, out DateOnly dateOfBirth)) nullableDob = dateOfBirth; 
        _manager.AddScientist(scientistName, scientistFaculty, scientistUniversity, nullableDob);
        Console.WriteLine("\nScientist added successfully.\n");
    }

    private bool InputArticleTitle(out string title)
    {
        string articleTitle = null;
        Console.Write("Enter article title (leave blank to exit): ");
        articleTitle = Console.ReadLine();
        if (articleTitle == null || articleTitle.Trim() == "")
        {
            Console.WriteLine("--Operation Terminated--");
            title = null;
            return false;
        }

        title = articleTitle;
        return true;
    }

    private List<Scientist> InputArticleAuthors()
    {
        List<Scientist> articleAuthors = [];
        Console.WriteLine("Initiating authors list entry. Please enter the names one by one.");
        while (true)
        {
            string authorNameString = null;
            Console.Write("Enter author name (Leave blank to exit): ");
            authorNameString = Console.ReadLine();
            bool isInvalidName = CheckScientistName(authorNameString, true);
            if (isInvalidName) return articleAuthors;
            bool scientistExists = _manager.TryParseScientist(authorNameString, out Scientist scientist);
            if (scientistExists)
            {
                articleAuthors.Add(scientist);
            }
            else
            {
                Console.Write("New name detected. Create new scientist entry?\nEnter \'y\' to proceed or leave blank to cancel entry: " );
                string choice = Console.ReadLine();
                if (choice == null || choice.ToLower() != "y") continue;
                ActionCreateScientist(authorNameString);
                _manager.TryParseScientist(authorNameString, out Scientist newScientist);
                articleAuthors.Add(newScientist);
            }
        }
    }

    private DateOnly InputArticleDateOfPublication()
    {
        string dateOfPubString;
        DateOnly dateOfPublication;
        do
        {
            Console.Write("Enter the date of publication (yyyy/mm/dd): ");
            dateOfPubString = Console.ReadLine();
        } while (!DateOnly.TryParse(dateOfPubString, out dateOfPublication));

        return dateOfPublication;
    }

    private int InputArticleNumberOfPages()
    {
        string numOfPagesString;
        int numberOfPages;
        do
        {
            Console.Write("Enter the number of pages: ");
            numOfPagesString = Console.ReadLine();
        } while (!Int32.TryParse(numOfPagesString, out numberOfPages));

        return numberOfPages;
    }

    private ArticleCategory InputArticleCategory()
    {
        ShowCategories();
        int categoryNumber = InputCategoryChoice();
        return (ArticleCategory)categoryNumber;
    }
    
    private void ActionCreateArticle()
    {
        Console.WriteLine();
        if(!InputArticleTitle(out string title)) return;
        IEnumerable<Scientist> authors = InputArticleAuthors();
        DateOnly dateOfPublication = InputArticleDateOfPublication();
        int numberOfPages = InputArticleNumberOfPages();
        ArticleCategory category = InputArticleCategory();
        _manager.AddArticle(title, authors, dateOfPublication, numberOfPages, category);
        Console.WriteLine("\nArticle added successfully.");
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
                    ShowArticlesPerCategory();
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