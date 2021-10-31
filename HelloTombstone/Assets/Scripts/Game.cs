using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
               
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
       
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "Player")
        {
            gameObject.SetActive(false);
            score++;
            Debug.Log("Score" + score);
            Vector3 temp = new Vector3(Random.Range(-10.6f, 10.6f), Random.Range(-10.6f, 10.6f), Random.Range(-10.6f, 10.6f));
            gameObject.transform.position = temp;
            gameObject.SetActive(true);
        }

   
    }
    // Update is called once per frame
    void Update()
    {
      
    }
}
