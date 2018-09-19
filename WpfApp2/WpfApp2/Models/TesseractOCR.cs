using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tesseract;

namespace WpfApp2.Models
{
    public class TesseractOCR : ITesseractOCR
    {
        public string executedTime;
        private TesseractEngine ocrEnglish;
        private TesseractEngine ocrKorean;
       // private TesseractEngine ocrJapanese;
        private TesseractEngine curOcr;
        public TesseractOCR()
        {
            ocrEnglish = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
            ocrKorean = new TesseractEngine("./tessdata", "kor", EngineMode.Default);
           //rJapanese = new TesseractEngine("./tessdata", "jpn", EngineMode.Default);

        }

        public string LangORC { get; set; }

        public String getTime()
        {
            return this.executedTime;
        }
        public List<string> ListImageOCR(List<ImageClass> list)
        {
            throw new NotImplementedException();
        }

        public string OneImageOCR(ImageClass one)
        {
            if (one == null)
            {
                return string.Empty;

            }
            else
            {
                return runningOCR(one.FilePath);
            }
        }

        public void SelectLang(string selectedLang)
        {
            switch (selectedLang)
            {
                case "English":
                    LangORC = "eng";
                    curOcr = ocrEnglish;
                    break;
                case "Korean":
                    LangORC = "kor";
                    curOcr = ocrKorean;
                    break;
                case "Japanese":
                    LangORC = "jpn";
                   // curOcr = ocrJapanese;
                    break;
                case "Vietnamese":
                    LangORC = "vie";
                    curOcr = ocrEnglish;
                    break;
                default:
                    LangORC = "eng";
                    curOcr = ocrEnglish;
                    break;
            }

        }

        private string runningOCR(string filePath)
        {
            
            try
            {
                if (!File.Exists(filePath))
                {
                    //return "please open at least one image";
                    MessageBox.Show("Please open at least one image");
                    return null;

                }

                if (LangORC==null)
                {
                    MessageBox.Show("Please select language for recognizing");
                    return null;
                }
                else
                {
                    //Stopwatch stopW = Stopwatch.StartNew();
                    Console.WriteLine("Current language is: "+ LangORC);
                    //using (var ocr = new TesseractEngine("./tessdata", LangORC , EngineMode.Default))
                    //using (var ocr = curOcr)
                    {
                        Stopwatch stopW = Stopwatch.StartNew();
                        using (var img = Pix.LoadFromFile(filePath))
                        {
                            using (var page = curOcr.Process(img))
                            {
                                var resultText = page.GetText();
                                if (!String.IsNullOrEmpty(resultText))
                                {
                                    stopW.Stop();
                                    var time_dur = stopW.Elapsed.TotalMilliseconds.ToString();
                                    this.executedTime = time_dur;
                                    Console.WriteLine(time_dur);
                                    return resultText;
                                }
                            }
                        }
                    }

                   

                    //textEditor.Text = page.GetText();
                }

            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

      
    }
}
