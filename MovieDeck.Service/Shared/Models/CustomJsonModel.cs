namespace MovieDeck.Service.Shared.Models
{
    public class CustomJsonModel
    {
        public string? SuccessMessage { get; set; } 
        public string? ErrorMessage { get; set; } 
        public bool IsValid { get; set; }
        public object? Data { get; set; }
    }
}
