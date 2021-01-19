﻿using System.Collections.Generic;
using UnityEngine;

namespace PearlGreySoftware
{
    public class InteractiveSteeringWheel : InteractiveObject
    {

        #region Public Events

        public VoidEvent OnShootingStarted = new VoidEvent();
        public VoidEvent OnShootingEnded = new VoidEvent();
        public VoidEvent OnSpecialWeaponTriggered = new VoidEvent();

        #endregion

        #region Private Fields

        [SerializeField]
        private float m_maximumAbsoluteAngle = 60f;

        [SerializeField]
        private GameObject m_visuals = null;

        private float m_currentAngle = 0f;
        private List<PlayerHandController> m_handsPullingTrigger = new List<PlayerHandController>();
        private bool m_currentShootingState = false;

        #endregion

        #region Public Methods

        public override bool TryStartInteraction(PlayerHandController hand)
        {
            if (base.TryStartInteraction(hand))
            {
                hand.OnFaceButtonDown.AddListener(OnFaceButtonDown);
                hand.OnTriggerDown.AddListener(OnTriggerDown);
                hand.OnTriggerUp.AddListener(OnTriggerUp);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool TryEndInteraction(PlayerHandController hand)
        {
            if (base.TryEndInteraction(hand))
            {
                hand.OnFaceButtonDown.RemoveListener(OnFaceButtonDown);
                hand.OnTriggerDown.RemoveListener(OnTriggerDown);
                hand.OnTriggerUp.RemoveListener(OnTriggerUp);
                m_handsPullingTrigger.Remove(hand);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Protected Methods

        protected new void Update()
        {
            base.Update();
            m_currentAngle = Mathf.Clamp(GetSteeringWheelAngleBasedOnHandPositions(), -m_maximumAbsoluteAngle, m_maximumAbsoluteAngle);
            UpdateTriggerFunctions();
            UpdateVisuals();
        }

        #endregion

        #region Private Methods

        private float GetSteeringWheelAngleBasedOnHandPositions()
        {
            // TODO-RPB: 
            if (m_handsInteractingWithMe.Count == 0)
            {
                return 0f;
            }
            if (m_handsInteractingWithMe.Count == 1)
            {
                // RPB: Angle is delta of angle difference from start to current position
                var handStartPosition = transform.InverseTransformPoint(m_interactionStartPoint[m_handsInteractingWithMe[0]]);
                var handCurrentPosition = transform.InverseTransformPoint(m_handsInteractingWithMe[0].transform.position);

                var flattenedHandStartPosition = new Vector2(handStartPosition.x, handStartPosition.y);
                var flattenedHandCurrentPosition = new Vector2(handCurrentPosition.x, handCurrentPosition.y);

                return Vector2.SignedAngle(flattenedHandStartPosition, flattenedHandCurrentPosition);

            }
            else if (m_handsInteractingWithMe.Count == 2)
            {
                // RPB: Angle is angle from left to right hand
                // TODO-RPB: Check if this works if the left and right hands are switched for some weird reason...
                // TODO-RPB: Fix how this breaks depending on order of engagement with the hands
                // Solution is probably figuring out which is a left and which is a right hand, and/or adding/subtracking 180 degrees.
                var hand0CurrentPosition = transform.InverseTransformPoint(m_handsInteractingWithMe[0].transform.position);
                var hand1CurrentPosition = transform.InverseTransformPoint(m_handsInteractingWithMe[1].transform.position);
                var flattened0HandCurrentPosition = new Vector2(hand0CurrentPosition.x, hand0CurrentPosition.y);
                var flattened1HandCurrentPosition = new Vector2(hand1CurrentPosition.x, hand1CurrentPosition.y);

                return Vector2.SignedAngle(new Vector2(1f, 0f), flattened0HandCurrentPosition - flattened1HandCurrentPosition);
            }
            else
            {
                Log("Tried to set steering angle with more than 2 hands interacting...", LogType.Error);
                return 0f;
            }
        }

        private void UpdateVisuals()
        {
            // TODO-RPB: Move hands to the wheel while interacting.
            m_visuals.transform.localEulerAngles = new Vector3(0f, 0f, m_currentAngle);
        }

        private void UpdateTriggerFunctions()
        {
            var newShootingState = false;

            if (m_handsPullingTrigger.Count == 0)
            {
                newShootingState = false;
            }
            else
            {
                newShootingState = true;

                for (int i = 0; i < m_handsPullingTrigger.Count; i++)
                {
                    // TODO-RPB: Trigger the haptics, per-hand
                }
            }

            if (newShootingState != m_currentShootingState)
            {
                if (newShootingState == true)
                {
                    OnShootingStarted.Invoke();
                }
                else
                {
                    OnShootingEnded.Invoke();
                }

                m_currentShootingState = newShootingState;
            }
        }

        private void OnTriggerDown(PlayerHandController hand)
        {
            Debug.LogError("hi trigger down");
            m_handsPullingTrigger.Add(hand);
        }

        private void OnTriggerUp(PlayerHandController hand)
        {
            Debug.LogError("hi trigger up");
            m_handsPullingTrigger.Remove(hand);
        }

        private void OnFaceButtonDown(PlayerHandController hand)
        {
            Debug.LogError("hi face button down");
            OnSpecialWeaponTriggered.Invoke();
        }

        #endregion

    }
}