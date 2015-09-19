# AndroidLib
.NET Wrapper around adb.exe and advanced shell commands by code

This library should make it easier for .NET developers who are developing applications for(not on) android to interact with the phone over adb and stuff like that. With this library the splitting, cutting, trimming of the adb output is finished! It tries to simplify to do every adb command by code and get its result also in code so e.g. device.Name, device.SerialNumber, device.InstallApk("C:\foo.apk", InstallLocation.Sd), String output = Adb.ExecuteCommandWithOutput(device, "bugreport") and so on). It is inspirated and parts of code are taken from https://github.com/regaw-leinad/AndroidLib!
