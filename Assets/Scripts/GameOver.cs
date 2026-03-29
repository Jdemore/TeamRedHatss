using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverUI;
    //public PointSystem pointSystem;

    //public TMP_Text points;
    //public TMP_Text lives;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //points.text = "Points: " + pointSystem.points;
        //lives.text = "Lives Left: " + pointSystem.lives;

        Vector3 spawnPos = new Vector3(-10.38733f, 9.77538f, -20.27686f);
        Instantiate(GameOverUI, spawnPos, Quaternion.identity);
    }

    public void restartGame() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
