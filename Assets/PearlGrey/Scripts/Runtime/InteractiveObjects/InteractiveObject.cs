using System.Collections.Generic;
using UnityEngine;

namespace PearlGreySoftware
{

    [RequireComponent(typeof(Collider))]
    public class InteractiveObject : PearlBehaviour
    {

        // TODO-RPB: Standard haptic feedback?

        #region Public Events

        [SerializeField]
        public VoidEvent OnHighlightStarted = new VoidEvent();

        [SerializeField]
        public VoidEvent OnHighlightEnded = new VoidEvent();

        [SerializeField]
        public VoidEvent OnInteractionStarted = new VoidEvent();

        [SerializeField]
        public VoidEvent OnInteractionEnded = new VoidEvent();

        [SerializeField]
        public VoidEvent OnIdleStarted = new VoidEvent();

        [SerializeField]
        public VoidEvent OnIdleEnded = new VoidEvent();

        #endregion

        #region Private Static Fields

        private static List<InteractiveObject> s_interactiveObjects = new List<InteractiveObject>();

        #endregion

        #region Private Fields

        [ReadOnly]
        [SerializeField]
        private InteractionState m_currentInteractionState = InteractionState.Unassigned;

        [SerializeField]
        private bool m_isTwoHanded = false;

        private List<PlayerHandController> m_handsHighlightingMe = new List<PlayerHandController>();
        private List<PlayerHandController> m_handsInteractingWithMe = new List<PlayerHandController>();


        #endregion

        #region Public Static Properties


        public static IReadOnlyList<InteractiveObject> InteractiveObjects
        {
            get { return s_interactiveObjects; }
        }

        #endregion

        #region Public Methods


        public bool TryStartHighlight(PlayerHandController hand)
        {
            if (!m_handsHighlightingMe.Contains(hand))
            {
                m_handsHighlightingMe.Add(hand);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryEndHighlight(PlayerHandController hand)
        {
            if (m_handsHighlightingMe.Contains(hand))
            {
                m_handsHighlightingMe.Remove(hand);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryStartInteraction(PlayerHandController hand)
        {
            if (m_isTwoHanded || m_handsInteractingWithMe.Count == 0)
            {
                m_handsInteractingWithMe.Add(hand);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryEndInteraction(PlayerHandController hand)
        {
            m_handsInteractingWithMe.Remove(hand);
            return true;
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            RegisterInteractiveObject();
        }

        private void OnDestroy()
        {
            UnregisterInteractiveObject();
        }

        private void Update()
        {
            var newInteractionState = InteractionState.Unassigned;

            if (m_handsInteractingWithMe.Count > 0)
            {
                newInteractionState = InteractionState.Interacting;
            }
            else if (m_handsHighlightingMe.Count > 0)
            {
                newInteractionState = InteractionState.Highlighted;
            }
            else
            {
                newInteractionState = InteractionState.Idle;
            }

            if (newInteractionState != m_currentInteractionState)
            {
                switch (m_currentInteractionState)
                {
                    case InteractionState.Idle:
                        OnIdleEnded.Invoke();
                        break;
                    case InteractionState.Highlighted:
                        OnHighlightEnded.Invoke();
                        break;
                    case InteractionState.Interacting:
                        OnInteractionEnded.Invoke();
                        break;
                    default:
                        break;
                }

                switch (newInteractionState)
                {
                    case InteractionState.Idle:
                        OnIdleStarted.Invoke();
                        break;
                    case InteractionState.Highlighted:
                        OnHighlightStarted.Invoke();
                        break;
                    case InteractionState.Interacting:
                        OnInteractionStarted.Invoke();
                        break;
                    default:
                        break;
                }

                m_currentInteractionState = newInteractionState;
            }
        }

        private void RegisterInteractiveObject()
        {
            s_interactiveObjects.Add(this);
        }

        private void UnregisterInteractiveObject()
        {
            s_interactiveObjects.Remove(this);
        }

        #endregion

    }

    public enum InteractionState
    {
        Unassigned,
        Idle,
        Highlighted,
        Interacting
    }
}