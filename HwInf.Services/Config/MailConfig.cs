namespace HwInf.Services.Config
{
    public class MailConfig
    {
        public MailConfig()
        {
            Current = this;
        }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public static MailConfig Current;
    }
}