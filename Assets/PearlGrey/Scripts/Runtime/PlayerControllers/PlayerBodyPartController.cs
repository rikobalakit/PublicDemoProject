using System.Collections;
using UnityEngine;
using UnityEngine.SpatialTracking;

namespace PearlGreySoftware
{

    public abstract class PlayerBodyPartController : PearlBehaviour
    {

        #region Protected Fields

        [SerializeField]
        protected TrackedPoseDriver m_trackedPoseDriver = default;

        #endregion

        #region Protected Methods

        protected void Start()
        {
            if (!VerifyComponents())
            {
                return;
            }
        }

        #endregion

        #region Private Methods

        private bool VerifyComponents()
        {
            if (m_trackedPoseDriver == null)
            {
                SetStatus("No Tracked Pose Driver found", LogType.Error);
                return false;
            }

            return true;
        }

        #endregion

    }
}