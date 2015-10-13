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
}
