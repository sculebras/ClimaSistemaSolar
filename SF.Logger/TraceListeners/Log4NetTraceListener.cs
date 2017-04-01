using System;
using System.Diagnostics;
using System.Linq;
using System.Globalization;
using log4net;

namespace SF.Logger.TraceListeners
{
    public class Log4NetTraceListener : TraceListener
    {
        private readonly log4net.ILog _log;
        public Log4NetTraceListener()
        {
            log4net.Config.XmlConfigurator.Configure();
            //log4net.Util.LogLog.EmitInternalMessages = false;
            string strAppName = GetAppName();
            _log = log4net.LogManager.GetLogger(strAppName);
        }

        public static string GetAppName()
        {
            var arrAppName = System.AppDomain.CurrentDomain.FriendlyName.Split('/');
            string strAppName = arrAppName[arrAppName.Length - 1].Split('-')[0];
            return strAppName;

            //System.Reflection.Assembly _objParentAssembly;

            //if (System.Reflection.Assembly.GetEntryAssembly() == null)
            //    _objParentAssembly = System.Reflection.Assembly.GetCallingAssembly();
            //else
            //    _objParentAssembly = System.Reflection.Assembly.GetEntryAssembly();

            //if (_objParentAssembly.CodeBase.StartsWith("http://"))
            //    throw new System.IO.IOException("Deployed from URL");

            //if (System.IO.File.Exists(_objParentAssembly.Location))
            //    return _objParentAssembly.Location;
            //if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + System.AppDomain.CurrentDomain.FriendlyName))
            //    return System.AppDomain.CurrentDomain.BaseDirectory + System.AppDomain.CurrentDomain.FriendlyName;
            //if (System.IO.File.Exists(System.Reflection.Assembly.GetExecutingAssembly().Location))
            //    return System.Reflection.Assembly.GetExecutingAssembly().Location;

            //throw new System.IO.IOException("Assembly not found");
        }
        public override void Write(object o)
        {
            base.Write(o);
        }

        public override void WriteLine(string message)
        {
            this.Write(message);
        }

        public override void Write(string message)
        {
            StackTrace st = new StackTrace();
            var stack = this.GetTracingStackFrame(st);
            var loggerName = stack.GetMethod().DeclaringType.FullName;
            this.Write(message, loggerName);
        }

        public override void WriteLine(string message, string category)
        {
            this.Write(message, category);
        }

        public override void Write(string message, string category)
        {
            //_log = LogManager.GetLogger(category ?? this.GetType().FullName);
            _log.Debug(message);
        }


        public override void Fail(string message)
        {
            this.Fail(message, string.Empty);
        }

        public override void Fail(string message, string detailMessage)
        {
            var stack = new StackTrace();
            var frame = this.GetTracingStackFrame(stack);
            var log = LogManager.GetLogger(frame.GetMethod().DeclaringType);

            message = string.IsNullOrEmpty(detailMessage)
                              ? message
                              : string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", message, Environment.NewLine, detailMessage);
            log.WarnFormat("[Fail] {0}", message);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            var array = new object[] { message, relatedActivityId };
            this.TraceEvent(eventCache, source, TraceEventType.Transfer, id, "{0}", array);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var array = new[] { data };
            this.TraceData(eventCache, source, eventType, id, array);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (null == data || 0 == data.Length)
            {
                base.TraceData(eventCache, source, eventType, id, data);
                return;
            }

            foreach (var datum in data.Where(x => !this.TraceException(eventType, source, x)))
            {
                var array = new[] { datum };
                TraceEvent(eventCache, source, eventType, id, "{0}", array);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (args == null) return;
            if (0 == args.Length)
            {
                base.TraceEvent(eventCache, source, eventType, id, format, args);
            }


            if (string.IsNullOrWhiteSpace(source))
            {
                var frame = GetTracingStackFrame(new StackTrace());
                source = frame.GetMethod().DeclaringType.FullName;

                if (source == null)
                    source = this.GetType().FullName;
            }

            //_log = LogManager.GetLogger(source);
            switch (eventType)
            {
                case TraceEventType.Critical:
                    _log.FatalFormat(format, args);
                    break;

                case TraceEventType.Error:
                    _log.ErrorFormat(format, args);
                    break;

                case TraceEventType.Information:
                    _log.InfoFormat(format, args);
                    break;

                case TraceEventType.Resume:
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Transfer:
                case TraceEventType.Verbose:
                    _log.DebugFormat(format, args);
                    break;

                case TraceEventType.Warning:
                    _log.WarnFormat(format, args);
                    break;
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            var array = new[] { message };
            this.TraceEvent(eventCache, source, eventType, id, "{0}", array);
        }

        private bool TraceException(TraceEventType eventType, string source, object data)
        {
            if (TraceEventType.Critical != eventType && TraceEventType.Error != eventType)
                return false;

            var exception = data as Exception;
            if (null == exception)
                return false;

            if (string.IsNullOrWhiteSpace(source))
            {
                var frame = GetTracingStackFrame(new StackTrace());
                source = frame.GetMethod().DeclaringType.FullName;

                if (source == null)
                    source = this.GetType().FullName;
            }

            //_log = LogManager.GetLogger(source);
            switch (eventType)
            {
                case TraceEventType.Critical:
                    _log.Fatal(exception.Message, exception);
                    break;

                case TraceEventType.Error:
                    _log.Error(exception.Message, exception);
                    break;
            }

            return true;
        }


        private StackFrame GetTracingStackFrame(StackTrace stack)
        {
            for (var i = 0; i < stack.FrameCount; i++)
            {
                var frame = stack.GetFrame(i);
                var method = frame.GetMethod();
                if (null == method)
                {
                    continue;
                }

                if ("System.Diagnostics" == method.DeclaringType.Namespace)
                {
                    continue;
                }

                if ("System.Threading" == method.DeclaringType.Namespace)
                {
                    continue;
                }

                if (this.GetType() == method.DeclaringType)
                {
                    continue;
                }

                return stack.GetFrame(i);
            }

            return null;
        }
    }

}

