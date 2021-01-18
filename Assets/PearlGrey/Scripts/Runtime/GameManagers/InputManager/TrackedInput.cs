using UnityEngine;
namespace PearlGreySoftware
{
    public class TrackedInput
    {

        #region Public Constructors

        public TrackedInput(object ovrInputMapping)
        {
            OVRInputMapping = ovrInputMapping;
        }

        #endregion

        #region Public Events

        public FloatEvent OnValueChanged = new FloatEvent();
        public VoidEvent OnInputDown = new VoidEvent();
        public VoidEvent OnInputUp = new VoidEvent();

        #endregion

        #region Public Fields

        public object OVRInputMapping;
        public float Value;
        public bool IsDown = false;

        #endregion
    }
}