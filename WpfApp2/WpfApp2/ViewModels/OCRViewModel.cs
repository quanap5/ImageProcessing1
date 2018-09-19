using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp2.Commands;
using WpfApp2.Models;
using System.Collections.ObjectModel;


namespace WpfApp2.ViewModels
{
    public class OCRViewModel : ViewModelBase
    {
        #region Commands
        public ICommand OpenImageCommand { get; set; }
        public ICommand StartOCRCommand { get; set; }
        public ICommand SelectLangCommand { get; set; }
        #endregion

        #region pathToTessData
        private readonly ITesseractOCR _tesseractOCR;
        private readonly string _pathTessData = Environment.CurrentDirectory + @"\tessdata";
        #endregion

        #region Properties
        private List<ImageClass> _imagesList;
        public List<ImageClass> ImagesList
        {
            get { return _imagesList; }
            set
            {
                _imagesList = value;
                OnPropertyChanged("ImagesList");
            }
        }

        private ImageClass _imageOne;
        public ImageClass ImageOne
        {
            get { return _imageOne; }
            set
            {
                _imageOne = value;
                OnPropertyChanged("ImageOne");
            }
        }

        private string _curLang;
        public string CurLang
        {
            get { return _curLang; }
            set
            {
                if (_curLang == value) return;
                
                _curLang = value;
                OnPropertyChanged("CurLang");
                _tesseractOCR.SelectLang(_curLang);
               //_tesseractOCR.SelectLang("eng");
                
            }
        }

        private ObservableCollection<string> _givenLang;
        public ObservableCollection<string> GivenLang
        {
            get { return _givenLang; }
            set
            {
                if (_givenLang == value) return;
                _givenLang = value;
                OnPropertyChanged("GiveLang");
            }
        }

        #endregion

        private string _outPutText;
        public String OutPutText
        {
            get { return _outPutText; }
            set
            {
                _outPutText = value;
                OnPropertyChanged("OutPutText");
            }
        }

        private string _outTime;
        public String OutTime
        {
            get{ return _outTime; }
            set
            {
                _outTime = value;
                OnPropertyChanged("OutTime");
            }
        }

        public OCRViewModel(ITesseractOCR ocr)
        {
            _tesseractOCR = ocr;
            //Implement Command based on RelayCommand
            OpenImageCommand = new RelayCommand(OpenImage);
            StartOCRCommand = new RelayCommand(StartOCR);
       

        }

        public OCRViewModel()
        {
            _tesseractOCR = new TesseractOCR();
            OpenImageCommand = new RelayCommand(OpenImage);
            StartOCRCommand = new RelayCommand(StartOCR);
            GivenLang = new ObservableCollection<string>();
            GivenLang.Add("English");
            GivenLang.Add("Korean");
           // GivenLang.Add("Japanese");
           // GivenLang.Add("Vietnamese");
        }

        private void StartOCR()
        {
            OutPutText = "OCR is running....";
            Console.WriteLine("Executing StartOCR");
            Console.WriteLine(_pathTessData);
            if (!Directory.Exists(_pathTessData))
            {
                MessageBox.Show("You dont have Tess data. OCR can not Run");
            }
            else
            {
                var tem_Text = _tesseractOCR.OneImageOCR(ImageOne);
                //var tem_Text = "Quan Nguyen Van--->";
                if (tem_Text == null)
                {
                    OutPutText = "No answer";
                    OutTime = _tesseractOCR.getTime() + " ms for running";
                }
                else
                {
                    OutPutText = tem_Text;
                    OutTime = _tesseractOCR.getTime() + " ms for running";
                }
            }


        }

        /// <summary>
        /// This is specificed Command
        /// </summary>
        private void OpenImage()
        {
            Console.WriteLine("Executing OpenImage");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a image for OCR";
            openFileDialog.Filter = "All supported graphics |*.jpg; *.jpeg;*.png|" +
                "JPEG(*.jpg;*.jpeg)|*.jpg; *.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                //inputImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                ////textEditor.Text = openFileDialog.FileName;
                //textEditor.Text = "Click Run button to see the results";
                //img_Src = openFileDialog.FileName;
                //ocr = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube);
                //btnRun.IsEnabled = true;
                //toolRun.IsEnabled = true;
                var filename = openFileDialog.FileName;
                var bitmap = new BitmapImage(new Uri(filename));
                
                ImageOne = new ImageClass
                {
                    FilePath = filename,
                    Image = bitmap
                };

                OutPutText = "Click RUN button to start OCR demo";
              
            }
        }
    }
}
