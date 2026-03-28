using UnityEngine;

public class collision : MonoBehaviour
{
    public PointSystem pointSystem;

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("correct")) 
        {
            Debug.Log("Correct");
            // add health, move on
            pointSystem.getPoints();    // increase points

            Debug.Log("Points: " + pointSystem.points);
            Debug.Log("Lives: " + pointSystem.lives);

        } else if (other.gameObject.CompareTag("incorrect")) 
        {
            Debug.Log("Incorrect");
            pointSystem.loseLife(); // decrease lives
            if(pointSystem.lives == 0) {
                // do something
            }

            Debug.Log("Points: " + pointSystem.points);
            Debug.Log("Lives: " + pointSystem.lives);

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
