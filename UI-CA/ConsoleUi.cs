using System.Collections;
using System.ComponentModel.DataAnnotations;
using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.BL.Domain.Extensions;
using Castle.Components.DictionaryAdapter.Xml;

namespace ArticleManagement.UI.CA;

public class ConsoleUi
{
    private bool _quitConsoleApp = false;
    private readonly IManager _manager;

    public ConsoleUi(IManager mgr)
    {
        _manager = mgr;
    }
    
    private static void PrintHeader(string header)
    {
        Console.WriteLine($"\n{header}\n{new String('=', header.Length)}");
    }

    private static string SumArticleAuthors(ScientificArticle article)
    {   
        string authorNamesString = "";
        foreach (ArticleScientistLink articleScientistLink in article.AuthorLinks)
        {
            authorNamesString += articleScientistLink.Scientist.Name + ", ";
        }
        return authorNamesString.Trim().Trim(',');
    }
    private static void PrintArticlesList(IEnumerable<ScientificArticle> scientificArticles)
    {
        int index = 0;
        foreach (var article in scientificArticles)
        {
            index++;
            string authors = SumArticleAuthors(article);
            Console.WriteLine($"{article.Id}. {article.Title} ({article.NumberOfPages} {(article.NumberOfPages == 1 ? "page" : "pages")}){(authors == "" ? "" : $"; by {authors}")} [published in \"{(article.Journal?.JournalName ?? "UNKNOWN")}\" on {article.DateOfPublication}]\n");
        }
        if (index == 0) Console.WriteLine("None Found\n");
    }

    private static void PrintScientistsList(IEnumerable<Scientist> scientists)
    {
        int index = 0;
        foreach (var scientist in scientists)
        {
            index++;
            string articlesOverview = null;
            foreach (var articleLink in scientist.ArticleLinks)
            {
                articlesOverview += $"\t\t<Article> {articleLink.Article.Title}\n";
            }
            Console.WriteLine($"{scientist.Id}. {scientist.Name}, Faculty of {scientist.Faculty} at {scientist.University} {(scientist.DateOfBirth != null? $"(born {scientist.DateOfBirth})" : "" )}\n" +
                              $"\tHas contributed to:\n" +
                              $"{articlesOverview ?? "\t\tNo articles found in database.\n"}");
        }
        if (index == 0) Console.WriteLine("None Found\n");
    }
    
    private void ShowAllArticles()
    {
        PrintHeader("All articles");
  
        PrintArticlesList(_manager.GetAllArticlesWithAuthorsAndJournals());
    }

    private static void ShowCategories()
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
    
    private bool MatchScientistName(string nameString, Scientist scientist)
    {
        string[] scientistNameParts = nameString.Split(" ");

        foreach (var namePart in scientistNameParts)
        {
            bool isMatching = scientist.Name.ToLower().Contains(namePart.ToLower());
            if (!isMatching) return false;
        }

        return true;
    }

    
    private bool TryParseScientist(string scientistNameString, out Scientist existingScientist)
    {
        if (!(scientistNameString == null || scientistNameString.Trim() == ""))
        {
            foreach (Scientist scientist in _manager.GetAllScientists())
            {
                if (MatchScientistName(scientistNameString, scientist))
                {
                    existingScientist = scientist;
                    return true;
                }
            }
        }
        existingScientist = null;
        return false;
    }

    private static int InputCategoryChoice()
    {
        ShowCategories();
        Console.Write("Choose category number: ");
        string categoryChoiceString = Console.ReadLine();
        int categoryChoice;
        while (!Int32.TryParse(categoryChoiceString, out categoryChoice))
        {
            Console.Write("\nPlease enter a valid value.\nChoose category number: ");
            categoryChoiceString = Console.ReadLine();
        }

        return categoryChoice;
    }
    
    private static void VerifyCategoryChoice(int categoryChoice)
    {
        if (!Enum.IsDefined((ArticleCategory)categoryChoice))
        {
            throw new ArithmeticException("Invalid Category Number! Please try again.");
        }
    }

    private void ShowArticlesPerCategory()
    {
        int categoryChoice = InputCategoryChoice();

        try
        {
            VerifyCategoryChoice(categoryChoice);
        }
        catch (ArithmeticException exception)
        {
            Console.WriteLine(exception.Message);
            return;
        }
        
        ArticleCategory selectedCategory = (ArticleCategory)categoryChoice;
        PrintHeader($"Articles on {selectedCategory.GetString()}");
        
        PrintArticlesList(_manager.GetArticlesByCategoryWithAuthorsAndJournals(selectedCategory));
       
    }

