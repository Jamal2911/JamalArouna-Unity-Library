using UnityEngine;

namespace JamalArouna.Library.Utilities.Components
{
    public class HandleRenderersOnStart : MonoBehaviour
    {
    
        private enum Behavior
        {
            ShowRenderers,
            DisableRenderers,
            DestroyRenderers,
        }
    
        [SerializeField] private Behavior startBehavior = Behavior.DestroyRenderers;
    
        private void Start()
        {
            if(startBehavior == Behavior.ShowRenderers) return;
        
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                if(r == null) continue;
                switch (startBehavior)
                {
                    case Behavior.DisableRenderers:
                        r.enabled = false;
                        break;
                    case Behavior.DestroyRenderers:
                        Destroy(r);
                        break;
                }
            }
        }
    }
}
