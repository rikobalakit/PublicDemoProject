using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PearlGreySoftware
{
    public class ShooterGameCanvasDrawer : PearlBehaviour
    {

        // RPB: Ultimately, this class should treat the ShooterGameManager as READ ONLY
        // The ShooterGameManager should never care about the existence of its 'monitors'
        // TODO-RPB: Make the "ShooterGameWorldDrawer" that mirrors this but draws entities out in space
        // TODO-RPB: Is there a reason to make a parent class of this and the above comment's class?
        // TODO-RPB: Add PearlBehaviour statuses

        #region Private Fields

        private float m_managerSpaceToCanvasSpaceMultiplier = 100f;

        [SerializeField]
        private ShooterGameManager m_shooterGameManager = null;

        [SerializeField]
        private Canvas m_baseCanvas = null;

        [SerializeField]
        private Image m_playerShipImageReference = null;

        [SerializeField]
        private Image m_enemyShip1ImageReference = null;

        [SerializeField]
        private Image m_enemyShip2ImageReference = null;

        [SerializeField]
        private Image m_enemyShip3ImageReference = null;

        [SerializeField]
        private Image m_playerBulletImageReference = null;

        [SerializeField]
        private Image m_playerBombImageReference = null;

        private Dictionary<ShooterGameEntity, Image> m_entityToImageMap = new Dictionary<ShooterGameEntity, Image>();

        #endregion

        #region Private Methods

        private void Update()
        {
            UpdateEntityDrawingExistence();
            UpdateEntityDrawings();
        }

        private void UpdateEntityDrawingExistence()
        {
            var currentEntities = m_shooterGameManager.ActiveGameEntities;

            var newEntities = new List<ShooterGameEntity>();
            var deadEntities = new List<ShooterGameEntity>();

            foreach (var entity in currentEntities)
            {
                if (!m_entityToImageMap.ContainsKey(entity))
                {
                    newEntities.Add(entity);
                }
            }

            foreach (var mapping in m_entityToImageMap)
            {
                if (!currentEntities.Contains(mapping.Key))
                {
                    deadEntities.Add(mapping.Key);
                }
            }

            foreach (var newEntity in newEntities)
            {
                Image newEntityImage = null;

                // TODO-RPB: Set image depending on type of entity it is
                if (newEntity.GetType() == typeof(PlayerShipEntity))
                {
                    newEntityImage = Instantiate(m_playerShipImageReference);
                }
                else if (newEntity.GetType() == typeof(BulletEntity))
                {
                    newEntityImage = Instantiate(m_playerBulletImageReference);
                }
                else if (newEntity.GetType() == typeof(BombEntity))
                {
                    newEntityImage = Instantiate(m_playerBombImageReference);
                }
                else if (newEntity.GetType() == typeof(Enemy1Entity))
                {
                    newEntityImage = Instantiate(m_enemyShip1ImageReference);
                }
                else if (newEntity.GetType() == typeof(Enemy2Entity))
                {
                    newEntityImage = Instantiate(m_enemyShip2ImageReference);
                }
                else if (newEntity.GetType() == typeof(Enemy3Entity))
                {
                    newEntityImage = Instantiate(m_enemyShip3ImageReference);
                }


                newEntityImage.rectTransform.SetParent(m_baseCanvas.transform, false);
                m_entityToImageMap.Add(newEntity, newEntityImage);
            }

            foreach (var deadEntity in deadEntities)
            {
                Destroy(m_entityToImageMap[deadEntity]);
                m_entityToImageMap.Remove(deadEntity);
            }
        }

        private void UpdateEntityDrawings()
        {

            // TODO-RPB: Figure out the properties on Unity UI that cuts off images out of bounds.

            foreach(var mapping in m_entityToImageMap)
            {
                mapping.Value.rectTransform.anchoredPosition = mapping.Key.Position * m_managerSpaceToCanvasSpaceMultiplier;
            }
        }

        #endregion

    }
}