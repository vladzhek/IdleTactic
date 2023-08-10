using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utils
{
    public static class Logger
    {
        /*
        public static void Log(this object target, 
            string message, 
            [CallerFilePath] string callerPath = "", 
            [CallerMemberName] string caller = "")
        {
            Log(message, callerPath, caller);
        }

        public static void Warning(this object target, 
            string message, 
            [CallerFilePath] string callerPath = "", 
            [CallerMemberName] string caller = "")
        {
            Warning(message, callerPath, caller);
        }

        public static void Error(this object target, 
            string message, 
            [CallerFilePath] string callerPath = "", 
            [CallerMemberName] string caller = "")
        {
            Error(message, callerPath, caller);
        }
        */
        
        public static void Log(object message, 
            [CallerFilePath] string callerPath = "", 
            [CallerMemberName] string caller = "")
        {
            Debug.Log(FormatMessage(message.ToString(), callerPath, caller));
        }
        
        public static void Log(string message = "", 
            [CallerFilePath] string callerPath = "", 
            [CallerMemberName] string caller = "")
        {
            Debug.Log(FormatMessage(message, callerPath, caller));
        }

        public static void Warning(string message = "",
            [CallerFilePath] string callerPath = "", 
            [CallerMemberName] string caller = "")
        {
            Debug.LogWarning($"<color=orange>Warning</color> {FormatMessage(message, callerPath, caller)}");
        }

        public static void Error(string message,
            [CallerFilePath] string callerPath = "", 
            [CallerMemberName] string caller = "")
        {
            Debug.LogError($"<color=red>Error</color> {FormatMessage(message, callerPath, caller)}");
        }

        private static string FormatMessage(string message, string callerPath, string caller)
        {
            var callerTypeName = Path.GetFileNameWithoutExtension(callerPath);
            
            return //$"{DateTime.Now:HH:mm:ss.fff} " +
                $"[{callerTypeName}.{caller}] {message}" ;
        }
    }
}