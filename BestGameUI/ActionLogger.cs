using System;
using System.IO;
using System.Text;

namespace BestGameUI
{
    /// <summary>Пишет все боевые события в файл «Logs/…». Поток открыт один раз на всю программу.</summary>
    public static class ActionLogger
    {
        private static StreamWriter? _writer;

        /// <summary>Создаёт новый лог-файл при запуске боя.</summary>
        public static void StartNewBattle()
        {
            Directory.CreateDirectory("Logs");
            var fileName = $"Logs\\{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            _writer = new StreamWriter(fileName, false, Encoding.UTF8) { AutoFlush = true };
        }

        public static void Log(string text)
        {
            if (_writer == null) return;          // если по ошибке не инициализировали
            _writer.WriteLine(text);
        }

        public static void Close() => _writer?.Dispose();
    }
}