    private void ShowAllScientists()
    {
        PrintHeader("All scientists");

        PrintScientistsList(_manager.GetAllScientistsWithArticles());
    }

    private void ShowFilteredScientists()
    {
        Console.Write("\nEnter (part of) a name or leave blank: ");
        string nameString = Console.ReadLine();
        Console.Write("Enter a full date (dd/mm/yyyy) or leave blank: ");
        string dobString = Console.ReadLine();
        bool isDateValid = DateOnly.TryParse(dobString, out DateOnly dateOfBirth);
        
        PrintHeader("Scientists By Name / DoB");
        PrintScientistsList(_manager.GetScientistsByNameAndDateOfBirth(nameString, isDateValid? dateOfBirth : null));
    }

    private static string InputScientistName(string name)
    {
        string scientistName = null;
        if (name != null)
        {
            scientistName = name;
        }
        else
        {
            Console.Write("Enter scientist name (enter \'exit\' to quit): ");
            scientistName = Console.ReadLine();
        }

        return scientistName;
    }

    private bool CheckScientistName(string scientistName, bool articleAuthorEntry = false)
    {
        if ((articleAuthorEntry && scientistName.Trim() == "") || scientistName.Trim().ToLower() == "exit")
        {
            if (!articleAuthorEntry) Console.WriteLine("--Operation Terminated--");
            return false;
        }

        if (articleAuthorEntry) return true;
        
        if (!TryParseScientist(scientistName, out Scientist existingScientist)) return true;
        Console.WriteLine($"{existingScientist.Name} already in the list!");
        return false;

    }
    

    private void ActionCreateScientist(string name = null)
    {
        string scientistName = InputScientistName(name);
        if (!CheckScientistName(scientistName)) return;
        
        Console.Write("Enter scientist university: ");
        string scientistUniversity = Console.ReadLine();
        Console.Write("Enter scientist faculty: ");
        string scientistFaculty = Console.ReadLine();
        Console.Write("Enter scientist date of birth [yyyy/mm/dd] (optional): ");
        string scientistDobString = Console.ReadLine();
        DateOnly? nullableDob = null;
        if (DateOnly.TryParse(scientistDobString, out DateOnly dateOfBirth)) nullableDob = dateOfBirth;
        
        try
        {
            Scientist scientist = _manager.AddScientist(scientistName, scientistFaculty, scientistUniversity, nullableDob);
            Console.WriteLine($"\nScientist added successfully: {scientist.Name}\n");
        }
        catch (ValidationException exception)
        {
            var errorMessages = exception.Message.Split("|");
            Console.WriteLine();
            foreach (var errorMessage in errorMessages)
            {
                Console.WriteLine(errorMessage);
            }
        }
    }

    private static string InputArticleTitle()
    {
        string articleTitle = null;
        Console.Write("Enter article title: ");
        articleTitle = Console.ReadLine();

        return articleTitle;
    }

