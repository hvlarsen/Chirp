using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Razor.Data;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly DBFacade _service;
    public List<Cheep> Cheeps { get; set; } = new();

    public PublicModel(DBFacade service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        Cheeps = _service.GetCheeps();
        return Page();
    }
}
