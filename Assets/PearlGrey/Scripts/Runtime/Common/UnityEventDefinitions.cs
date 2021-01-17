using UnityEngine;
using UnityEngine.Events;

namespace PearlGreySoftware
{
    [SerializeField]
    public class FloatEvent: UnityEvent<float>
    {

    }

    [SerializeField]
    public class VoidEvent : UnityEvent
    {

    }
}