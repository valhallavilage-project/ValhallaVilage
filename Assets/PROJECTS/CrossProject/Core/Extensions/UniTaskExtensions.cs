using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core.Extensions
{
    /// <summary>
    /// Extension methods for UniTask to prevent lost exceptions
    /// </summary>
    public static class UniTaskExtensions
    {
        /// <summary>
        /// Forget task but log any exceptions that occur
        /// Use this instead of plain .Forget() to prevent silent failures
        /// </summary>
        public static void ForgetWithLog(this UniTask task, string context = null)
        {
            task.Forget(exception =>
            {
                if (exception != null)
                {
                    var contextInfo = string.IsNullOrEmpty(context) ? "" : $" in {context}";
                    Debug.LogError($"[UniTask Exception{contextInfo}] {exception.GetType().Name}: {exception.Message}");
                    Debug.LogException(exception);
                }
            });
        }
    }
}
