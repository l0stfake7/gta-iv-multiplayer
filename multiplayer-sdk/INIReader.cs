using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVSDK
{
    public class INIReader
    {
        Dictionary<string, string> dict;
        public INIReader(string[] inilines)
        {
            dict = new Dictionary<string,string>();
            foreach (string line in inilines)
            {
                if (line.StartsWith("#")) continue;
                string[] splitted = line.Split('=');
                dict.Add(splitted[0].Trim(), splitted[1].Trim());
            }
        }

        public string getString(string key)
        {
            return dict[key];
        }
        public int getInt(string key)
        {
            return int.Parse(getString(key));
        }
        public float getFloat(string key)
        {
            return float.Parse(getString(key));
        }
    }
}
