using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Soaptest.ServiceReference1;

namespace Soaptest
{   
    class Student
    {
        public string vorname, nachname, studiengang_kz, semester, verband, gruppe, uid, status, personenkennzeichen, email, person_id;

        public Student(string vorname, string nachname, string studiengang_kz, string semester, string verband, string gruppe, string uid, string status, string personenkennzeichen, string email, string person_id)
        {
            this.vorname = vorname;
            this.nachname = nachname;
            this.studiengang_kz = studiengang_kz;
            this.semester = semester;
            this.verband = verband;
            this.gruppe = gruppe;
            this.uid = uid;
            this.status = status;
            this.personenkennzeichen = personenkennzeichen;
            this.email = email;
            this.person_id = person_id;
        }

        public void Debug()
        {
            Console.WriteLine("vorname:"+this.vorname);
            Console.WriteLine("nachname:"+this.nachname);
            Console.WriteLine("studiengang_kz:"+this.studiengang_kz);
            Console.WriteLine("semester:"+this.semester);
            Console.WriteLine("verbande:"+this.verband);
            Console.WriteLine("gruppe:"+this.gruppe);
            Console.WriteLine("uid:"+this.uid);
            Console.WriteLine("status:"+this.status);
            Console.WriteLine("personenkennzeichen:"+this.personenkennzeichen);
            Console.WriteLine("email:"+this.email);
            Console.WriteLine("person_id:"+this.person_id);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            //Credentials for Authentification 
            /*
             * WICHTIG 
             * WICHTIG
             * 
             * USERNAME UND PASSWORT SIND DER BERECHTIGTE USER FUER SOAP!!!!
             * 
             * MAIL AN oesi@technikum-wien.at UM USER ZU BERECHTIGEN
             */
           
            string username = "USERNAME";                             //   <---------------------------------------   USERNAME
            string password = "PASSWORT";                             //   <---------------------------------------   PASSWORD

            /*
             * StudentFromUid
             */
            Student student = StudentFromUid(username, password,"if15b001");
            student.Debug();

            /*
             * StudentsFromStudiengang
             */
            List<Student> studg= StudentsFromStudiengang(username, password,"257"); //sollte List<Student> returnen
            foreach(Student stud in studg)
            {
                stud.Debug();
            }

            /*
             * StudentFromMatrikelnummer
             */
            Student studentFromMat = StudentFromMatrikelnummer(username, password, "1510257001");
            studentFromMat.Debug();
            
            Console.ReadKey();
        }

        
        private static Student StudentFromUid(string username, string password, string searched_uid)
        {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994");

            
            string usernamePassword = username + ":" + password;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994"), "Basic", new NetworkCredential(username, password));
            myReq.Credentials = mycache;
            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

            //Rest of Http Header
            myReq.Method = "POST";
            myReq.ContentType = "text/xml";
            myReq.Timeout = 1000;
            myReq.Headers.Add("SOAPAction", ":\"getStudentFromUid\"");

            //xmlDocument = Inhalt der XML-Request
            string xmlDocument = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><q1:getStudentFromUid xmlns:q1=\"http://technikum-wien.at\"><student_uid xsi:type=\"xsd:string\">"+searched_uid+"</student_uid><authentifizierung href=\"#id1\"/></q1:getStudentFromUid><q2:GetAuthentifizierung id=\"id1\" xsi:type=\"q2:GetAuthentifizierung\" xmlns:q2=\"http://technikum-wien.at\"><username xsi:type=\"xsd:string\">"+username+"</username><passwort xsi:type=\"xsd:string\">"+password+"</passwort></q2:GetAuthentifizierung></s:Body></s:Envelope>";

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
            string vorname, nachname, studiengang_kz, semester, verband, gruppe, uid, status, personenkennzeichen, email, person_id;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                

                string responseText = reader.ReadToEnd();
                XDocument document = XDocument.Load(new StringReader(responseText));
                
                //vorname
                vorname= document.Descendants("vorname").SingleOrDefault().Value;
                //nachname
                nachname=document.Descendants("nachname").SingleOrDefault().Value;
                //studiengang_kz
                studiengang_kz=document.Descendants("studiengang_kz").SingleOrDefault().Value;
                //semester
                semester=document.Descendants("semester").SingleOrDefault().Value;
                //verband
                verband=document.Descendants("verband").SingleOrDefault().Value;
                //gruppe
                gruppe=document.Descendants("gruppe").SingleOrDefault().Value;
                //uid
                uid=document.Descendants("uid").SingleOrDefault().Value;
                //status
                status=document.Descendants("status").SingleOrDefault().Value;
                //personenkennzeichen
                personenkennzeichen=document.Descendants("personenkennzeichen").SingleOrDefault().Value;
                //email
                email=document.Descendants("email").SingleOrDefault().Value;     
                //person_id
                person_id=document.Descendants("person_id").SingleOrDefault().Value;

            }
            Student result = new Student(vorname, nachname, studiengang_kz, semester, verband, gruppe, uid, status, personenkennzeichen, email, person_id);
            return result;
        }

