using log4net.Core;

namespace PSC.Log4net.Async
{
    internal class LoggingEventContext
    {
        public LoggingEventContext(LoggingEvent loggingEvent, object httpContext)
        {
            LoggingEvent = loggingEvent;
            HttpContext = httpContext;
        }

        public LoggingEvent LoggingEvent { get; set; }

        public object HttpContext { get; set; }
    }
}