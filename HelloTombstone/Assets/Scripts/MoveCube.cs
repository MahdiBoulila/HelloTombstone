using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 0.2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){ 
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(xDirection, 0.0f, yDirection);
        transform.position += moveDirection * speed;
        
    }
}
