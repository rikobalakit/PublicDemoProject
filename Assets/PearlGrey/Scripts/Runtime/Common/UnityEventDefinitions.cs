using System;
using UnityEngine;
using UnityEngine.Events;

namespace PearlGreySoftware
{
    #region Public Classes

    [Serializable]
    public class HandVoidEvent : UnityEvent<PlayerHandController> { }

    [Serializable]
    public class FloatEvent: UnityEvent<float> { }

    [Serializable]
    public class VoidEvent : UnityEvent { }

    #endregion
}