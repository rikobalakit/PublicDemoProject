using System.Collections;
using UnityEngine;

namespace PearlGreySoftware
{

    public class PlayerMainController : PearlBehaviour
    {

        #region Public Methods

        public void ResetPlayerPosition()
        {
            ResetPlayerPositionInternal();
        }

        #endregion

        #region Private Fields

        [SerializeField]
        private PlayerHeadController m_playerHead = null;

        [SerializeField]
        private PlayerHandController m_playerHandLeft = null;

        [SerializeField]
        private PlayerHandController m_playerHandRight = null;

        #endregion

        #region Private Methods

        private IEnumerator Start()
        {
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

            SetStatus("Initialized");
        }

        private void ResetPlayerPositionInternal()
        {
            // TODO-RPB: Fill this in
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

        #endregion

    }

}