using System;

namespace TYU.Script.Virtual_OS.Virus
{
    [Flags]
    public enum VirusEnum
    {
        None = 0,
        MEMEZ = 1 << 0,
        WeakPoint = 1 << 1, //취약점
        SecurityError = 1 << 2, //보안오류
        
        
    }
}