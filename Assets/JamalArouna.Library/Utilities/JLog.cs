using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace JamalArouna.Utilities.Logging
{
    /// <summary>
    /// Provides utility methods for logging messages with optional categories, colors, contexts, and log types.
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
        /// Logs a message without color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Log(string message, bool willLog = true, Object context = null)
            => LogMessage(message, Color.clear, willLog, LogTypes.Log, context);

        /// <summary>
        /// Logs a message with a specified color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="messageColor">The color applied to the message.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Log(string message, Color messageColor, bool willLog = true, Object context = null)
            => LogMessage(message, messageColor, willLog, LogTypes.Log, context);

        /// <summary>
        /// Logs a message with a category prefix.
        /// </summary>
        /// <param name="category">The category label.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Log(string category, string message, bool willLog = true, Object context = null)
            => LogMessage($"[{category}] {message}", Color.clear, willLog, LogTypes.Log, context);

        /// <summary>
        /// Logs a categorized message with a specified color.
        /// </summary>
        /// <param name="category">The category label.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="color">The color applied to the message.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Log(string category, string message, Color color, bool willLog = true, Object context = null)
            => LogMessage($"[{category}] {message}", color, willLog, LogTypes.Log, context);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Error(string message, bool willLog = true, Object context = null)
            => LogMessage(message, Color.red, willLog, LogTypes.Error, context);

        /// <summary>
        /// Logs a categorized error message.
        /// </summary>
        /// <param name="category">The category label.</param>
        /// <param name="message">The error message.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Error(string category, string message, bool willLog = true, Object context = null)
            => LogMessage($"[{category}] {message}", Color.red, willLog, LogTypes.Error, context);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Warning(string message, bool willLog = true, Object context = null)
            => LogMessage(message, Color.yellow, willLog, LogTypes.Warning, context);

        /// <summary>
        /// Logs a categorized warning message.
        /// </summary>
        /// <param name="category">The category label.</param>
        /// <param name="message">The warning message.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        public static void Warning(string category, string message, bool willLog = true, Object context = null)
            => LogMessage($"[{category}] {message}", Color.yellow, willLog, LogTypes.Warning, context);

        /// <summary>
        /// Handles the internal log formatting and output.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="color">The optional color applied to the message.</param>
        /// <param name="willLog">If false, the message will not be logged.</param>
        /// <param name="logType">The type of log message to output.</param>
        /// <param name="context">
        /// Optional Unity object used as the log context. When assigned, Unity highlights the object when the log entry is selected.
        /// </param>
        [HideInCallstack]
        private static void LogMessage(
            string message,
            Color color,
            bool willLog,
            LogTypes logType,
            Object context = null)
        {
            if (!willLog) return;

            string logText = color != Color.clear
                ? $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>"
                : message;

            switch (logType)
            {
                case LogTypes.Log:
                    Debug.Log(logText, context);
                    break;

                case LogTypes.Error:
                    Debug.LogError(logText, context);
                    break;

                case LogTypes.Warning:
                    Debug.LogWarning(logText, context);
                    break;
            }
        }
    }
}