using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataContext
{
    public class DataLoader
    {
        //Get all files based on the file path and search term
        public List<string> GetAllFile(string filePath, string keyName)
        {
            List<string> fileslist = new List<string>();

            if (string.IsNullOrEmpty(keyName))
            {
                string[] filesData = System.IO.Directory.GetFiles(filePath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);
                foreach (var item in filesData)
                {
                   fileslist.Add(item);
                }
            }
            else
            {
                DateTime time1 = DateTime.Now;
                string currentDate = time1.Day + "-" + time1.Month + "-" + time1.Year;
                string searchDate1 = currentDate;
                string[] filesData = System.IO.Directory.GetFiles(filePath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);
                foreach (var item in filesData)
                {
                    if (item.Contains(keyName))
                    {
                        if (item.Contains(searchDate1))
                        {
                            fileslist.Add(item);
                        }
                    }
                }
            }
            return fileslist;
        }

        //move file name
        public int MoveFile(string downloadPath, string filePath, string keyName)
        {
            DateTime time = DateTime.Now;
            string currentdate = time.Day + "-" + time.Month + "-" + time.Year;
            string searchDate = currentdate;
            string[] files = System.IO.Directory.GetFiles(filePath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);
            int countFiles = 0;
            foreach (string item in files)
            {
                if (item.Contains(keyName))
                {
                    if (item.Contains(searchDate))
                    {
                        countFiles = countFiles + 1;
                    }
                }
            }
            currentdate = currentdate + "-" + keyName + "-" + countFiles.ToString();
            string fileName = filePath + currentdate + ".csv";
            File.Move(downloadPath, fileName);
            return countFiles;

        }

        public List<string> LoadDropDown(string filePath, string keyWord)
        {
            DateTime time = DateTime.Now;
            string currentDate = time.Day + "-" + time.Month + "-" + time.Year;
            string searchDate = currentDate;
            string[] files = System.IO.Directory.GetFiles(filePath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);
            List<string> fileslist = new List<string>();

            foreach (var item in files)
            {
                string name = item.Split('\\')[item.Split('\\').Length - 1].Split('.')[0].Substring(0, item.Split('\\')[item.Split('\\').Length - 1].Split('.')[0].Length - 2);
                if (!(fileslist.Contains(name)))
                {
                    fileslist.Add(name);
                }
            }
            return fileslist;
        }
    }
}
