using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DinkToPdf
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VoidCallback(IntPtr converter);
}
