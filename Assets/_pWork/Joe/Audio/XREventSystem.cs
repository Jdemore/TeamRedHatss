/*
 * XREventSystem - Persistent EventSystem for XR UI interaction.
 *
 * Ensures exactly one EventSystem + XRUIInputModule exists across
 * scene loads. Prevents duplicates via singleton pattern.
 *
 * Setup:
 *   1. Create a prefab with this component.
 *   2. Add EventSystem and XRUIInputModule to the same GameObject.
 *   3. Assign to Bootstrapper._persistentPrefabs (becomes DontDestroyOnLoad).
 */

using UnityEngine;
using UnityEngine.EventSystems;

namespace Toolkit.UI
{
    [RequireComponent(typeof(EventSystem))]
    public class XREventSystem : Debugger
    {
        public static XREventSystem Instance { get; private set; }

        protected override string LogCategory => "UI";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Log("Duplicate EventSystem destroyed");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
