namespace IST4210_hw5.Models
{
    public class Login
    {
        
            private string _password = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Password { get { return _password; } set { _password = PasswordOneWayHash.GetHash(value); } }
            public string AuthenticationError { get; set; } = string.Empty;
        

    }
}
