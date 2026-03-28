using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Console.Log("Quitting");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}