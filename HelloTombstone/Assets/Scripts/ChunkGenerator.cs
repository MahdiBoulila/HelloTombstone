using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public const int CHUNK_SIZE = 11;

    public ChunkNoiseGenerator noiseGenerator;
    public GameObject chunk;

    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random(1234);
        chunk.SetActive(false);

        float[,] initArea = noiseGenerator.GetNoiseMap(3*CHUNK_SIZE, random);

        //PrintMap(initArea, 3 * CHUNK_SIZE);
        /*
        float[,] chunkMapt = new float[CHUNK_SIZE, CHUNK_SIZE];
        for(int yt = 0; yt < CHUNK_SIZE; yt++)
            for(int xt = 0; xt < CHUNK_SIZE; xt++)
                chunkMapt[xt,yt] = initArea[xt*3, yt*3];

        GeneratePlane(0,0,chunkMapt);
        
        return;
        */
        
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                float[,] chunkMap = new float[CHUNK_SIZE, CHUNK_SIZE];

                int baseX = CHUNK_SIZE * x;
                int baseY = CHUNK_SIZE * y;

                for (int ly = 0; ly < CHUNK_SIZE; ly++)
                {
                    for (int lx = 0; lx < CHUNK_SIZE; lx++)
                    {
                        chunkMap[lx, ly] = initArea[baseX + lx, baseY + ly];
                    }
                }

                GeneratePlane(3 - 1 - x, 3 - 1 -y, chunkMap);
            }
        }
    }

    public GameObject GeneratePlane(int cx, int cy, float[,] map)
    {
        GameObject newChunk = Instantiate(chunk);
        
        MeshFilter meshFilter = newChunk.GetComponent<MeshFilter>();
        MeshCollider meshCollider = newChunk.GetComponent<MeshCollider>();
        
        Vector3[] vec = new Vector3[CHUNK_SIZE * CHUNK_SIZE];
        Vector3[] baseMesh = meshFilter.mesh.vertices;
        int meshCount = baseMesh.Length;
        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                int index = (x + (y * CHUNK_SIZE));
                float yWeight = map[CHUNK_SIZE - x - 1, y];
                //Debug.Log($"noise: {noise[x,y] * 100}");
                vec[index] = new Vector3(baseMesh[index].x, 0.5f * Mathf.Floor(map[x,y] * 20), baseMesh[index].z);
                //vec[index] = new Vector3(baseMesh[index].x, 20 * yWeight, baseMesh[index].z);
                //vec[index] = new Vector3(baseMesh[index].x, map[x, y] * 20, baseMesh[index].z);
            }
        }

        meshFilter.mesh.vertices = vec;
        meshFilter.sharedMesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();

        meshCollider.sharedMesh = meshFilter.mesh;

        newChunk.SetActive(true);

        newChunk.transform.position = new Vector3(10*cx,0,10*cy);
        
        return newChunk;
    }

    private void PrintMap(float[,] map, int stride)
    {
        string s = "[";
        for (int y = 0; y < stride; y++)
        {
            for (int x = 0; x < stride; x++)
            {
                s += $"({map[x,y]}),";
            }

            s += "]\n[";
        }
        
        Debug.Log(s);
    }
}
