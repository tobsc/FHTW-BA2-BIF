using System;
using System.Collections.Generic;
using System.Text;

namespace DinkToPdf.Contracts
{
    public interface IObject: ISettings
    {
        byte[] GetContent();
    }
}
