using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using  Chirp.Razor.Data; // Only because it needs to know Cheep.cs

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly DBFacade _service;
    public List<Cheep> Cheeps { get; set; } = new();

    public UserTimelineModel(DBFacade service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        Cheeps = _service.GetCheepsByAuthor(author);
        return Page();
    }
}
