using System.Collections.Generic;
using UnityEngine;

namespace PearlGreySoftware
{
    public class SetMaterialOnRenderers : MonoBehaviour
    {
        [SerializeField]
        private List<Renderer> m_renderers = new List<Renderer>();

        public void SetMaterials(Material newMaterial)
        {
            SetMaterialsInternal(newMaterial);
        }

        private void SetMaterialsInternal(Material newMaterial)
        {
            for(int i = 0; i < m_renderers.Count; i++)
            {
                if(m_renderers[i] != null)
                {
                    m_renderers[i].material = newMaterial;
                }
            }
        }
    }
}