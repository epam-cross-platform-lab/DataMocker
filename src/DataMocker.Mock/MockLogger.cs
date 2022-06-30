using System;
namespace DataMocker.Mock
{
    public static class MockLogger
    {
        private static Action<string> logMethod;

        public static void SetLogMethod(Action<string> action)
        {
            logMethod = action;
        }

        internal static void Log(string message)
        {
            if (logMethod != null)
            {
                logMethod.Invoke(message);
            }
        }
    }
}
