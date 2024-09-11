namespace MovieBringer.WebApp.Models
{
    public class CustomJsonModel
    {
        public string SuccessMessage { get; set; } = "Success";
        public string ErrorMessage { get; set; }= "Failed";
        public List<string> ErrorMessages { get; set; } = new(){ "Error Happened"};
        public bool IsValid { get; set; } = false;

        public object? Data { get; set; }
    }
}
