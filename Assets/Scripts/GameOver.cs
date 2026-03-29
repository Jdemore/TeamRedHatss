using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameOverUI.SetActive(true);
    }

    public void restartGame() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
