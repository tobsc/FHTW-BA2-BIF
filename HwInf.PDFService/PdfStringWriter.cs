
using System.IO;
using System.Text;

namespace HwInf.PDFService
{
    public class PdfStringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}