using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkNoiseGenerator : MonoBehaviour
{
    public float[,] GetNoiseMap
    (
        int stride,
        System.Random random,
        float? topLeft = null, 
        float? topRight = null, 
        float? botLeft = null,
        float? botRight = null
    )
    {
        float[,] baseMap = new float[stride, stride];
        InitializeNodes(baseMap, random, stride, topLeft, topRight, botLeft, botRight);

        LayerNoise(baseMap,2, 2, stride, random);

        return baseMap;
    }

    private void LayerNoise
    (
        float[,] map, 
        int initLevel, 
        int level, 
        int stride,
        System.Random random,
        bool copyLeft = false,
        bool copyTop = false
    )
    {
        if (stride <= 0)
        {
            Debug.Log("STRIDE IS ZERO");
            return;
        }
        
        //pre order descent. Lerp parent
        LerpNoise(map, stride);

        if (level <= 0 || stride % 2 != 1)
            return;
        //add child layer.
        
        int subDivisionStride = stride / (initLevel - level + 1);
        int subDivisionCount = (initLevel != level) ? 2 : 1;
        int lastSubDivisionIndex = subDivisionCount-1;

        int endpointChildRange = subDivisionStride - 1;

        int endpointParentRange = stride - 1;
        
        for (int subY = 0; subY < subDivisionCount; subY++)
        {
            for (int subX = 0; subX < subDivisionCount; subX++)
            {
                int baseX = subDivisionStride * (lastSubDivisionIndex - subX);
                int baseY = subDivisionStride * subY;

                int wholeStrideOffsetX = subDivisionStride - baseX;
                int wholeStrideOffsetY = baseY + subDivisionStride;
                
                bool isTopLeft = subY == 0 && subX == 0;
                bool isTopRight = subY == 0 && subX == lastSubDivisionIndex;
                bool isBotLeft = subY == lastSubDivisionIndex && subX == 0;

                bool isNowCopyingLeft = subX != 0;
                bool isNowCopyingTop = subY != 0;
                
                float topLeft = map[baseX, baseY];
                float? topRight = 
                    (isTopLeft && !copyTop) 
                    ? null 
                    : (float?)map[wholeStrideOffsetX, 0];
                float? botLeft = 
                    (isTopLeft && !copyLeft) 
                    ? null 
                    : (float?)map[0, wholeStrideOffsetY];
                float? botRight = 
                    (isTopLeft || isBotLeft || isTopRight) 
                    ? null 
                    : (float?)map[wholeStrideOffsetX, wholeStrideOffsetY];

                float[,] subDivision = new float[subDivisionStride,subDivisionStride];
                
                InitializeNodes(subDivision, random, subDivisionStride, topLeft, topRight, botLeft, botRight);
                
                LayerNoise
                (
                    subDivision,
                    initLevel,
                    level -1,
                    subDivisionStride,
                    random,
                    isNowCopyingLeft,
                    isNowCopyingTop
                );

                for (int sy = 0; sy < subDivisionStride; sy++)
                {
                    for (int sx = 0; sx < subDivisionStride; sx++)
                    {
                        int flipY = subDivisionStride - 1 - sy;
                        int flipX = subDivisionStride - 1 - sx;
                        int flipMapY = stride - 1 - (baseY + sy);
                        int flipMapX = stride - 1 - (baseY + sx);
                        float levelDivisor = Mathf.Pow(2, (initLevel - level));
                        map[baseX+sx, flipMapY] += subDivision[sx,flipY] / levelDivisor;
                        //map[baseX + sx, baseY + sy] += subDivision[sx, sy] / levelDivisor;
                    }
                }
            }
        }
    }

    private void InitializeNodes
    (
        float[,] map, 
        System.Random random, 
        int stride,
        float? topLeft = null,
        float? topRight = null,
        float? botLeft = null,
        float? botRight = null
    )
    {
        int nodePoint = stride - 1;
        //Debug.Log($"STRIDE: {stride}");
        map[0, 0] = topLeft ?? (float)(random.NextDouble() - 0.5);
        map[0, nodePoint] = topRight ?? (float)(random.NextDouble() - 0.5);
        map[nodePoint, 0] = botLeft ?? (float)(random.NextDouble() - 0.5);
        map[nodePoint, nodePoint] = botRight ?? (float)(random.NextDouble() - 0.5);
    }

    private void LerpNoise
    (
        float[,] map,
        int stride
    )
    {
        int nodePoint = stride - 1;
        
        //node positions.
        float topL = map[0, 0];
        float topR = map[nodePoint, 0];
        float bottomL = map[0, nodePoint];
        float bottomR = map[nodePoint, nodePoint];

        //get vectors for distance weights
        Vector2 xy_topL = new Vector2(0, 0);
        Vector2 xy_topR = new Vector2(nodePoint, 0);
        Vector2 xy_bottomL = new Vector2(0, nodePoint);
        Vector2 xy_bottomR = new Vector2(nodePoint,nodePoint);

        //the maximum distance a node can be from a point.
        float maxDist = Vector2.Distance(new Vector2(0, 0), new Vector2(nodePoint, nodePoint));

        for (int y = 0; y < stride; y++)
        {
            for (int x = 0; x < stride; x++)
            {
                //if (CheckIfShouldSkip(x, y, seamTop, seamRight, seamBottom, seamLeft, stride))
                //    continue;
                
                Vector2 point = new Vector2(x, y);

                float dist_topL = Vector2.Distance(point, xy_topL);
                float dist_topR = Vector2.Distance(point, xy_topR);
                float dist_bottomR = Vector2.Distance(point, xy_bottomL);
                float dist_bottomL = Vector2.Distance(point, xy_bottomR);

                float weight_topL = topL * dist_topL / maxDist;
                float weight_topR = topR * dist_topR / maxDist;
                float weight_bottomL = bottomL * dist_bottomL / maxDist;
                float weight_bottomR = bottomR * dist_bottomR / maxDist;

                float sum =
                    weight_topL
                    +
                    weight_topR
                    +
                    weight_bottomL
                    +
                    weight_bottomR;

                float avg = sum / (4);

                map[x, y] = avg;
            }
        }
    }
}
