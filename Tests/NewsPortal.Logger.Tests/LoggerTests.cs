using System;
using System.IO;
using System.Linq;
using NewsPortal.Domain.Logger;
using NUnit.Framework;

namespace NewsPortal.Logger.Tests
{
    /// <summary>
    /// Тесты логгера
    /// </summary>
    [TestFixture]
    public class LoggerTests
    {
        private ILogger _logger;
        private const string FilePath = "D:\\Logs\\log.txt";

        [SetUp]
        public void Setup()
        {
             _logger = new Logger();
        }

        /// <summary>
        /// Запись лога
        /// </summary>
        [Ignore("Тест для запуска в ручную. Создается файл лога.")]
        [Test]
        public void Logger_CreateErrorLog()
        {
            //act
            _logger.Error(new Exception("test"));
            //assert
            FileInfo info = new FileInfo(FilePath);
            FileAssert.Exists(info);
        }

        /// <summary>
        /// Путь файла лога
        /// </summary>
        [Test]
        public void Logger_CheckFilePath()
        {
            //act
            string[] filePaths = _logger.GetLogFilePaths();
            //assert
            Assert.AreEqual(filePaths.Length, 1);
            Assert.AreEqual(filePaths.First(), FilePath);
        }
    }
}
