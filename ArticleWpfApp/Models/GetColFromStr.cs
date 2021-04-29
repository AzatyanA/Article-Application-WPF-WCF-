using ArticleLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ArticleWpfApp.Models
{
    class GetColFromStr
    {
        public static List<Message> GetColFromString(string str)
        {
            List<Message> list = new List<Message>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            using (XmlReader reader = XmlReader.Create((new StringReader(xmlDoc.InnerXml))))
            {
                var serializer = new XmlSerializer(typeof(List<Message>));
                list = (List<Message>)serializer.Deserialize(reader);
            }
            return list;
        }
    }
}
