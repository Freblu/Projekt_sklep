namespace PartsCom.Ui.Models;

#pragma warning disable CA1515
public sealed class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
