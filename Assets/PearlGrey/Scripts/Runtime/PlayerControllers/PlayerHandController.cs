using System.Collections;
using UnityEngine;

namespace PearlGreySoftware
{

    public class PlayerHandController : PlayerBodyPartController
    {

        #region Private Constants

        private object INPUT_IDENTIFIER_LEFT_GRIP = OVRInput.Axis1D.PrimaryHandTrigger;
        private object INPUT_IDENTIFIER_RIGHT_GRIP = OVRInput.Axis1D.SecondaryHandTrigger;

        #endregion

        #region Public Enums

        public enum HandSide
        {
            Unassigned = 0,
            Left = 1,
            Right = 2
        }

        #endregion

        #region Public Properties

        public HandSide Chirality
        {
            get
            {
                // TODO-RPB: This is lazy init, which isn't great :/
                if (m_chirality == HandSide.Unassigned)
                {
                    SetChiralityFromTrackedPoseDriver();
                }

                return m_chirality;
            }
        }

        #endregion

        #region Private Fields

        [ReadOnly]
        [SerializeField]
        private HandSide m_chirality = default;

        #endregion

        #region Protected Methods

        protected new void Start()
        {
            base.Start();
            SetChiralityFromTrackedPoseDriver();
            StartCoroutine(Initialize());
        }

        #endregion

        #region Private Methods

        private IEnumerator Initialize()
        {
            SetStatus("Waiting for dependencies...");

            yield return new WaitWhile(() => GameManager.Instance == null);
            yield return new WaitWhile(() => GameManager.Instance.InputManager == null);
            yield return new WaitWhile(() => !GameManager.Instance.InputManager.IsInitialized);

            SetStatus("Dependencies finished. Initializing...");

            if (m_chirality == HandSide.Left)
            {
                var inputStates = GameManager.Instance.InputManager.InputStates;
                inputStates[INPUT_IDENTIFIER_LEFT_GRIP].OnValueChanged.AddListener(OnGripValueChanged);
                inputStates[INPUT_IDENTIFIER_LEFT_GRIP].OnInputUp.AddListener(OnGripUp);
                inputStates[INPUT_IDENTIFIER_LEFT_GRIP].OnInputDown.AddListener(OnGripDown);
            }
            else if (m_chirality == HandSide.Right)
            {
                var inputStates = GameManager.Instance.InputManager.InputStates;
                inputStates[INPUT_IDENTIFIER_RIGHT_GRIP].OnValueChanged.AddListener(OnGripValueChanged);
                inputStates[INPUT_IDENTIFIER_RIGHT_GRIP].OnInputUp.AddListener(OnGripUp);
                inputStates[INPUT_IDENTIFIER_RIGHT_GRIP].OnInputDown.AddListener(OnGripDown);
            }
            else
            {
                SetStatus("Tried to initialize while chirality was unassigned");
            }
        }

        private void SetChiralityFromTrackedPoseDriver()
        {
            if (m_trackedPoseDriver.poseSource == UnityEngine.SpatialTracking.TrackedPoseDriver.TrackedPose.LeftPose)
            {
                m_chirality = HandSide.Left;
                SetStatus("Chirality Set: Left");
            }
            else if (m_trackedPoseDriver.poseSource == UnityEngine.SpatialTracking.TrackedPoseDriver.TrackedPose.RightPose)
            {
                m_chirality = HandSide.Right;
                SetStatus("Chirality Set: Right");
            }
            else
            {
                Debug.LogError($"Could not set chirality for {gameObject.name}");
                SetStatus("Couldn't set chirality", LogType.Error);
            }

        }

        private void OnGripValueChanged(float newValue)
        {
            
        }

        private void OnGripDown()
        {
            
        }

        private void OnGripUp()
        {
            
        }
        

        #endregion

    }
}