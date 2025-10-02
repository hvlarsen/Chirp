using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Razor.Data;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; } = new();
    public int CurrentPage { get; set; } = 1; 

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet([FromQuery] int page)
    {
        if (page == 0) page = 1; 

        CurrentPage = page;

        Cheeps = _service.GetCheeps(CurrentPage);
        return Page();
    }
}
