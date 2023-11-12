namespace ChatApp.Infrastructure.Feature.Services.Email.Template
{
    public partial class RegistrationConfirmEmail
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public RegistrationConfirmEmail(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
