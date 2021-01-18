using System.Collections.Generic;
using UnityEngine;

namespace PearlGreySoftware
{
    public interface IInputManager
    {

        #region Interface Properties

        bool IsInitialized
        {
            get;
        }

        IReadOnlyDictionary<InputName, TrackedInput> InputStates
        {
            get;
        }

        #endregion

        #region Interface Methods

        void InitializeFromGameManager(GameManager gameManager);

        #endregion

    }
}