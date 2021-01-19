using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace PearlGreySoftware
{

    public class OculusInputManager : PearlBehaviour, IInputManager
    {

        #region Private Constants

        private const float THRESHOLD_BUTTON_DOWN = 0.75f;
        private const float THRESHOLD_BUTTON_UP = 0.25f;

        #endregion

        #region Private Fields

        private Dictionary<InputName, object> m_inputToOVRInputMapping = new Dictionary<InputName, object>(){
            {InputName.ThumbXYLeftButton, OVRInput.Button.PrimaryThumbstick},
            {InputName.ThumbXYRightButton, OVRInput.Button.SecondaryThumbstick},
            {InputName.GripLeftAxis, OVRInput.Axis1D.PrimaryHandTrigger},
            {InputName.GripRightAxis, OVRInput.Axis1D.SecondaryHandTrigger},
            {InputName.IndexLeftAxis, OVRInput.Axis1D.PrimaryIndexTrigger},
            {InputName.IndexRightAxis, OVRInput.Axis1D.SecondaryIndexTrigger},
            {InputName.FaceFrontLeftButton, OVRInput.Button.Three},
            {InputName.FaceFrontRightButton, OVRInput.Button.One},
            {InputName.FaceBackLeftButton, OVRInput.Button.Four},
            {InputName.FaceBackRightButton, OVRInput.Button.Two},
        };

        private GameManager m_gameManager = null;
        private Dictionary<InputName, TrackedInput> m_inputStates = new Dictionary<InputName, TrackedInput>();
        private OVRManager m_ovrManager = null;

        #endregion

        #region Interface Properties

        public IReadOnlyDictionary<InputName, TrackedInput> InputStates
        {
            get { return m_inputStates; }
        }

        #endregion

        #region Interface Methods

        public void InitializeFromGameManager(GameManager gameManager)
        {
            InitializeFromGameManagerInternal(gameManager);
        }

        #endregion

        #region Private Methods

        private void InitializeFromGameManagerInternal(GameManager gameManager)
        {
            SetStatus(StandardStatus.INITIALIZATION_RUNNING);

            m_gameManager = gameManager;
            m_ovrManager = gameObject.AddComponent<OVRManager>();

            foreach (var inputMapping in m_inputToOVRInputMapping)
            {
                m_inputStates.Add(inputMapping.Key, new TrackedInput(inputMapping.Value));
            }

            SetInitialized();
        }

        private void Update()
        {
            if (!IsInitialized)
            {
                return;
            }

            UpdateInputStates();
        }

        private void UpdateInputStates()
        {
            if(m_ovrManager == null)
            {
                Log("Cannot update input states because OVRManager is not available!", LogType.Error);
                return;
            }

            foreach (var inputState in m_inputStates)
            {
                float previousValue = inputState.Value.Value;

                if (inputState.Value.OVRInputMapping.GetType() == typeof(OVRInput.Button))
                {
                    bool currentValueBool = OVRInput.Get((OVRInput.Button)inputState.Value.OVRInputMapping, OVRInput.Controller.All);

                    if (currentValueBool == true)
                    {
                        inputState.Value.Value = 1f;
                    }
                    else
                    {
                        inputState.Value.Value = 0f;
                    }
                }
                else if (inputState.Value.OVRInputMapping.GetType() == typeof(OVRInput.Axis1D))
                {
                    inputState.Value.Value = OVRInput.Get((OVRInput.Axis1D)inputState.Value.OVRInputMapping, OVRInput.Controller.All);
                }
                else
                {
                    SetStatus($"Unsupported input type {inputState.Value.OVRInputMapping.GetType()}", LogType.Error);
                    continue;
                }

                float currentValue = inputState.Value.Value; // RPB: Readability

                if (currentValue != previousValue)
                {
                    inputState.Value.OnValueChanged.Invoke(currentValue);
                }

                if (currentValue > THRESHOLD_BUTTON_DOWN && !inputState.Value.IsDown)
                {
                    inputState.Value.IsDown = true;
                    inputState.Value.OnInputDown.Invoke();
                }
                else if (currentValue < THRESHOLD_BUTTON_UP && inputState.Value.IsDown)
                {
                    inputState.Value.IsDown = false;
                    inputState.Value.OnInputUp.Invoke();
                }
            }
        }

        #endregion

    }
}