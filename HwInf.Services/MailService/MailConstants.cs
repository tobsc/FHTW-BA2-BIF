namespace HwInf.Services.MailService
{
    public static class MailConstants
    {
        public static string FromMailAddress = "noreply@hw-inf.technikum-wien.at";
        public static string Subject = "HwInf";
        public static string ContactMessage = "<br/> Du kannst den Verwalter deiner Anfrage unter folgender Mail kontaktieren: {0}<br/>";
        public static string AcceptedMessage = "{0}: akzeptiert<br>";
        public static string DeclinedMessage = "{0}: abgelehnt<br>";
    }
}