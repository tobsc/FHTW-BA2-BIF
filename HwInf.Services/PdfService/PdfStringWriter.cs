using System.IO;
using System.Text;

namespace HwInf.Services.PdfService
{
    public class PdfStringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}