/*
 * Bootstrapper - Boot scene initializer.
 *
 * Place in a lightweight "Boot" scene (build index 0). Initializes
 * global services and persistent managers, then loads the first real scene.
 *
 * Pattern:
 *   Boot (index 0) initializes everything > loads MainMenu or Gameplay.
 *
 * Usage:
 *   1. Create a "Boot" scene with a single GameObject holding this component.
 *   2. Assign persistent manager prefabs in the Inspector.
 *   3. Set the first scene to load.
 */

using System.Collections;
using UnityEngine;

namespace Toolkit.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Persistent Managers")]
        [Tooltip("Prefabs instantiated once and marked DontDestroyOnLoad.")]
        [SerializeField] private GameObject[] _persistentPrefabs;

        [Header("Loading")]
        [SerializeField] private string _loadingScene = "_InitLoading";
        [SerializeField] private float _minimumLoadingTime = 5f;

        [Header("First Scene")]
        [SerializeField] private string _startScene = "_StartScreen";

        private IEnumerator Start()
        {
            // Spawn persistent managers
            for (int i = 0; i < _persistentPrefabs.Length; i++)
            {
                GameObject prefab = _persistentPrefabs[i];
                if (prefab == null) continue;

                GameObject instance = Instantiate(prefab);
                DontDestroyOnLoad(instance);
            }

            yield return null;

            // Load loading screen additively
            yield return SceneMgmt.Manager.LoadAdditiveByName(_loadingScene);

            LoadingScreenFade fade = LoadingScreenFade.Instance;

            float fadeTime = fade != null ? fade.FadeDuration : 0f;

            // Fade in
            if (fade != null)
                yield return fade.FadeIn();

            float timer = 0f;

            // Wait until fade-out should begin
            while (timer < _minimumLoadingTime - fadeTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // Fade out
            if (fade != null)
                yield return fade.FadeOut();

            // Ensure full minimum time elapsed
            while (timer < _minimumLoadingTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            yield return SceneMgmt.Manager.Unload(_loadingScene);

            SceneMgmt.Manager.LoadByName(_startScene);
        }
    }
}