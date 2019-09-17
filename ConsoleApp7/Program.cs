using System;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace WeatherTZ
{
    class Program
    {
        public static string path = @"output.txt";
        static void Main(string[] args)
        {
            
            Console.WriteLine("Please enter adress to the site");
            string uri;
            uri = Console.ReadLine();
            CreateUri(uri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream receiveStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF32);

            Console.WriteLine("The answer is recieved");

            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                fs.Close();
                StreamWriter sw = new StreamWriter(path, true, Encoding.Unicode);
                sw.Write(reader.ReadToEnd());
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            getHTML();
            response.Close();
            reader.Close();
            Console.ReadKey();
        }

        public static string CreateUri(string uri)
        {
            string adress = "https://";
            if (uri[0] == 'h' && uri[1] == 't' && uri[2] == 't' && uri[3] == 'p' && uri[4] == 's')
            {
                return uri;
            }
            else
            {
                uri = adress + uri;
            }
            return uri;
        }

        public static void getHTML()
        {
            StreamReader sr = new StreamReader(path);
            string doc = sr.ReadToEnd();
            sr.Close();
            StreamWriter sw = new StreamWriter(path);
            sw.Write(" ");
            MatchCollection allMatches = null;
            Regex regex = new Regex(@"<a.(?<groupname>.*?)<\/a>");
            allMatches = regex.Matches(doc);
            Console.WriteLine("Number of tags A = " + allMatches.Count);
            int i = 0;
            foreach(var match in allMatches)
            {
                ++i;
                string s = match.ToString();
                int indexS = s.IndexOf("href=\"");
                s = s.Remove(0, indexS + 6);
                int indexF = s.IndexOf("\"");
                sw.WriteLine(s.Substring(0, indexF - 1));
                Console.WriteLine("Запись в файл № " + i);
                Console.WriteLine(s.Substring(0, indexF - 1));
                if (i == 20)
                    break;
                }
            sw.Close();
        }
    }
}
