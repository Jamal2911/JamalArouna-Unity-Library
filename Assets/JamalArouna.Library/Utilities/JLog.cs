using UnityEngine;

namespace JamalArouna.Utilities.Logging
{
    /// <summary>
    /// Utility class for logging messages with optional categories, colors, and log types.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2026.
    /// </remarks>
    public static class JLog
    {
        private enum LogTypes
        {
            Log,
            Error,
            Warning
        }

        /// <summary>
        /// Logs a default message without color.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Log(string message, bool willLog = true)
            => LogMessage(message, Color.clear, willLog, LogTypes.Log);

        /// <summary>
        /// Logs a message with a specified color.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="messageColor">Color applied to the message.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Log(string message, Color messageColor, bool willLog = true)
            => LogMessage(message, messageColor, willLog, LogTypes.Log);

        /// <summary>
        /// Logs a message with a category prefix.
        /// </summary>
        /// <param name="category">Category label.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Log(string category, string message, bool willLog = true)
            => LogMessage($"[{category}] {message}", Color.clear, willLog, LogTypes.Log);

        /// <summary>
        /// Logs a categorized message with a specified color.
        /// </summary>
        /// <param name="category">Category label.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="color">Color applied to the message.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Log(string category, string message, Color color, bool willLog = true)
            => LogMessage($"[{category}] {message}", color, willLog, LogTypes.Log);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Error(string message, bool willLog = true)
            => LogMessage(message, Color.red, willLog, LogTypes.Error);

        /// <summary>
        /// Logs a categorized error message.
        /// </summary>
        /// <param name="category">Category label.</param>
        /// <param name="message">Error message.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Error(string category, string message, bool willLog = true)
            => LogMessage($"[{category}] {message}", Color.red, willLog, LogTypes.Error);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">Warning message.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Warning(string message, bool willLog = true)
            => LogMessage(message, Color.yellow, willLog, LogTypes.Warning);

        /// <summary>
        /// Logs a categorized warning message.
        /// </summary>
        /// <param name="category">Category label.</param>
        /// <param name="message">Warning message.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        public static void Warning(string category, string message, bool willLog = true)
            => LogMessage($"[{category}] {message}", Color.yellow, willLog, LogTypes.Warning);

        /// <summary>
        /// Handles the internal logging logic.
        /// </summary>
        /// <param name="message">Formatted message.</param>
        /// <param name="color">Optional color applied to the message.</param>
        /// <param name="willLog">If false, logging is skipped.</param>
        /// <param name="logType">Type of log to output.</param>
        private static void LogMessage(string message, Color color, bool willLog, LogTypes logType)
        {
            if (!willLog) return;

            string logText;

            if (color != Color.clear)
            {
                string hex = ColorUtility.ToHtmlStringRGB(color);
                logText = $"<color=#{hex}>{message}</color>";
            }
            else
            {
                logText = message;
            }

            switch (logType)
            {
                case LogTypes.Log:
                    Debug.Log(logText);
                    break;
                case LogTypes.Error:
                    Debug.LogError(logText);
                    break;
                case LogTypes.Warning:
                    Debug.LogWarning(logText);
                    break;
            }
        }
    }
}