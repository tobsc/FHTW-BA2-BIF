using System;
using System.Collections.Generic;
using System.Text;

namespace DinkToPdf.Contracts
{
    public interface IDocument: ISettings
    {
        IEnumerable<IObject> GetObjects();
    }
}
