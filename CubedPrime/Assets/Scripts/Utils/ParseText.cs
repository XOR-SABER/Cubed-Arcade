using System;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

namespace Utils
{
    public class ParseText
    {
        private string Text;
        private string _folder;
        private string _filename;

        public ParseText(string filename, string folder)
        {
            _folder = folder;
            _filename = filename;
            Parse();
        }

        private void Parse()
        {
            try
            {
                string path = Application.dataPath + "/" +_folder + "/"+ _filename + ".txt";
                Text = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Debug.Log("Parse text failed" + e.ToString());
            }
        }

        public string GetText()
        {
            return Text;
        }
    }
}
