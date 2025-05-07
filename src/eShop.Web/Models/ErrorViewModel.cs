namespace eShop.Web.Models;

public class ErrorViewModel
{
    public string Title { get; set; } = "Error";
    public string Message { get; set; } = "An error has occurred.";
    public string? RequestId { get; set; }
    public int StatusCode { get; set; } = 500;
    
    // Only populated in development mode
    public string? DeveloperMessage { get; set; }
    
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}