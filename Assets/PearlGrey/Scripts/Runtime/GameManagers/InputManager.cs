using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace PearlGreySoftware
{

    public class InputManager : PearlBehaviour
    {

        #region Private Constants

        private const float THRESHOLD_BUTTON_DOWN = 0.75f;
        private const float THRESHOLD_BUTTON_UP = 0.25f;

        #endregion

        #region Private Classes

        public class TrackedInputs
        {
            public float Value;
            public FloatEvent OnValueChanged = new FloatEvent();
            public VoidEvent OnInputDown = new VoidEvent();
            public VoidEvent OnInputUp = new VoidEvent();
            public bool IsDown = false;
        }

        #endregion

        #region Private Fields

        private OVRInput.Button[] m_trackedButtons = new OVRInput.Button[]
        {
            OVRInput.Button.One,
            OVRInput.Button.Two,
            OVRInput.Button.Three,
            OVRInput.Button.Four
        };

        private OVRInput.Axis1D[] m_trackedTriggers = new OVRInput.Axis1D[]
        {
            OVRInput.Axis1D.PrimaryHandTrigger,
            OVRInput.Axis1D.SecondaryHandTrigger,
            OVRInput.Axis1D.PrimaryIndexTrigger,
            OVRInput.Axis1D.SecondaryIndexTrigger
        };


        private GameManager m_gameManager = null;
        private Dictionary<object, TrackedInputs> m_inputStates = new Dictionary<object, TrackedInputs>();

        #endregion

        #region Public Properties

        public IReadOnlyDictionary<object, TrackedInputs> InputStates
        {
            get { return m_inputStates; }
        }

        #endregion

        #region Public Methods

        public void InitializeFromGameManager(GameManager gameManager)
        {
            InitializeFromGameManagerInternal(gameManager);
        }

        #endregion

        #region Private Methods

        private void InitializeFromGameManagerInternal(GameManager gameManager)
        {
            m_gameManager = gameManager;

            for (int i = 0; i < m_trackedButtons.Length; i++)
            {
                m_inputStates.Add(m_trackedButtons[i], new TrackedInputs());
            }

            for (int i = 0; i < m_trackedTriggers.Length; i++)
            {
                m_inputStates.Add(m_trackedTriggers[i], new TrackedInputs());
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
            if(m_gameManager.OVRManager == null)
            {
                SetStatus("Cannot update input states because OVRManager is not available!");
                return;
            }

            foreach (var inputState in m_inputStates)
            {
                float previousValue = inputState.Value.Value;

                if (inputState.Key.GetType() == typeof(OVRInput.Button))
                {
                    bool currentValueBool = OVRInput.Get((OVRInput.Button)inputState.Key, OVRInput.Controller.All);

                    if (currentValueBool == true)
                    {
                        inputState.Value.Value = 1f;
                    }
                    else
                    {
                        inputState.Value.Value = 0f;
                    }
                }
                else if (inputState.Key.GetType() == typeof(OVRInput.Axis1D))
                {
                    inputState.Value.Value = OVRInput.Get((OVRInput.Axis1D)inputState.Key, OVRInput.Controller.All);
                }
                else
                {
                    SetStatus($"Unsupported input type {inputState.Key.GetType()}", LogType.Error);
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