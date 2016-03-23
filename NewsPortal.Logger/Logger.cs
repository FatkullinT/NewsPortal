using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using ILogger = NewsPortal.Domain.Logger.ILogger;

namespace NewsPortal.Logger
{
    public class Logger : ILogger
    {
        private readonly ILog _log;

        public Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger(typeof(Logger));
        }

        /// <summary>
        /// Запись информационного лога
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            _log.Info(message);
        }

        /// <summary>
        /// Запись отладочного лога
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            _log.Debug(message);
        }

        /// <summary>
        /// Логирование ошибки
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
            _log.Error(ex.Message, ex);
        }

        /// <summary>
        /// Логирование ошибки
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Error(string message, Exception ex)
        {
            _log.Error(message, ex);
        }

        /// <summary>
        /// Пути к файлам с логами (если такие есть)
        /// </summary>
        /// <returns></returns>
        public string[] GetLogFilePaths()
        {
            IEnumerable<FileAppender> fileAppenders = ((Hierarchy) LogManager.GetRepository())
                .Root
                .Appenders
                .OfType<FileAppender>();
            return fileAppenders.Select(appender => appender.File).ToArray();
        }
    }
}
