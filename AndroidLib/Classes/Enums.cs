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

    public enum RebootMode
    {
        Normal,
        Bootloader,
        Recovery
    }

    public enum InstallLocationType
    {
        Auto = 0,
        Internal = 1,
        External = 2
    }
}
