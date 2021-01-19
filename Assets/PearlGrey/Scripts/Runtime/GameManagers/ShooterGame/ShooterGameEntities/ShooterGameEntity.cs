using System;
using UnityEngine;

namespace PearlGreySoftware
{
    [Serializable]
    public class ShooterGameEntity
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public void UpdatePosition(float lastFrameTimeSeconds)
        {
            Position += (Velocity * lastFrameTimeSeconds);
        }
    }
}

