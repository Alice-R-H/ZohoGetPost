using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZohoGetPost
{
    public class TxtFileMethods
    {
        public void WriteToTxtFile(string fullPath, string result)
        {
            //write to a txt file

            StreamWriter writeToTxt = new StreamWriter(fullPath);

            writeToTxt.WriteLine(result);
            writeToTxt.Close();
        }

        public string ReadTxtFile(string fullPath)
        {
            //read from a txt file

            StreamReader readTxtFile = new StreamReader(fullPath);

            string fileContent = readTxtFile.ReadToEnd();

            readTxtFile.Close();

            return fileContent;
        }

        public void WriteExpiryDate(string txtFolder, string expiryTime)
        {
            //write the new expiry date for the token

            double seconds = 3600;
            DateTime localDate = DateTime.Now;
            string expiryTimeDate = localDate.AddSeconds(seconds).ToString();
            string expiryTimePath = txtFolder + expiryTime;

            WriteToTxtFile(expiryTimePath, expiryTimeDate);
        }
    }
}
