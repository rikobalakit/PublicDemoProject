using UnityEngine;

namespace PearlGreySoftware
{

    public class PearlBehaviour : MonoBehaviour
    {

        #region Public Enums

        public enum LogLevel
        {
            None = 0,
            Error = 1,
            Warning = 2,
            All = 3
        }

        public enum LogType
        {
            Error = 0,
            Warning = 1,
            General = 2
        }

        #endregion

        #region Protected Fields

        [SerializeField]
        protected LogLevel CurrentLogLevel = PearlBehaviour.LogLevel.All;

        #endregion

        #region Private Fields

        [ReadOnly]
        [SerializeField]
        private string m_status = string.Empty;

        private bool m_isInitialized = false;

        #endregion

        #region Public Properties

        public bool IsInitialized
        {
            get { return m_isInitialized; }
        }

        #endregion

        #region Protected Properties

        protected string Status
        {
            get { return m_status; }
        }

        #endregion

        #region Protected Methods

        protected void SetInitialized()
        {
            SetStatus(StandardStatus.INITIALIZATION_FINISHED, LogType.General);
            m_isInitialized = true;
        }

        protected void SetInitialized(string customMessage)
        {
            SetStatus(customMessage, LogType.General);
            m_isInitialized = true;
        }

        protected void SetStatus(string statusText)
        {
            SetStatus(statusText, LogType.General);
        }

        protected void SetStatus(string statusText, LogType logType)
        {
            m_status = statusText;
            Log(statusText, logType);
        }

        // TODO-RPB: Add a filter and option for repetitive statuses.

        protected void Log(string logText)
        {
            Log(logText, LogType.General);
        }

        protected void Log(string logText, LogType logType)
        {
            var formattedLogText = $"[{gameObject.name}.{this.GetType().Name}] {logText}";

            if (logType == LogType.Error && (CurrentLogLevel == LogLevel.All || CurrentLogLevel == LogLevel.Warning || CurrentLogLevel == LogLevel.Error))
            {
                Debug.LogError(formattedLogText);
            }
            else if (logType == LogType.Warning && (CurrentLogLevel == LogLevel.All || CurrentLogLevel == LogLevel.Warning))
            {
                Debug.LogWarning(formattedLogText);
            }
            else if (logType == LogType.General && CurrentLogLevel == LogLevel.All)
            {
                Debug.Log(formattedLogText);
            }
        }

        #endregion

    }
}