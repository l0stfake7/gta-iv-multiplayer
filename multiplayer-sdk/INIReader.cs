using System.Collections.Generic;

namespace MIVSDK
{
    public class INIReader
    {
        private Dictionary<string, string> dict;

        public INIReader(string[] inilines)
        {
            dict = new Dictionary<string, string>();
            foreach (string line in inilines)
            {
                if (line.StartsWith("#")) continue;
                string[] splitted = line.Split('=');
                dict.Add(splitted[0].Trim(), splitted[1].Trim());
            }
        }

        public float getFloat(string key)
        {
            return float.Parse(getString(key));
        }

        public int getInt(string key)
        {
            return int.Parse(getString(key));
        }

        public string getString(string key)
        {
            return dict[key];
        }
    }
}