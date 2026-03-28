/*
 * This script is the MonoBehaviour Wrapper for the SceneManager
 * This will allow laoding of scenes with simple calls and buttons
 * Previous/Next    | no variables
 * ByIndex          | Int
 * ByName           | String
*/

using UnityEngine;

namespace SceneMgmt
{
public class Loader : MonoBehaviour
{
    [SerializeField] private LoadMode loadMode;
    [SerializeField] private int buildIndex;
    [SerializeField] private string sceneName;

    public enum LoadMode
    {
        Next,
        Previous,
        ByIndex,
        ByName
    }

    public void Load()
    {
        Console.Log("Loading");
        switch (loadMode)
        {
            case LoadMode.Next:
                Manager.LoadNext();
                break;

            case LoadMode.Previous:
                Manager.LoadPrevious();
                break;

            case LoadMode.ByIndex:
                Manager.LoadByIndex(buildIndex);
                break;

            case LoadMode.ByName:
                Manager.LoadByName(sceneName);
                break;
        }
    }
}
}