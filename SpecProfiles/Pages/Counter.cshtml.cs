using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace SpecProfiles.Pages;

public class CounterModel : PageModel
{
    private readonly ILogger<CounterModel> _logger;

    [BindProperty]
    public int CounterValue { get; set; }

	public CounterModel(ILogger<CounterModel> logger)
    {
        _logger = logger;
        this.CounterValue = 0;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

	public IActionResult OnPost(string? counter_value)
	{
		this.CounterValue = int.Parse(counter_value ?? "0") + 1;
		return Page();
	}
}

