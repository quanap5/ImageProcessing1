using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
    public interface ITesseractOCR
    {
        string OneImageOCR(ImageClass one);
        List<string> ListImageOCR(List<ImageClass> list);

        String getTime();

        void SelectLang(string selectedLang);
    }
}
