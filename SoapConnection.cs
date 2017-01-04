using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Soaptest
{   

    class Program
    {
        static void Main(string[] args)
        { 
            Dictionary<string, string> student = new Dictionary<string, string>();
            student=StudentFromUid("if15b001"); //nach wird gesucht

            //alle moeglichen felder des Dictionaries
            Console.WriteLine("vorname="+student["vorname"]);
            Console.WriteLine("nachname="+student["nachname"]);
            Console.WriteLine("studiengang_kz="+student["studiengang_kz"]);
            Console.WriteLine("semester="+student["semester"]);
            Console.WriteLine("verband="+student["verband"]);
            Console.WriteLine("gruppe="+student["gruppe"]);
            Console.WriteLine("uid="+student["uid"]);
            Console.WriteLine("status="+student["status"]);
            Console.WriteLine("personenkennzeichen="+student["personenkennzeichen"]);
            Console.WriteLine("email="+student["email"]);
            Console.WriteLine("person_id="+student["person_id"]);


            Console.ReadKey();
        }

        
        private static Dictionary<string,string> StudentFromUid(string uid)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994");

            //Credentials for Authentification 
            /*
             * WICHTIG 
             * WICHTIG
             * 
             * USERNAME UND PASSWORT SIND DER BERECHTIGTE USER FUER SOAP!!!!
             * 
             * MAIL AN oesi@technikum-wien.at UM USER ZU BERECHTIGEN
             */

            string username = "USERIDOFAUTHORIZEDUSER";                             //   <---------------------------------------   USERNAME
            string password = "SUPERSECRETPASSWORD";                                //   <---------------------------------------   PASSWORD
            string usernamePassword = username + ":" + password;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994"), "Basic", new NetworkCredential(username, password));
            myReq.Credentials = mycache;
            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

            //Rest of Http Header
            myReq.Method = "POST";
            myReq.ContentType = "text/xml";
            myReq.Timeout = 30000;
            myReq.Headers.Add("SOAPAction", ":\"getStudentFromUid\"");

            //xmlDocument = Inhalt der XML-Request
            string xmlDocument = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><q1:getStudentFromUid xmlns:q1=\"http://technikum-wien.at\"><student_uid xsi:type=\"xsd:string\">"+uid+"</student_uid><authentifizierung href=\"#id1\"/></q1:getStudentFromUid><q2:GetAuthentifizierung id=\"id1\" xsi:type=\"q2:GetAuthentifizierung\" xmlns:q2=\"http://technikum-wien.at\"><username xsi:type=\"xsd:string\">USERIDOFAUTHORIZEDUSER</username><passwort xsi:type=\"xsd:string\">SUPERSECRETPASSWORD</passwort></q2:GetAuthentifizierung></s:Body></s:Envelope>";

            byte[] PostData = Encoding.UTF8.GetBytes(xmlDocument);
            myReq.ContentLength = PostData.Length;

            //Request
            using (Stream requestStream = myReq.GetRequestStream())
            {
                requestStream.Write(PostData, 0, PostData.Length);
            }

            //Response
            HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
            WebHeaderCollection header = response.Headers;

            var encoding = ASCIIEncoding.ASCII;

            //StreamReader for Response to have the response available as string
            XmlDocument document = new XmlDocument();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                
                string responseText = reader.ReadToEnd();
                document.LoadXml(responseText);
                XmlNodeList nodeList;
                
                //vorname
                 nodeList = document.GetElementsByTagName("vorname");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("vorname",node.InnerText);
                    
                }

                //nachname
                 nodeList = document.GetElementsByTagName("nachname");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("nachname", node.InnerText);

                }

                //studiengang_kz
                 nodeList = document.GetElementsByTagName("studiengang_kz");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("studiengang_kz", node.InnerText);
                    
                }

                //semester
                 nodeList = document.GetElementsByTagName("semester");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("semester", node.InnerText);
                    
                }

                //verband
                 nodeList = document.GetElementsByTagName("verband");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("verband", node.InnerText);
                   ;
                }

                //gruppe
                 nodeList = document.GetElementsByTagName("gruppe");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("gruppe", node.InnerText);
                   
                }

                //uid
                 nodeList = document.GetElementsByTagName("uid");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("uid", node.InnerText);
                    
                }

                //status
                 nodeList = document.GetElementsByTagName("status");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("status", node.InnerText);
                   
                }

                //personenkennzeichen
                 nodeList = document.GetElementsByTagName("personenkennzeichen");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("personenkennzeichen", node.InnerText);
                    
                }

                //email
                 nodeList = document.GetElementsByTagName("email");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("email", node.InnerText);
                    
                }

                //person_id
                 nodeList = document.GetElementsByTagName("person_id");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add("person_id", node.InnerText);
                    
                }
                
                //Falls benoetigt: zeigt den ganzen responsetext
                //Console.WriteLine(responseText);

            }

            return dic;
        }



    }
}
