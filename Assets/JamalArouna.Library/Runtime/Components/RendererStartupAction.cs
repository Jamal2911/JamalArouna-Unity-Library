using UnityEngine;

namespace JamalArouna.Library.Components
{
    public sealed class RendererStartupAction : MonoBehaviour
    {
        private enum StartupAction
        {
            ShowRenderers,
            DisableRenderers,
            DestroyRenderers,
        }
    
        [SerializeField] private StartupAction action = StartupAction.DestroyRenderers;
    
        private void Start()
        {
            if (action == StartupAction.ShowRenderers)
                return;
        
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer == null)
                    continue;

                switch (action)
                {
                    case StartupAction.DisableRenderers:
                        renderer.enabled = false;
                        break;
                    case StartupAction.DestroyRenderers:
                        Destroy(renderer);
                        break;
                }
            }
        }
    }
}
