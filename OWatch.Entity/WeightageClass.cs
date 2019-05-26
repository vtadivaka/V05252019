using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class WeightageClass
    {
        public List<Weightage> WeightageData(string path)
        {
            string csvData = File.ReadAllText(path);
            string[] data1 = csvData.Split('@');
            List<Weightage> objData = new List<Weightage>();
            foreach (var item in data1)
            {
                if (!string.IsNullOrEmpty(item.Trim()))
                {
                    string[] data2 = item.Split('#');
                    Weightage obj = new Weightage();
                    obj.Name = data2[0].Trim();
                    obj.Value = data2[1].Trim();
                    objData.Add(obj);
                }
            }
            return objData;
        }

        public List<Weightage> WeightageBNData(string path)
        {
            string csvData = File.ReadAllText(path);
            string[] data1 = csvData.Split('@');
            List<Weightage> objData = new List<Weightage>();
            foreach (var item in data1)
            {
                if (!string.IsNullOrEmpty(item.Trim()))
                {
                    string[] data2 = item.Split('#');
                    Weightage obj = new Weightage();
                    obj.Name = data2[0].Trim();
                    obj.Value = data2[1].Trim();
                    objData.Add(obj);
                }
            }
            return objData;
        }
    }

}
