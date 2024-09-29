namespace ConsoleApp;

public class ConsoleUi
{
    private bool _quitConsoleApp = false;
    private List<Scientist> Authors { get; set; }
    private List<ScientificArticle> Articles { get; set; }
    private List<ScienceJournal> Journals { get; set; }

    private void Seed()
    {
        Authors = new List<Scientist>();
        Journals = new List<ScienceJournal>();
        Articles = new List<ScientificArticle>();

        // Create Authors
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        Authors.Add(walterLewin);
        
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        Authors.Add(janVanParadijs);
        
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        Authors.Add(holgerPedersen);
        
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        Authors.Add(paulJoss);

        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        Authors.Add(charlesWarwick);

        Scientist anelaChoy = new Scientist("Anela Choy", "Ocenaography", "UC San Diego");
        Authors.Add(anelaChoy);
        
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));
        Authors.Add(robSherlock);


        // Create Journals
        ScienceJournal journalNature = new ScienceJournal("Nature");
        Journals.Add(journalNature);

        ScienceJournal journalScAdvances = new ScienceJournal("Science Advances", 15.00);
        Journals.Add(journalScAdvances);

        // Create Articles
        ScientificArticle articleOrbitalPeriodXRayBurster =
            new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53",
                new List<Scientist>(){ walterLewin, janVanParadijs, holgerPedersen},
                new DateOnly(1981, 12, 31), 3,
                ArticleCategory.Astrophysics, journalNature);
        Articles.Add(articleOrbitalPeriodXRayBurster);

        ScientificArticle articleXRayBurstSources =
            new ScientificArticle("X-ray burst sources", new List<Scientist>() { walterLewin, paulJoss },
                new DateOnly(1977, 11, 17), 6, ArticleCategory.Astrophysics, journalNature);
        Articles.Add(articleXRayBurstSources);
        
        ScientificArticle articleKappa =
            new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch", new List<Scientist>() { charlesWarwick },
                new DateOnly(2024, 09, 25), 18, ArticleCategory.Neuroscience, journalScAdvances);
        Articles.Add(articleKappa);

        ScientificArticle articleLarvaceans =
            new ScientificArticle(
                "From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea",
                new List<Scientist>() { robSherlock, anelaChoy }, new DateOnly(2017, 8, 16), 5,
                ArticleCategory.MarineEcology, journalScAdvances);
        Articles.Add(articleLarvaceans);
        
    }

    private void ShowAllArticles()
    {
        string header = "All articles";
        Console.WriteLine($"\n{header}\n{new String('=', header.Length)}");
        for (int i = 0; i < Articles.Count; i++)
        {
            Console.WriteLine($"{i+1}. {Articles[i]}\n");
        }
    }

    private void ShowArticlesPerCategory(int categoryChoice)
    {
        string header = $"Articles on {(ArticleCategory) categoryChoice}";
        Console.WriteLine($"\n{header}\n{new String('=', header.Length)}");
        int index = 1;
        foreach (ScientificArticle article in Articles)
        {
            if (article.Category == (ArticleCategory) categoryChoice)
            {
                Console.WriteLine($"{index}. {article}\n");
                index++;
            }
        }
    }

    private void ShowCategories()
    {
        Console.WriteLine("\n");
        for (int i = 0; i < Enum.GetNames(typeof(ArticleCategory)).Length; i++) {
            Console.WriteLine(i + " = " + (ArticleCategory) i);
        }
        Console.Write("Choose category number: ");
        string categoryChoiceString = Console.ReadLine();
        int categoryChoice;
        if (Int32.TryParse(categoryChoiceString, out categoryChoice))
        {
            ShowArticlesPerCategory(categoryChoice);
        }
    }

    private void ShowAllAuthors()
    {
        string header = "All authors";
        Console.WriteLine($"\n{header}\n{new String('=', header.Length)}");
        
        for (int i = 0; i < Authors.Count; i++)
        {
            Console.WriteLine($"{i+1}. {Authors[i]}\n");
        }
    }

    private void CheckNameFilter(string nameString, List<Scientist> filteredAuthorsList)
    {
        if (nameString != null)
        {
            if (nameString.Trim() != "")
            {
                foreach (Scientist author in Authors)
                {
                    if (author.Name.ToLower().Contains(nameString.ToLower()))
                    {
                        filteredAuthorsList.Add(author);
                    }
                }
            }
        }
    }

    private void CheckDobFilter(string dobString, List<Scientist> filteredAuthorsList)
    {
        if (DateOnly.TryParse(dobString, out DateOnly dateOfBirth))
        {
            foreach (Scientist author in Authors)
            {
                if (author.DateOfBirth == dateOfBirth)
                {
                    filteredAuthorsList.Add(author);
                }
            }
        }
    }

    private void ShowFilteredAuthors(List<Scientist> filteredAuthorsList)
    {
        string header = "Authors Found";
        Console.WriteLine($"\n{header}\n{new String('=', header.Length)}");

        if (filteredAuthorsList.Count == 0)
        {
            Console.WriteLine("None\n");
        }
        else
        {
            for (int i = 0; i < filteredAuthorsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {filteredAuthorsList[i]}\n");
            }
        }
    }
    
    private void FilterAuthors()
    {
        List<Scientist> filteredAuthorsList = new List<Scientist>();
        Console.WriteLine();
        Console.Write("Enter (part of) a name or leave blank: ");
        string nameString = Console.ReadLine();
        CheckNameFilter(nameString, filteredAuthorsList);
        
        Console.Write("Enter a full date (yyyy/mm/dd) or leave blank: ");
        string dobString = Console.ReadLine();
        CheckDobFilter(dobString, filteredAuthorsList);
        
        ShowFilteredAuthors(filteredAuthorsList);
    }
    
    private void MainMenuAction(int inputChoice)
    {
        switch (inputChoice)
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
               ShowAllAuthors();
               break;
            case 4:
                FilterAuthors();
                break;
        }
        Thread.Sleep(2000);
    }

    private void ShowMenu()
    {
        Console.Write("""
                      
                      What would you like to do?
                      ==========================
                      0) Quit
                      1) Show all articles
                      2) Show articles of category
                      3) Show all authors
                      4) Show authors with name and/or date of birth
                      """);
        Console.Write("\nChoice: ");
        string inputChoice = Console.ReadLine();
        int choice;
        if (Int32.TryParse(inputChoice, out choice))
        {
            MainMenuAction(choice);
        }
    }
    
    public void Run()
    {
        Seed();
        
        while (!_quitConsoleApp)
        {
            ShowMenu();
        }
        
    }
}