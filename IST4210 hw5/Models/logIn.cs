namespace IST4210_hw5.Models
{
    public class logIn
    {
        private string _password = String.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { internal get { return _password; } set { _password = PasswordOneWayHash.GetHash(value);  } } 
        public string AuthenticationError { get; set; } = string.Empty;

    }
}
