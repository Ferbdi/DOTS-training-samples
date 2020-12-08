﻿using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class CreateBuildingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Generate();

        Enabled = false;
    }

    private void Generate()
    {
        List<Point> pointsList = new List<Point>();
        List<Bar> barsList = new List<Bar>();
        
        Entity buildingEntity = new Entity();
        
        // buildings
        for (int i = 0; i < 35; i++)
        {
            int height = Random.Range(4, 12);
            Vector3 pos = new Vector3(Random.Range(-45f, 45f), 0f, Random.Range(-45f, 45f));
            float spacing = 2f;
            for (int j = 0; j < height; j++)
            {
                Point point = new Point();
                point.x = pos.x + spacing;
                point.y = j * spacing;
                point.z = pos.z - spacing;
                point.oldX = point.x;
                point.oldY = point.y;
                point.oldZ = point.z;
                if (j == 0)
                {
                    point.anchor = true;
                }

                pointsList.Add(point);
                point = new Point();
                point.x = pos.x - spacing;
                point.y = j * spacing;
                point.z = pos.z - spacing;
                point.oldX = point.x;
                point.oldY = point.y;
                point.oldZ = point.z;
                if (j == 0)
                {
                    point.anchor = true;
                }

                pointsList.Add(point);
                point = new Point();
                point.x = pos.x + 0f;
                point.y = j * spacing;
                point.z = pos.z + spacing;
                point.oldX = point.x;
                point.oldY = point.y;
                point.oldZ = point.z;
                if (j == 0)
                {
                    point.anchor = true;
                }

                pointsList.Add(point);
            }
        }

        // ground details
        for (int i = 0; i < 600; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-55f, 55f), 0f, Random.Range(-55f, 55f));
            Point point = new Point();
            point.x = pos.x + Random.Range(-.2f, -.1f);
            point.y = pos.y + Random.Range(0f, 3f);
            point.z = pos.z + Random.Range(.1f, .2f);
            point.oldX = point.x;
            point.oldY = point.y;
            point.oldZ = point.z;
            pointsList.Add(point);

            point = new Point();
            point.x = pos.x + Random.Range(.2f, .1f);
            point.y = pos.y + Random.Range(0f, .2f);
            point.z = pos.z + Random.Range(-.1f, -.2f);
            point.oldX = point.x;
            point.oldY = point.y;
            point.oldZ = point.z;
            if (Random.value < .1f)
            {
                point.anchor = true;
            }

            pointsList.Add(point);
        }

        int batch = 0;

        for (int i = 0; i < pointsList.Count; i++)
        {
            for (int j = i + 1; j < pointsList.Count; j++)
            {
                Bar bar = new Bar();
                bar.AssignPoints(pointsList[i], pointsList[j]);
                if (bar.length < 5f && bar.length > .2f)
                {
                    bar.point1.neighborCount++;
                    bar.point2.neighborCount++;

                    barsList.Add(bar);
                }
            }
        }

        var points = new Point[barsList.Count * 2];
        var pointCount = 0;
        for (int i = 0; i < pointsList.Count; i++)
        {
            if (pointsList[i].neighborCount > 0)
            {
                points[pointCount] = pointsList[i];
                pointCount++;
            }
        }

        Debug.Log(pointCount + " points, room for " + points.Length + " (" + barsList.Count + " bars)");

        var bars = barsList.ToArray();
        
        // matProps = new MaterialPropertyBlock[barsList.Count];
        // Vector4[] colors = new Vector4[instancesPerBatch];
        // for (int i = 0; i < barsList.Count; i++)
        // {
        //     colors[i % instancesPerBatch] = barsList[i].color;
        //     if ((i + 1) % instancesPerBatch == 0 || i == barsList.Count - 1)
        //     {
        //         MaterialPropertyBlock block = new MaterialPropertyBlock();
        //         block.SetVectorArray("_Color", colors);
        //         matProps[i / instancesPerBatch] = block;
        //     }
        // }

        System.GC.Collect();
    }
}