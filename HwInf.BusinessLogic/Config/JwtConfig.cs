namespace HwInf.BusinessLogic.Config
{
    public class JwtConfig
    {
        public JwtConfig()
        {
            Current = this;
        }
        public string SecretKey { get; set; }
        public int HoursValid { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public static JwtConfig Current;
    }
}