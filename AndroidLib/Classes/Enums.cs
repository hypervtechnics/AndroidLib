using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib
{
    public enum DeviceState
    {
        Unknown,
        Online,
        Offline,
        Bootloader,
        Unauthorized
    }

    public enum AdbMountType
    {
        Root,
        NonRoot
    }

    public enum ErrorType
    {
        RemoteObjectNotFound,
        NoSuchFileOrDirectory,
        Unknown,
        None
    }
}