        private static Student StudentFromMatrikelnummer(string username, string password, string searched_mat)
        {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994");


            string usernamePassword = username + ":" + password;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994"), "Basic", new NetworkCredential(username, password));
            myReq.Credentials = mycache;
            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

            //Rest of Http Header
            myReq.Method = "POST";
            myReq.ContentType = "text/xml";
            myReq.Timeout = 1000;
            myReq.Headers.Add("SOAPAction", ":\"getStudentFromMatrikelnummer\"");

            //xmlDocument = Inhalt der XML-Request
            string xmlDocument = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><q1:getStudentFromMatrikelnummer xmlns:q1=\"http://technikum-wien.at\"><student_uid xsi:type=\"xsd:string\">" + searched_mat + "</student_uid><authentifizierung href=\"#id1\"/></q1:getStudentFromMatrikelnummer><q2:GetAuthentifizierung id=\"id1\" xsi:type=\"q2:GetAuthentifizierung\" xmlns:q2=\"http://technikum-wien.at\"><username xsi:type=\"xsd:string\">" + username + "</username><passwort xsi:type=\"xsd:string\">" + password + "</passwort></q2:GetAuthentifizierung></s:Body></s:Envelope>";
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
            string vorname, nachname, studiengang_kz, semester, verband, gruppe, uid, status, personenkennzeichen, email, person_id;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {


                string responseText = reader.ReadToEnd();
                XDocument document = XDocument.Load(new StringReader(responseText));

                //vorname
                vorname = document.Descendants("vorname").SingleOrDefault().Value;
                //nachname
                nachname = document.Descendants("nachname").SingleOrDefault().Value;
                //studiengang_kz
                studiengang_kz = document.Descendants("studiengang_kz").SingleOrDefault().Value;
                //semester
                semester = document.Descendants("semester").SingleOrDefault().Value;
                //verband
                verband = document.Descendants("verband").SingleOrDefault().Value;
                //gruppe
                gruppe = document.Descendants("gruppe").SingleOrDefault().Value;
                //uid
                uid = document.Descendants("uid").SingleOrDefault().Value;
                //status
                status = document.Descendants("status").SingleOrDefault().Value;
                //personenkennzeichen
                personenkennzeichen = document.Descendants("personenkennzeichen").SingleOrDefault().Value;
                //email
                email = document.Descendants("email").SingleOrDefault().Value;
                //person_id
                person_id = document.Descendants("person_id").SingleOrDefault().Value;

            }
            Student result = new Student(vorname, nachname, studiengang_kz, semester, verband, gruppe, uid, status, personenkennzeichen, email, person_id);
            return result;
        }

        private static List<Student> StudentsFromStudiengang(string username, string password, string searched_studiengang, string searched_semester="", string searched_verband="", string searched_gruppe="")
        {
            List<Student> result = new List<Student>();
            int index = 0;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994");


            string usernamePassword = username + ":" + password;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri("https://cis.technikum-wien.at/soap/student.soap.php?0.12187700 1480588994"), "Basic", new NetworkCredential(username, password));
            myReq.Credentials = mycache;
            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

            //Rest of Http Header
            myReq.Method = "POST";
            myReq.ContentType = "text/xml";
            myReq.Timeout = 100000;
            myReq.Headers.Add("SOAPAction", ":\"getStudentFromStudiengang\"");

            //xmlDocument = Inhalt der XML-Request
            string xmlDocument = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><q1:getStudentFromStudiengang xmlns:q1=\"http://technikum-wien.at\"><studiengang xsi:type=\"xsd:string\">"+searched_studiengang+"</studiengang><semester xsi:type=\"xsd:string\">"+searched_semester+"</semester><verband xsi:type=\"xsd:string\">"+searched_verband+"</verband><gruppe xsi:type=\"xsd:string\">"+searched_gruppe+"</gruppe><authentifizierung href=\"#id1\"/></q1:getStudentFromStudiengang><q2:GetAuthentifizierung id=\"id1\" xsi:type=\"q2:GetAuthentifizierung\" xmlns:q2=\"http://technikum-wien.at\"><username xsi:type=\"xsd:string\">"+username+"</username><passwort xsi:type=\"xsd:string\">"+password+"</passwort></q2:GetAuthentifizierung></s:Body></s:Envelope>";

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
            string vorname, nachname, studiengang_kz, semester, verband, gruppe, uid, status, personenkennzeichen, email, person_id;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {


                string responseText = reader.ReadToEnd();
                XDocument document = XDocument.Load(new StringReader(responseText));

                foreach (var item in document.Descendants("item"))
                {
                    //vorname
                    vorname = document.Descendants("vorname").ElementAt(index).Value;
                    //nachname
                    nachname = document.Descendants("nachname").ElementAt(index).Value;
                    //studiengang_kz
                    studiengang_kz = document.Descendants("studiengang_kz").ElementAt(index).Value;
                    //semester
                    semester = document.Descendants("semester").ElementAt(index).Value;
                    //verband
                    verband = document.Descendants("verband").ElementAt(index).Value;
                    //gruppe
                    gruppe = document.Descendants("gruppe").ElementAt(index).Value;
                    //uid
                    uid = document.Descendants("uid").ElementAt(index).Value;
                    //status
                    status = document.Descendants("status").ElementAt(index).Value;
                    //personenkennzeichen
                    personenkennzeichen = document.Descendants("personenkennzeichen").ElementAt(index).Value;
                    //email
                    email = document.Descendants("email").ElementAt(index).Value;
                    //person_id
                    person_id = document.Descendants("person_id").ElementAt(index).Value;

                    result.Insert(index, new Student(vorname, nachname, studiengang_kz, semester, verband, gruppe, uid, status, personenkennzeichen, email, person_id));
                    index++;
                }

                


                Console.WriteLine(responseText);

            }
            return result;
        }



    }
}
