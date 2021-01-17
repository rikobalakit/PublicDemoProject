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
        private string m_status = "Uninitialized";

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
            SetStatus("Initialized", LogType.General);
            m_isInitialized = true;
        }

        // TODO-RPB: Add a filter and option for repetitive statuses.

        protected void SetStatus(string statusText)
        {
            SetStatus(statusText, LogType.General);
        }

        protected void SetStatus(string statusText, LogType logType)
        {
            m_status = statusText;
            var logText = $"[{gameObject.name}.{this.GetType().Name}] {statusText}";

            if (logType == LogType.Error && (CurrentLogLevel == LogLevel.All || CurrentLogLevel == LogLevel.Warning || CurrentLogLevel == LogLevel.Error))
            {
                Debug.LogError(logText);
            }
            else if (logType == LogType.Warning && (CurrentLogLevel == LogLevel.All || CurrentLogLevel == LogLevel.Warning))
            {
                Debug.LogWarning(logText);
            }
            else if(logType == LogType.General && CurrentLogLevel == LogLevel.All)
            {
                Debug.Log(logText);
            }
        }

        #endregion

    }
}