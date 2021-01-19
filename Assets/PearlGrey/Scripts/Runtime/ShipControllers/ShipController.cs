using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PearlGreySoftware
{
    public class ShipController : PearlBehaviour
    {
        [SerializeField]
        private InteractiveSteeringWheel m_steeringWheel = null;

        [SerializeField]
        private Text m_tempShootingStatusLabel = null;

        [SerializeField]
        private Text m_tempSpecialWeaponStatusLabel = null;

        private void Start()
        {
            m_steeringWheel.OnSpecialWeaponTriggered.AddListener(OnSpecialWeaponTriggered);
            m_steeringWheel.OnShootingStarted.AddListener(OnShootingStarted);
            m_steeringWheel.OnShootingEnded.AddListener(OnShootingEnded);

            m_tempShootingStatusLabel.text = "Main Gun IDLE";
            m_tempSpecialWeaponStatusLabel.text = string.Empty;
        }

        private void OnSpecialWeaponTriggered()
        {
            StartCoroutine(TempSpecialWeaponText());
        }

        private void OnShootingStarted()
        {
            m_tempShootingStatusLabel.text = "Main Gun ACTIVE";
        }

        private void OnShootingEnded()
        {
            m_tempShootingStatusLabel.text = "Main Gun IDLE";
        }

        private IEnumerator TempSpecialWeaponText()
        {
            m_tempSpecialWeaponStatusLabel.text = "SPECIAL WEAPON TRIGGERED";
            yield return new WaitForSeconds(1f);
            m_tempSpecialWeaponStatusLabel.text = string.Empty;
        }
    }
}