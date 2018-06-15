using Novell.Directory.Ldap;

namespace HwInf.Common.LDAP
{
    public class LDAPUserParameters
    {
        public LDAPUserParameters()
        {
            IsAuthenticated = false;
        }

        public LDAPUserParameters(bool auth, string firstname, string lastname, string fullname, string email, string studiengang, string studiengangKuerzel, string personal)
        {
            IsAuthenticated = auth;
            Firstname = firstname;
            Lastname = lastname;
            Fullname = fullname;
            Mail = email;
            Studiengang = studiengang;
            StudiengangKuerzel = studiengangKuerzel;
            PersonalType = personal;
        }

        public bool IsAuthenticated { get; private set; }

        public string Mail { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Fullname { get; private set; }
        public string Studiengang { get; private set; }
        public string StudiengangKuerzel { get; private set; }
        public string PersonalType { get; private set; }
    }

    public class LDAPAuthenticator
    {
        const string ATTRIBUTES = "ou=People,dc=technikum-wien,dc=at";
        const string HOST = "ldap.technikum-wien.at";
        const int PORT = 389;

        private static bool? _disablePasswordCheck;
        public static bool DisablePasswordCheck
        {
            get
            {
                if (_disablePasswordCheck == null)
                {
                    // set DEV_ENVIRONMENT = 1, done by IISExpress
                    _disablePasswordCheck = System.Environment.GetEnvironmentVariable("DEV_ENVIRONMENT") == "1";
                    _disablePasswordCheck = false;
                }
                return _disablePasswordCheck.Value;
            }
        }

        private static bool? _startTLS;
        public static bool StartTLS
        {
            get
            {
                if (_startTLS == null)
                {
                    _startTLS = (System.Configuration.ConfigurationManager.AppSettings["ldap_start_tls"] as string ?? "true") == "true";
                    _startTLS = false;
                }
                return _startTLS.Value;
            }
        }

        private static LDAPUserParameters GetUserParameter(LdapConnection ldap, string username)
        {
            try
            {
                //user daten parameter lesen - frage: daten nur holen, wenn noch nicht registriert oder sollen die daten immer verwendet werden und u.U auch in sofi aktualisiert werden
                string[] attributesToReturn = new string[] { "displayName", "sn", "givenName", "cn", "mail", "ou", "gidNumber", "Uid" };

                string sMail = "";
                string sDisplayName = "";
                string sFirstName = "";
                string sLastName = "";
                string sStudiengang = "";
                string sStudiengangKuerzel = "";
                string sPersonalBezeichnung = "";

                var queue = ldap.Search(ATTRIBUTES, LdapConnection.SCOPE_SUB, string.Format("(Uid={0})", username), attributesToReturn, false);

                LdapEntry entry = queue.Next();
                if (entry != null)
                {
                    sMail = entry.getAttribute("mail").StringValue ?? string.Empty;
                    sLastName = entry.getAttribute("sn").StringValue ?? string.Empty;
                    sFirstName = entry.getAttribute("givenName").StringValue ?? string.Empty;
                    // sDisplayName = entry.getAttribute("displayName").StringValue ?? string.Format("{0} {1}", sFirstName, sLastName);
                    string iGidNumber = entry.getAttribute("gidNumber").StringValue ?? string.Empty; // 101=Technikum, 102=Student

                    sStudiengang = "";
                    sStudiengangKuerzel = "";
                    sPersonalBezeichnung = "";
                    var oOu = entry.getAttribute("ou");
                    if (iGidNumber == "101" || iGidNumber == "120") //tw-personal & FH Admin
                    {
                        sPersonalBezeichnung = "Teacher"; 
                    }
                    else if (oOu != null && oOu.size() > 0 && iGidNumber == "102") //student
                    {
                        sStudiengang = oOu.StringValueArray[1];
                        sStudiengangKuerzel = oOu.StringValueArray[2];
                    }
                }

                return new LDAPUserParameters(true, sFirstName, sLastName, sDisplayName, sMail, sStudiengang, sStudiengangKuerzel, sPersonalBezeichnung);
            }
            catch
            {
                return new LDAPUserParameters();
            }
        }

        public static LDAPUserParameters GetUserParameter(string username)
        {
            var ldap = Connect();
            try
            {
                ldap.Bind(null, null);
                return GetUserParameter(ldap, username);
            }
            finally
            {
                ldap.Disconnect();
            }
        }

        public static LDAPUserParameters Authenticate(string username, string password)
        {
            var ldap = Connect();
            try
            {
                if (DisablePasswordCheck == false)
                {
                    ldap.Bind("Uid=" + username + "," + ATTRIBUTES, password);
                }
                else
                {
                    ldap.Bind(null, null);
                }

                return GetUserParameter(ldap, username);
            }
            catch
            {
                return new LDAPUserParameters();
            }
            finally
            {
                ldap.Disconnect();
            }
        }

        private static LdapConnection Connect()
        {
            var ldap = new LdapConnection();
            ldap.Connect(HOST, PORT);
            if (StartTLS)
            {
                ldap.StartTls();
            }
            return ldap;
        }
    }
}