    private List<Scientist> InputArticleAuthors()
    {
        List<Scientist> articleAuthors = [];
        Console.WriteLine("Initiating authors list entry. Please enter the names one by one.");
        while (true)
        {
            string authorNameString = null;
            Console.Write("Enter author name (leave blank for next step): ");
            authorNameString = Console.ReadLine();
            bool isInvalidName = !CheckScientistName(authorNameString, true);
            if (isInvalidName) return articleAuthors;
            bool scientistExists = TryParseScientist(authorNameString, out Scientist scientist);
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
                if (TryParseScientist(authorNameString, out Scientist newScientist)) articleAuthors.Add(newScientist);
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

    private static int InputArticleNumberOfPages()
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
    
    private void ActionCreateArticle()
    {
        Console.WriteLine();
        string title = InputArticleTitle();
        IEnumerable<Scientist> authors = InputArticleAuthors();
        DateOnly dateOfPublication = InputArticleDateOfPublication();
        int numberOfPages = InputArticleNumberOfPages();
        int category = InputCategoryChoice();
        try
        {
            ScientificArticle article = _manager.AddArticle(title, authors, dateOfPublication, numberOfPages, (ArticleCategory) category);
            Console.WriteLine($"\nArticle added successfully: {article.Title}");
        }
        catch (ValidationException exception)
        {
            var errorMessages = exception.Message.Split("|");
            foreach (var errorMessage in errorMessages)
            {
                Console.WriteLine(errorMessage);
            }
        }
    }

    private void PrintScientistsWitId(IEnumerable<Scientist> scientists)
    {
        foreach (var scientist in scientists)
        {
            Console.WriteLine($"[{scientist.Id}] {scientist.Name}");
        }
        Console.Write("Please enter a scientist ID: ");
    }
    
    private void PrintArticlesWitId(IEnumerable<ScientificArticle> articles)
    {
        foreach (var article in articles)
        {
            Console.WriteLine($"[{article.Id}] {article.Title}");
        }
        Console.Write("Please enter an article ID: ");
    }

    private bool CheckIdString(string idString, out int selectedId)
    {
        if (!Int32.TryParse(idString, out int convertedId))
        {
            Console.WriteLine("\nPlease enter a valid value.");
            selectedId = convertedId;
            return false;
        }

        selectedId = convertedId;
        return true;
    }
    
    private void ShowScientistSelectionMenu(string chooseScientistString, out int scientistId)
    {
        Console.WriteLine(chooseScientistString);
        PrintScientistsWitId(_manager.GetAllScientists());
        string scientistChoice = Console.ReadLine();
        bool isValidScientistIdFormat = CheckIdString(scientistChoice, out int selectedScientistId);
        scientistId = isValidScientistIdFormat? selectedScientistId : Int32.MaxValue;
    }

    private void ShowArticleScientistLinkEditMenu(string chooseScientistString, string chooseArticleString, out int scientistId, out int articleId, out bool isLeadResearcher)
    {
        ShowScientistSelectionMenu(chooseScientistString, out int selectedScientistId);
        scientistId = selectedScientistId;
        
        Console.WriteLine("\n" + chooseArticleString);
        PrintArticlesWitId(_manager.GetAllArticles());
        string articleChoice = Console.ReadLine();
        
        Console.Write("Is this scientist the lead researcher? Enter Y/N: ");
        string leadResearcherReply = Console.ReadLine();
        isLeadResearcher = leadResearcherReply?.ToLower() == "y";
        
        bool isValidArticleIdFormat = CheckIdString(articleChoice, out int selectedArticleId);
        articleId = isValidArticleIdFormat? selectedArticleId : Int32.MaxValue;
    }
    
    private void ActionCreateArticleScientistLink()
    {
        PrintHeader("Adding article to scientist");
       ShowArticleScientistLinkEditMenu("Which scientist would you like to add an article to?", "Which article would you like to assign to this scientist?", 
           out int selectedScientistId, out int selectedArticleId, out bool isLeadResearcher);
        // TODO: add validation
        try
        {
            ArticleScientistLink articleScientistLink = _manager.AddArticleScientistLink(selectedArticleId, selectedScientistId, isLeadResearcher);
            Console.WriteLine("\nArticle-Scientist link created successfully!\n");
        } catch (ValidationException exception)
        {
            Console.WriteLine("\n" + exception.Message + "\n");
        }
    }
    
    private void ActionDeleteArticleScientistLink()
    {
        PrintHeader("Removing article from scientist");
        ShowScientistSelectionMenu("Which scientist would you like to remove an article from?", out int selectedScientistId);
        Console.WriteLine("\n" + "Which article would you like to remove from this scientist?");
        PrintArticlesWitId(_manager.GetArticlesOfScientist(selectedScientistId));
        string articleChoice = Console.ReadLine();
        bool isValidArticleIdFormat = CheckIdString(articleChoice, out int selectedArticleId);
        if (!isValidArticleIdFormat) return;
        
        try
        {
            _manager.RemoveArticleScientistLink(selectedArticleId, selectedScientistId);
            Console.WriteLine("\nArticle removed successfully!\n");
        } catch (ValidationException exception)
        {
            Console.WriteLine("\n" + exception.Message + "\n");
        }
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
                case 7:
                    ActionCreateArticleScientistLink();
                    break;
                case 8:
                    ActionDeleteArticleScientistLink();
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
            "Show scientists with name and/or date of birth", "Add an article", "Add a scientist",
            "Add article to scientist", "Remove article from scientist"
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