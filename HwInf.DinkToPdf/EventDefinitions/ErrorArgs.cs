using DinkToPdf.Contracts;
using System;

namespace DinkToPdf.EventDefinitions
{
    public class ErrorArgs: EventArgs
    {
        public IDocument Document { get; set; }

        public string Message { get; set; }
    }
}
