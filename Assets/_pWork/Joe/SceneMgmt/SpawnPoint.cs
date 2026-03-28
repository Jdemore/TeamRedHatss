/*
 * SpawnPoint - Marks where the XR Rig should be placed when a scene loads.
 *
 * Drop on an empty GameObject in each scene to define the player's
 * starting position and rotation. Only one per scene -- if multiples
 * exist the first found is used.
 *
 * Usage:
 *   1. Create an empty GameObject, name it "SpawnPoint".
 *   2. Add this component.
 *   3. Position and rotate it where the player should appear.
 */

using UnityEngine;

namespace Toolkit.Core
{
    public class SpawnPoint : Debugger
    {
        public static SpawnPoint Current { get; private set; }

        protected override string LogCategory => "Spawn";

        [Header("Gizmo")]
        [SerializeField] private float _gizmoRadius = 0.3f;
        [SerializeField] private Color _gizmoColor = new Color(0f, 1f, 0.6f, 0.8f);

        private void OnEnable()
        {
            if (Current != null && Current != this)
                Warn($"Multiple SpawnPoints in scene -- using {gameObject.name}, ignoring {Current.gameObject.name}");

            Current = this;
            Log($"SpawnPoint registered: {transform.position}");
        }

        private void OnDisable()
        {
            if (Current == this) Current = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireSphere(transform.position, _gizmoRadius);

            // Draw forward arrow
            Gizmos.DrawRay(transform.position, transform.forward * (_gizmoRadius * 2f));

            // Small up indicator for orientation
            Gizmos.color = new Color(_gizmoColor.r, _gizmoColor.g, _gizmoColor.b, 0.4f);
            Gizmos.DrawRay(transform.position, transform.up * _gizmoRadius);
        }
#endif
    }
}
