using System.Collections;
using UnityEngine;

namespace PearlGreySoftware
{

    public class PlayerMainController : PearlBehaviour
    {

        #region Private Constants

        private object INPUT_IDENTIFIER_LEFT_THUMBSTICK_BUTTON = OVRInput.Button.PrimaryThumbstick;
        private object INPUT_IDENTIFIER_RIGHT_THUMBSTICK_BUTTON = OVRInput.Button.SecondaryThumbstick;

        #endregion

        #region Private Fields

        [SerializeField]
        private Transform m_playerRootTransform = null;

        [SerializeField]
        private PlayerHeadController m_playerHead = null;

        [SerializeField]
        private PlayerHandController m_playerHandLeft = null;

        [SerializeField]
        private PlayerHandController m_playerHandRight = null;

        // TODO-RPB: Allow this to be automatically found in the scene upon level load
        [SerializeField]
        private HeadCalibrationPoint m_headCalibrationPoint = null;

        private InputName m_thumbXYButtonLeftInputName = InputName.ThumbXYLeftButton;
        private InputName m_thumbXYButtonRightInputName = InputName.ThumbXYRightButton;

        #endregion

        #region Private Methods

        private IEnumerator Start()
        {
            yield return new WaitWhile(() => GameManager.Instance == null);
            yield return new WaitWhile(() => GameManager.Instance.InputManager == null);
            yield return new WaitWhile(() => !GameManager.Instance.InputManager.IsInitialized);

            if (m_playerHead == null)
            {
                SetStatus($"{nameof(m_playerHead)} not assigned. Looking for one now...", LogType.Warning);
                m_playerHead = gameObject.GetComponentInChildren<PlayerHeadController>(true);

                if (m_playerHead == null)
                {
                    SetStatus("No Head",  LogType.Error);
                    yield break;
                }
                else
                {
                    SetStatus("Found a head");
                }
            }

            if (m_playerHandLeft == null)
            {
                SetStatus($"{nameof(m_playerHandLeft)} not assigned. Looking for one now...", LogType.Warning);
                m_playerHandLeft = FindHand(PlayerHandController.HandSide.Left);
            }

            if (m_playerHandRight == null)
            {
                SetStatus($"{nameof(m_playerHandRight)} not assigned. Looking for one now...", LogType.Warning);
                m_playerHandRight = FindHand(PlayerHandController.HandSide.Right);
            }

            var inputStates = GameManager.Instance.InputManager.InputStates;
            inputStates[m_thumbXYButtonLeftInputName].OnInputDown.AddListener(OnAnyThumbXYButtonDown);
            inputStates[m_thumbXYButtonRightInputName].OnInputDown.AddListener(OnAnyThumbXYButtonDown);

            ResetPlayerRootTransform();

            SetStatus("Initialized");
        }

        private void ResetPlayerRootTransform()
        {
            float yRotationDelta = m_headCalibrationPoint.transform.eulerAngles.y - m_playerHead.transform.eulerAngles.y;
            m_playerRootTransform.rotation *= Quaternion.Euler(0f, yRotationDelta, 0f);

            var positionDelta = m_headCalibrationPoint.transform.position - m_playerHead.transform.position;
            m_playerRootTransform.position += positionDelta;
        }

        private PlayerHandController FindHand(PlayerHandController.HandSide chirality)
        {
            
            var foundHands = gameObject.GetComponentsInChildren<PlayerHandController>(true);

            foreach (var foundHand in foundHands)
            {
                if (foundHand.Chirality == chirality)
                {
                    SetStatus($"{chirality} Hand found");
                    return foundHand;
                }
            }

            SetStatus($"No {chirality} Hand found", LogType.Error);
            return null;
        }

        private void OnAnyThumbXYButtonDown()
        {
            ResetPlayerRootTransform();
        }

        #endregion

    }

}