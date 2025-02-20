using Microsoft.AspNetCore.Identity;

namespace ArticleManagement.UI.Web;

public class IdentitySeeder
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public IdentitySeeder(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task AsyncSeed()
    {
        var defaultUser = new IdentityUser("defaultuser@kdg.be")
        {
            Email = "defaultuser@kdg.be"
        };
        await _userManager.CreateAsync(defaultUser, "DefaultUser123!");
        
        var bob = new IdentityUser("bob@kdg.be")
        {
            Email = "bob@kdg.be"
        };
        await _userManager.CreateAsync(bob, "Bob123!");
        
        var marley = new IdentityUser("marley@kdg.be")
        {
            Email = "marley@kdg.be"
        };
        await _userManager.CreateAsync(marley, "Marley123!");
        
        var ross = new IdentityUser("ross@kdg.be")
        {
            Email = "ross@kdg.be"
        };
        await _userManager.CreateAsync(ross, "Ross123!");
        
        var debouwer = new IdentityUser("debouwer@kdg.be")
        {
            Email = "debouwer@kdg.be"
        };
        await _userManager.CreateAsync(debouwer, "DeBouwer123!");
    }
}