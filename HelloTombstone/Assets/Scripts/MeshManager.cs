using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour{
    // Start is called before the first frame update
    private float x_ini; // initial x coordinate
    private float y_ini; // initial y coordinate
    private float x_curr; // current x coordinate
    private float y_curr; // current y coordinate
    private int distance = 1; // distance between chunks in meters
    public GameObject Player;
    void Start(){
        x_ini = 0;
        y_ini = 0;
    }

    // Update is called once per frame
    void Update(){
        // gets current player's coordinates
        Vector3 currPos = Player.transform.position;
        x_curr = currPos.x;
        y_curr = currPos.y;
        // list of game objects, and list of vector3
        // List<GameObject> objList = ?;
        // List<Vector3> vectorList = ?;

        if (System.Math.Abs((int) x_curr - (int) x_ini) == distance
            || System.Math.Abs((int) y_curr - (int) y_ini) == distance){ // if the player is in another chunk
              // generate new chunks based on player's current position
            //   ??
        }

    }
}
