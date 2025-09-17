

namespace SharedWebComponents.Pages;

public sealed partial class Index : IDisposable
{
    private int _currentPrompt = 0;

    private readonly CancellationTokenSource _cancellation = new();
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(30));
    private readonly string[] _prompts = new[]
    {
        "Select Purchaser",
        "Sending Invoice",
        "How to change company logo",
        "Invoice Sent",
        "Rejected Invoice"
    };
    private readonly Queue<Image> _images = new();

    [Inject]
    public required ApiClient Client { get; set; }

    protected override void OnInitialized()
    {
        _images.Enqueue(new("_content/SharedWebComponents/media/select-purchaser.png", "Select Purchaser"));
        _images.Enqueue(new("_content/SharedWebComponents/media/ar-invoice-sent.png", "Sending Invoice"));
        _images.Enqueue(new("_content/SharedWebComponents/media/company-logo.png", "Change Company Logo"));
        _images.Enqueue(new("_content/SharedWebComponents/media/image-ar-invoice-sent.png", "Invoice Sent"));
        _images.Enqueue(new("_content/SharedWebComponents/media/rejected-invoice.png", "Rejected Invoice"));

        _ = UpdateImageAsync();
    }

    public void Dispose()
    {
        _cancellation.Cancel();
        _cancellation.Dispose();
        _timer.Dispose();
    }

    private async Task UpdateImageAsync()
    {
        do
        {
            var prompt = _prompts[_currentPrompt++ % _prompts.Length];
            var images = await Client.RequestImageAsync(new PromptRequest { Prompt = prompt });
            if (images is { ImageUrls.Count: > 0 })
            {
                foreach (var image in images.ImageUrls)
                {
                    _ = _images.Dequeue();
                    _images.Enqueue(new Image(image.ToString(), prompt));
                }

                await InvokeAsync(StateHasChanged);
            }
        }
        while (await _timer.WaitForNextTickAsync(_cancellation.Token));
    }
}

internal readonly record struct Image(string Src, string Alt);
