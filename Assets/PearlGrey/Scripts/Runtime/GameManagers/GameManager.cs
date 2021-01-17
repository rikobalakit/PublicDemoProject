using System.Collections;
using UnityEngine;

namespace PearlGreySoftware
{

    public class GameManager : PearlBehaviour
    {

        #region Private Static Fields

        private static GameManager s_instance = null;

        #endregion

        #region Private Fields

        private InputManager m_inputManager = null;
        private OVRManager m_ovrManager = null;

        #endregion

        #region Public Static Properties

        public static GameManager Instance
        {
            get { return s_instance; }
        }

        #endregion

        #region Public Properties

        public InputManager InputManager
        {
            get { return m_inputManager; }
        }

        public OVRManager OVRManager
        {
            get { return m_ovrManager; }
        }

        #endregion

        #region Private Methods

        private IEnumerator Start()
        {
            SetStatus("Initializing");

            // RPB: Establish a singleton instance of GameManager

            if (s_instance != null)
            {
                SetStatus("Initializing a GameManager when one already exists. GameManager is a SINGLETON", LogType.Error);
                Object.Destroy(this);
                yield break;
            }

            s_instance = this;


            // RPB: GameManager needs to persist through the application lifetime
            DontDestroyOnLoad(this.gameObject);

            m_ovrManager = gameObject.AddComponent<OVRManager>();

            m_inputManager = gameObject.AddComponent<InputManager>();
            m_inputManager.InitializeFromGameManager(this);

            SetStatus("Initialized");

        }

        #endregion

    }
}