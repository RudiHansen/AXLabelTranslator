using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleUI
{
    class Program
    {
        public const string APIKey = "REPLACE WITH APIKEY";

        static void Main(string[] args)
        {
            string FilePath = @"C:\Temp";
            string SourceLabelFile = "SourceLabelFile.csv";
            string TranslatedLabelFile = "TranslatedLabelFile.csv";

            List<LabelData> listLabelData = new List<LabelData>();

            listLabelData = ReadLabelDataFromCsv(Path.Combine(FilePath, SourceLabelFile));
            Console.WriteLine($"Read {listLabelData.Count()} labels from {SourceLabelFile}");

            listLabelData = TranslateLabelsToEn(listLabelData);

            WriteLabelDataToCsv(listLabelData, Path.Combine(FilePath, TranslatedLabelFile));
            Console.WriteLine($"Written {listLabelData.Count()} labels to {TranslatedLabelFile}");
            Console.WriteLine("Process done, press a key to end program.");
            Console.ReadKey();
        }
        private static List<LabelData> ReadLabelDataFromCsv(string _fileName)
        {
            List<LabelData> listLabelData = new List<LabelData>();
            bool readLine = false;

            using (var reader = new StreamReader(_fileName, Encoding.GetEncoding(1252)))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (readLine)
                    {
                        string[] values = line.Split(';');

                        LabelData labelData = new LabelData();
                        labelData.LabelId = values[0];
                        labelData.LabelTextDk = values[1];
                        labelData.LabelTextENUS = values[2];
                        labelData.LabelTextAuto = values[3];
                        labelData.LabelTextAutoDk = values[4];
                        if (values.Count() > 5 && values[5] == "True")
                        {
                            labelData.Changed = true;
                        }
                        else
                        {
                            labelData.Changed = false;
                        }
                        listLabelData.Add(labelData);
                    }
                    readLine = true;

                }
            }
            return listLabelData;
        }
        private static List<LabelData> TranslateLabelsToEn(List<LabelData> listLabelData)
        {
            var listToTranslate = listLabelData.Where(item => item.LabelTextDk != "").Where(item => item.LabelTextENUS == "").Where(item => item.LabelTextAuto == "");

            string accessToken = Trans.GetAccessToken(APIKey);
            int currentRecord = 0;
            int maxRecords2Translate = listToTranslate.Count();

            Console.WriteLine($"Translating {maxRecords2Translate}");
            //int maxRecords2Translate = 100;

            foreach (var labelData in listToTranslate)
            {
                if (currentRecord > maxRecords2Translate)
                {
                    break;
                }

                ShowPercentProgress($"Translating {labelData.LabelId} - ", currentRecord, maxRecords2Translate);

                int retries = 3;
                while(retries != 0)
                {
                    try
                    {
                        labelData.LabelTextAuto = Trans.Translate(accessToken, labelData.LabelTextDk, "da", "en");
                        retries = 0;
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "The remote server returned an error: (400) Bad Request.")
                        {
                            accessToken = Trans.GetAccessToken(APIKey);
                            retries--;
                        }
                        else
                        {
                            Console.WriteLine($"There was an error: {e.Message}");
                            throw;
                        }
                    }

                }
                currentRecord++;
            }
            return listLabelData;
        }
        private static void WriteLabelDataToCsv(List<LabelData> listLabelData, string _fileName)
        {
            using (TextWriter file = new StreamWriter(_fileName, false, Encoding.GetEncoding(1252)))
            {
                file.WriteLine("Label num;Danish;English;Auto Translate;Auto Danish;Changed");
                foreach (var labelData in listLabelData)
                {
                    file.WriteLine($"{labelData.LabelId};{labelData.LabelTextDk};{labelData.LabelTextENUS};{labelData.LabelTextAuto};{labelData.LabelTextAutoDk};{labelData.Changed}");
                }
            }
        }
        static void ShowPercentProgress(string message, int currElementIndex, int totalElementCount)
        {
            if (currElementIndex < 0 || currElementIndex >= totalElementCount)
            {
                return;
            }
            int percent = (100 * (currElementIndex + 1)) / totalElementCount;
            Console.Write("\r{0}{1}% complete", message, percent);
            if (currElementIndex == totalElementCount - 1)
            {
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
