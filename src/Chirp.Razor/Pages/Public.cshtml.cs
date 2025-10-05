using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Application.Interfaces;
using Chirp.Application.DTOs;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;

    public List<CheepDto> Cheeps { get; set; } = new();
    public int CurrentPage { get; set; } = 1;

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        CurrentPage = page <= 0 ? 1 : page;
        Cheeps = await _service.GetPublicTimelineAsync(CurrentPage);
        return Page();
    }
}