using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PearlGreySoftware
{
    public class ShooterGameManager : PearlBehaviour
    {

        // TODO-RPB: Add PearlBehaviour statuses
        // TODO-RPB: Do we need an input interface?

        #region Private Constants

        // RPB: Coordinate system:
        // Center X is 0, Center Y is 0
        // X increases left to right
        // Y increases bottom to top

        private const float FIELD_SIZE_X = 12f;
        private const float FIELD_SIZE_Y = 16f;
        private const float BULLET_COOLDOWN_TIME_SECONDS = 0.5f;

        #endregion

        #region Private Fields

        private List<ShooterGameEntity> m_gameEntities = new List<ShooterGameEntity>();
        private PlayerShipEntity m_playerShipEntity = new PlayerShipEntity();

        private float m_playerShipSteeringInput = 0f;
        private bool m_overrideWithKeyboardControls = false;
        private bool m_shootingInputOn = false;
        private float m_timeSinceLastBulletFired = 0f;

        #endregion

        #region Public Properties

        // TODO-RPB: Make this an IReadOnlyList but with Contains()
        public List<ShooterGameEntity> ActiveGameEntities
        {
            get { return m_gameEntities; }
        }

        #endregion

        #region Public Methods

        public void SetShipControlVelocity(float newVelocityX)
        {
            if (m_overrideWithKeyboardControls)
            {
                return;
            }

            m_playerShipSteeringInput = newVelocityX;
        }

        public void SetShootingOn()
        {
            if (m_overrideWithKeyboardControls)
            {
                return;
            }

            m_shootingInputOn = true;
        }

        public void SetShootingOff()
        {
            if (m_overrideWithKeyboardControls)
            {
                return;
            }

            m_shootingInputOn = false;
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            m_gameEntities.Add(m_playerShipEntity);

            //TODO-RPB: Make "Initialize" function for all entities, and move this into it
            m_playerShipEntity.Position = new Vector2(0f, -7f);
        }

        private void Update()
        {
            if (m_overrideWithKeyboardControls)
            {
                UpdateKeyboardControls();
            }

            UpdatePlayerShipInputs();
            UpdateEntities();
            UpdateShooting();
        }

        private void UpdateEntities()
        {
            // TODO-RPB: Add calculations to kill enemies if bullet hits them
            // TODO-RPB: Add calculations to prune bullets/bombs that go off-screen

            for(int i = 0; i < m_gameEntities.Count; i++)
            {
                var currentEntity = m_gameEntities[i];
                currentEntity.UpdatePosition(Time.deltaTime);
            }
        }

        private void UpdateKeyboardControls()
        {
            if(Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                m_playerShipSteeringInput = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                m_playerShipSteeringInput = 1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow) == Input.GetKey(KeyCode.LeftArrow))
            {
                m_playerShipSteeringInput = 0f;
            }

            m_shootingInputOn = Input.GetKey(KeyCode.Space);

        }

        private void UpdateShooting()
        {
            if (m_shootingInputOn)
            {
                if ((Time.time - BULLET_COOLDOWN_TIME_SECONDS) > m_timeSinceLastBulletFired)
                {
                    m_timeSinceLastBulletFired = Time.time;
                    var bulletStartPosition = m_playerShipEntity.Position;
                    var bullet = new BulletEntity();
                    bullet.Position = m_playerShipEntity.Position;
                    bullet.Velocity = new Vector2(0f, 2f);
                    m_gameEntities.Add(bullet);
                }
                else
                {
                    // RPB: Do not shoot because we are within the cooldown time.
                }
            }
        }

        private void UpdatePlayerShipInputs()
        {
            m_playerShipEntity.SetSteeringInput(m_playerShipSteeringInput);
        }

        #endregion

    }
}