using Microsoft.AspNetCore.Identity;

namespace ArticleManagement.UI.Web;

public class IdentitySeeder
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public IdentitySeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task AsyncSeed()
    {
        var adminRole = new IdentityRole("Admin");
        await _roleManager.CreateAsync(adminRole);
        
        var userRole = new IdentityRole("User");
        await _roleManager.CreateAsync(userRole);
        
        var defaultUser = new IdentityUser("default.user@kdg.be")
        {
            Email = "default.user@kdg.be"
        };
        await _userManager.CreateAsync(defaultUser, "DefaultUser123!");
        await _userManager.AddToRoleAsync(defaultUser, "User");
        
        var bob = new IdentityUser("bob@kdg.be")
        {
            Email = "bob@kdg.be"
        };
        await _userManager.CreateAsync(bob, "Bob123!");
        await _userManager.AddToRoleAsync(bob, "User");
        
        var marley = new IdentityUser("marley@kdg.be")
        {
            Email = "marley@kdg.be"
        };
        await _userManager.CreateAsync(marley, "Marley123!");
        await _userManager.AddToRoleAsync(marley, "User");
        
        var ross = new IdentityUser("ross@kdg.be")
        {
            Email = "ross@kdg.be"
        };
        await _userManager.CreateAsync(ross, "Ross123!");
        await _userManager.AddToRoleAsync(ross, "User");
        
        var deBouwer = new IdentityUser("de.bouwer@kdg.be")
        {
            Email = "de.bouwer@kdg.be"
        };
        await _userManager.CreateAsync(deBouwer, "DeBouwer123!");
        await _userManager.AddToRoleAsync(deBouwer, "Admin");
    }
}