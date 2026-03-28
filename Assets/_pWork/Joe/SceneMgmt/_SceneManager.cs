/*
 * This script simplifies the loading process in-game via static class
 * This will allow laoding of scenes with simple calls
 * Previous/Next    | no variables
 * ByIndex          | Int
 * ByName           | String
*/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneMgmt
{
    public static class Manager
    {
        // Standard loading fn's

        public static void LoadByIndex(int buildIndex)
        {
            if (!IsValidIndex(buildIndex)) return;
            SceneManager.LoadScene(buildIndex);
        }

        public static void LoadByName(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            SceneManager.LoadScene(sceneName);
        }

        public static void LoadNext()
        {
            int next = SceneManager.GetActiveScene().buildIndex + 1;
            if (next >= SceneManager.sceneCountInBuildSettings)
                next = 0;

            LoadByIndex(next);
        }

        public static void LoadPrevious()
        {
            int prev = SceneManager.GetActiveScene().buildIndex - 1;
            if (prev < 0)
                prev = SceneManager.sceneCountInBuildSettings - 1;

            LoadByIndex(prev);
        }

        // Async loading fn's

        public static AsyncOperation LoadAsyncByIndex(int buildIndex)
        {
            if (!IsValidIndex(buildIndex)) return null;
            return SceneManager.LoadSceneAsync(buildIndex);
        }

        public static AsyncOperation LoadAsyncByName(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return null;
            return SceneManager.LoadSceneAsync(sceneName);
        }

        // Additive Loading

        public static AsyncOperation LoadAdditiveByName(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return null;
            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public static AsyncOperation LoadAdditiveByIndex(int buildIndex)
        {
            if (!IsValidIndex(buildIndex)) return null;
            return SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        }

        public static AsyncOperation Unload(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return null;
            return SceneManager.UnloadSceneAsync(sceneName);
        }

        // Loading Screen

        public static AsyncOperation LoadWithLoadingScene(
            string targetScene,
            string loadingScene)    
        {
            if (string.IsNullOrEmpty(targetScene)) return null;
            if (string.IsNullOrEmpty(loadingScene)) return null;

            SceneManager.LoadScene(loadingScene);

            return SceneManager.LoadSceneAsync(targetScene);
        }


        private static bool IsValidIndex(int index)
        {
            return index >= 0 &&
                   index < SceneManager.sceneCountInBuildSettings;
        }
    }
}