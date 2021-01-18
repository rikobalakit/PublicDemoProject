using System.Collections;
using UnityEngine;

namespace PearlGreySoftware
{

    public class PlayerHandController : PlayerBodyPartController
    {

        #region Private Constants

        // TODO-RPB: Abstract this in InputManager.
        private object INPUT_IDENTIFIER_LEFT_GRIP = OVRInput.Axis1D.PrimaryHandTrigger;
        private object INPUT_IDENTIFIER_RIGHT_GRIP = OVRInput.Axis1D.SecondaryHandTrigger;

        private object INPUT_IDENTIFIER_LEFT_INDEX = OVRInput.Axis1D.PrimaryIndexTrigger;
        private object INPUT_IDENTIFIER_RIGHT_INDEX = OVRInput.Axis1D.SecondaryIndexTrigger;

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

        [SerializeField]
        private Transform m_gripPivot = null;

        [SerializeField]
        private Transform m_indexPivot = null;

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

                inputStates[INPUT_IDENTIFIER_LEFT_INDEX].OnValueChanged.AddListener(OnIndexValueChanged);
                inputStates[INPUT_IDENTIFIER_LEFT_INDEX].OnInputUp.AddListener(OnIndexUp);
                inputStates[INPUT_IDENTIFIER_LEFT_INDEX].OnInputDown.AddListener(OnIndexDown);
            }
            else if (m_chirality == HandSide.Right)
            {
                var inputStates = GameManager.Instance.InputManager.InputStates;
                inputStates[INPUT_IDENTIFIER_RIGHT_GRIP].OnValueChanged.AddListener(OnGripValueChanged);
                inputStates[INPUT_IDENTIFIER_RIGHT_GRIP].OnInputUp.AddListener(OnGripUp);
                inputStates[INPUT_IDENTIFIER_RIGHT_GRIP].OnInputDown.AddListener(OnGripDown);

                inputStates[INPUT_IDENTIFIER_RIGHT_INDEX].OnValueChanged.AddListener(OnIndexValueChanged);
                inputStates[INPUT_IDENTIFIER_RIGHT_INDEX].OnInputUp.AddListener(OnIndexUp);
                inputStates[INPUT_IDENTIFIER_RIGHT_INDEX].OnInputDown.AddListener(OnIndexDown);
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
            UpdateGripVisuals(newValue);
        }

        private void OnGripDown()
        {

        }

        private void OnGripUp()
        {

        }

        private void UpdateGripVisuals(float newValue)
        {
            if (m_gripPivot == null)
            {
                SetStatus($"{nameof(m_gripPivot)} null!");
                return;
            }

            float yRotation = 0f;
            // TODO-RPB: Generalize these magic numbers
            if (m_chirality == HandSide.Left)
            {
                yRotation = Mathf.Lerp(-30f, 120f, newValue);
            }
            else
            {
                yRotation = Mathf.Lerp(30f, -120f, newValue);
                
            }

            m_gripPivot.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        }

        private void OnIndexValueChanged(float newValue)
        {
            UpdatIndexVisuals(newValue);
        }

        private void OnIndexDown()
        {

        }

        private void OnIndexUp()
        {

        }

        private void UpdatIndexVisuals(float newValue)
        {
            if (m_indexPivot == null)
            {
                SetStatus($"{nameof(m_indexPivot)} null!");
                return;
            }

            float yRotation = 0f;
            // TODO-RPB: Generalize these magic numbers
            if (m_chirality == HandSide.Left)
            {
                yRotation = Mathf.Lerp(-30f, 120f, newValue);
            }
            else
            {
                yRotation = Mathf.Lerp(30f, -120f, newValue);

            }

            m_indexPivot.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        }


        #endregion

    }
}