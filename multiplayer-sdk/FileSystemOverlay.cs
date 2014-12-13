// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MIVSDK
{
    public class FileSystemOverlay
    {
        public static void prepareForMIV()
        {
            try
            {
                File.Copy(@"miv_datafiles\american.gxt.miv", @"common\text\american.gxt", true);
                File.Copy(@"miv_datafiles\script.img.miv", @"common\data\cdimages\script.img", true);
                string dpath = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Rockstar Games", "GTA IV", "savegames");
                string[] files = Directory.GetFiles(dpath);
                foreach (string file_full in files)
                {
                    string file = Path.GetFileName(file_full);
                    if (file.StartsWith("SGTA4"))
                    {
                        string newfile = Path.Combine("miv_datafiles", "savegames_dump", file);
                        if (File.Exists(newfile))
                        {
                            File.Delete(newfile);
                        }
                        File.Move(Path.Combine(dpath, file), newfile);
                    }
                }
                File.CreateText("_miv_prepared_for_miv.lock").Close();
            }
            catch (Exception e)
            {
                prepareForSP();
                System.Windows.Forms.MessageBox.Show(e.Message);
                throw e;
            }
        }

        public static void crashIfSPPreparationFail()
        {
            if (File.Exists("_miv_prepared_for_miv.lock"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        public static void prepareForSP()
        {
            try
            {
                File.Copy(@"miv_datafiles\american.gxt.sp", @"common\text\american.gxt", true);
                File.Copy(@"miv_datafiles\script.img.sp", @"common\data\cdimages\script.img", true);
                string dpath = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Rockstar Games", "GTA IV", "savegames");
                string[] files = Directory.GetFiles(Path.Combine("miv_datafiles", "savegames_dump"));
                foreach (string file_full in files)
                {
                    string file = Path.GetFileName(file_full);
                    if (file.StartsWith("SGTA4"))
                    {
                        string newfile = Path.Combine(dpath, file);
                        File.Copy(Path.Combine("miv_datafiles", "savegames_dump", file), newfile, true);
                    }
                }
                File.Delete("_miv_prepared_for_miv.lock");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                //prepareForSP();
                throw e;

            }
        }
    }
}
