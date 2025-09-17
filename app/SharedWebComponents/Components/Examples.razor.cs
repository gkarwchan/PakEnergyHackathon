

namespace SharedWebComponents.Components;

public sealed partial class Examples
{
    [Parameter, EditorRequired] public required string Message { get; set; }
    [Parameter, EditorRequired] public EventCallback<string> OnExampleClicked { get; set; }

    private string WhatIsIncluded { get; } = "What a vendor can get of benefits of automating the AP process?";
    private string WhatIsPerfReview { get; } = "What to expect from your purchaser?";
    private string WhatDoesPmDo { get; } = "How to submit an invoice?";

    private async Task OnClickedAsync(string exampleText)
    {
        if (OnExampleClicked.HasDelegate)
        {
            await OnExampleClicked.InvokeAsync(exampleText);
        }
    }
}
