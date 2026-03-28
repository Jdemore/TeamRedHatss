/*
 * RigSpawnHandler - Moves the persistent XR Rig to the scene's SpawnPoint on load.
 *
 * Attach to the XR Origin (XR Rig) prefab. Listens for scene loads and
 * teleports the rig to the SpawnPoint found in the new scene.
 *
 * Setup:
 *   1. Add this component to your persistent XR Rig prefab.
 *   2. Place a SpawnPoint in every gameplay scene.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toolkit.Core
{
    public class RigSpawnHandler : Debugger
    {
        protected override string LogCategory => "Spawn";

        [Header("Settings")]
        [Tooltip("Delay one frame after scene load so SpawnPoint.OnEnable runs first.")]
        [SerializeField] private bool _waitOneFrame = true;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Skip additive loads (loading screens, overlays)
            if (mode == LoadSceneMode.Additive) return;

            if (_waitOneFrame)
                StartCoroutine(MoveNextFrame(scene.name));
            else
                MoveToSpawn(scene.name);
        }

        private System.Collections.IEnumerator MoveNextFrame(string sceneName)
        {
            yield return null;
            MoveToSpawn(sceneName);
        }

        private void MoveToSpawn(string sceneName)
        {
            SpawnPoint spawn = SpawnPoint.Current;

            if (spawn == null)
            {
                Warn($"No SpawnPoint found in '{sceneName}' -- rig stays in place");
                return;
            }

            Transform rig = transform;
            Transform target = spawn.transform;

            // Reset rig position and rotation to match spawn point
            rig.position = target.position;
            rig.rotation = target.rotation;

            Log($"Rig moved to SpawnPoint in '{sceneName}' at {target.position}");
        }
    }
}
