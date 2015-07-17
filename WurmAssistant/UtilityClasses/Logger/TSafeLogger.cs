using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WurmAssistant
{
    /// <summary>
    /// Use from threads other than main
    /// </summary>
    static class TSafeLogger
    {
        public static void WriteLine(string message)
        {
            try
            {
                WurmAssistant.ZeroRef.BeginInvoke(
                    new WurmAssistant.LoggerWriteLineCallback(WurmAssistant.ZeroRef.InvokeLoggerWriteLine),
                    new object[] { message });
            }
            catch
            {
                Debug.WriteLine("Exception: TsafeLoggerWriteLine");
            }
        }

        public static void DisplayExceptionData(Exception exception)
        {
            try
            {
                WurmAssistant.ZeroRef.BeginInvoke(
                    new WurmAssistant.LoggerDisplayExceptionDataCallback(WurmAssistant.ZeroRef.InvokeLoggerDisplayExceptionData),
                    new object[] { exception });
            }
            catch
            {
                Debug.WriteLine("Exception: TSafeLoggerDisplayExceptionData");
            }
        }

        public static void CriticalException(Exception exception)
        {
            WurmAssistant.ZeroRef.BeginInvoke(
                new WurmAssistant.LoggerCriticalExceptionCallback(WurmAssistant.ZeroRef.InvokeCriticalException),
                new object[] { exception });
        }
    }
}
