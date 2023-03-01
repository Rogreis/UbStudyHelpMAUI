using log4net;
using log4net.Appender;
using log4net.Core;
namespace AmadonStandardLib.Helpers
{
    internal class AmadonLogAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            //string message = loggingEvent.RenderedMessage;
            //StaticObjects.FireSendMessage(message);
        }
    }
}
