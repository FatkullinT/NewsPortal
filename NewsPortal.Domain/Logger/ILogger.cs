using System;

namespace NewsPortal.Domain.Logger
{
    public interface ILogger
    {
        /// <summary>
        /// Запись информационного лога
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// Запись отладочного лога
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// Логирование ошибки
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        /// Логирование ошибки
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(string message, Exception ex);

        /// <summary>
        /// Пути к файлам с логами (если такие есть)
        /// </summary>
        /// <returns></returns>
        string[] GetLogFilePaths();
    }
}