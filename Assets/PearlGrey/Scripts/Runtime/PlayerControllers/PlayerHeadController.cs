using UnityEngine;

namespace PearlGreySoftware
{

    public class PlayerHeadController : PlayerBodyPartController
    {

        #region Private Methods

        protected new void Start()
        {
            base.Start();
            SetInitialized();
        }

        #endregion

    }

}