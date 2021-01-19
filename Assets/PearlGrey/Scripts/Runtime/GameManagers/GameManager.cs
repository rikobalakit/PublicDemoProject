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

        private IInputManager m_inputManager = null;

        #endregion

        #region Public Static Properties

        public static GameManager Instance
        {
            get { return s_instance; }
        }

        #endregion

        #region Public Properties

        public IInputManager InputManager
        {
            get { return m_inputManager; }
        }

        #endregion

        #region Private Methods

        private IEnumerator Start()
        {
            SetStatus(StandardStatus.INITIALIZATION_RUNNING);

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

            // TODO-RPB: Recognize what current XR device is being used. Then create the appropriate InputManager
            m_inputManager = gameObject.AddComponent<OculusInputManager>();
            m_inputManager.InitializeFromGameManager(this);

            SetInitialized();
        }

        #endregion

    }

}