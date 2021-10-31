using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public static List<GameObject> validChunks(GameObject[] objects, int player_x, int player_y, int r, out List<Tuple<int,int>> missing_positions)
    {
        List<GameObject> valid = new List<GameObject>();
        List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

        for(int i = -r; i <= r; i++)
            for(int j = -r; j <= r; j++)
                pairs.Add(Tuple.Create(i, j));

        foreach(GameObject obj in objects)
        {
            int chunk_x = (int) (obj.transform.position.x/10);
            int chunk_y = (int) (obj.transform.position.y/10);

            foreach(Tuple<int,int> pair in pairs)
            {

                if((player_x + pair.Item1 == chunk_x) && (player_y + pair.Item2 == chunk_y))
                {
                    valid.Add(obj);
                    pairs.Remove(pair);
                }
            }
        }
        missing_positions = pairs;
        return valid;
    }
}
