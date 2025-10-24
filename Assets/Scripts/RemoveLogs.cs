using UnityEngine;

public class OVRLogFilter : MonoBehaviour
{
    class FilteredLogHandler : ILogHandler
    {
        private readonly ILogHandler defaultHandler = Debug.unityLogger.logHandler;

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            string message = string.Format(format, args);

   
            if (message.Contains("[OVR") || message.Contains("[MetaXR"))
                return;

            defaultHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(System.Exception exception, Object context)
        {
            defaultHandler.LogException(exception, context);
        }
    }

    void Awake()
    {
        Debug.unityLogger.logHandler = new FilteredLogHandler();
    }
}
