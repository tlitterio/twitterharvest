using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;

namespace TwitterAPISearch
{
    public class SparkSend
    {
        public XmppClientConnection sparkcon;
        public void sparkconnect()
        {
            try
            {
                sparkcon = new XmppClientConnection("chi-wavemngt.geneva-trading.com", 5222);
                sparkcon.Open("TraderAlerts", "P@$$word");
            }
            catch
            {
            }
            return;
        }
        public void sparksend(string IM, List<string> ListOfUsers)
        {
            foreach (string str in ListOfUsers)
            {
                string sendableAdress = str + "@chi-wavemngt.geneva-trading.com";
                sparkcon.Send(new Message(sendableAdress, MessageType.chat, IM));
            }
        }
        public List<string> getsparkusers()
        {
            int counter = 0;
            string line;
            List<string> ListOfCurrentUsers = new List<string>();
            StreamReader GetListOfCurrentUsers = new StreamReader("c:\\temp\\users.txt");
            while ((line = GetListOfCurrentUsers.ReadLine()) != null)
            {
                ListOfCurrentUsers.Add(line);
                counter++;
            }
            GetListOfCurrentUsers.Close();
            return ListOfCurrentUsers;
        }
    }
}

