// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
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

        public DateTime getDateTime(string key)
        {
            return DateTime.Parse(getString(key));
        }

        public float getFloat(string key)
        {
            return float.Parse(getString(key));
        }

        public Int32 getInt(string key)
        {
            return Int32.Parse(getString(key));
        }

        public Int16 getInt16(string key)
        {
            return Int16.Parse(getString(key));
        }

        public Int32 getInt32(string key)
        {
            return Int32.Parse(getString(key));
        }

        public Int64 getInt64(string key)
        {
            return Int64.Parse(getString(key));
        }

        public string getString(string key)
        {
            return dict[key];
        }

        public UInt16 getUInt16(string key)
        {
            return UInt16.Parse(getString(key));
        }

        public UInt32 getUInt32(string key)
        {
            return UInt32.Parse(getString(key));
        }

        public UInt64 getUInt64(string key)
        {
            return UInt64.Parse(getString(key));
        }
    }
}