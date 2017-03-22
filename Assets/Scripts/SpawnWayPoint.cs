using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class SpawnWayPoint : MonoBehaviour {

    CircularLevel[] levels;
    public GameObject prefab;

    private void Start()
    {
        Invoke("setWayPoint",1);
    }

    void setWayPoint()
    {
        levels = GameObject.FindObjectsOfType<CircularLevel>();

        foreach (CircularLevel level in levels)
        {
            float radius = level.InnerRadius + level.Thickness;
            CircularSegment[] segments = level.GetComponentsInChildren<CircularSegment>();
            float[] angles = new float[segments.Length * 2];
            for (int i = 0; i < segments.Length; i++)
            {
                angles[i * 2] = segments[i].AngleStart/2.0f;
                angles[i * 2 + 1] = segments[i].AngleEnd/2.0f;
            }
            print("level = " + level);
            foreach (float f in angles)
                print("angles = " + f);
            spawnWayPoint(angles, 12, radius, level.gameObject);
        }
    }

    public void spawnWayPoint(float[] angles, int numWaypoints, float radius, GameObject level)
    {
        for(float i = (2 * Mathf.PI) / numWaypoints; i < 2 * Mathf.PI; i += (2 * Mathf.PI) / numWaypoints)
        {
            bool obstructed = false;
            //print("i = " + i);
            //for(int j = 0; j < angles.Length-1; j ++)
            //{
            //    //print("j = " + j);
            //    //print("start: " + angles[j] + ", end: " + angles[j+1]);
            //    if (i > angles[j] && i < angles[j+1])
            //    {
            //        obstructed = true;
            //    }
            //}
            if (!obstructed)
            {
                Vector3 pos = new Polar(radius,i).Cartesian3D;
                Instantiate(prefab,pos,Quaternion.identity,level.transform);
            }

        }
    }
}
