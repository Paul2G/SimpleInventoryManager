using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace ProyectoFinal
{
    public static class DataFiles
    {
        public static void toSerialize(List<Item> items)
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Item>));
            TextWriter w = new StreamWriter(@"data.xml");
            s.Serialize(w, items);
            w.Close();
        }

        public static List<Item> toDesearialize()
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Item>));
            List<Item> listarecovered = new List<Item>();

            TextReader r = new StreamReader(@"data.xml");
            listarecovered = (List<Item>)s.Deserialize(r);
            r.Close();
            return listarecovered;
        }
    }
}
