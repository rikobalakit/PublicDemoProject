using System;
using UnityEngine;

namespace PearlGreySoftware
{
    [Serializable]
    public class PlayerShipEntity : ShipEntity
    {
        private const float PLAYER_SHIP_POSITION_X_MIN = -6f;
        private const float PLAYER_SHIP_POSITION_X_MAX = 6f;
        private const float PLAYER_SHIP_VELOCITY_MULTIPLIER = 3f;
        private const float PLAYER_SHIP_POSITION_Y_FIXED = -7f;

        public void SetSteeringInput(float newInput)
        {
            Velocity = new Vector2(PLAYER_SHIP_VELOCITY_MULTIPLIER * newInput, 0f);
        }
    }
}