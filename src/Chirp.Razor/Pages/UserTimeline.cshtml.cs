using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Application.Interfaces;
using Chirp.Application.DTOs;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;

    public List<CheepDto> Cheeps { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public string Author { get; set; } = string.Empty;

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGetAsync(string author, [FromQuery] int page = 1)
    {
        CurrentPage = page <= 0 ? 1 : page;
        Author = author;

        Cheeps = await _service.GetPrivateTimelineAsync(author, CurrentPage);

        return Page();
    }
}