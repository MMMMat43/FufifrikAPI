namespace VKR.DTO
{
    public class LoginRequest
    {
        public string Phonenumber { get; set; } = null!;
        public string Passwordhash { get; set; } = null!;
    }
}
