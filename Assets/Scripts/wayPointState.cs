using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class wayPointState : MonoBehaviour {

    public int level;
    public GameObject nextWayPoint;
    public float cost;
    public NavMeshPath path;
    public Transform chest;

	// Use this for initialization
	void Start () {
        path = null;
        cost = -1;
	}
	
	// Update is called once per frame
	void Update () {
        wayPointState[] wps = GameObject.FindObjectsOfType<wayPointState>();
        if (level == 0)
        {
            NavMesh.CalculatePath(gameObject.transform.position, chest.position, NavMesh.AllAreas, path);
        } else {
            foreach (wayPointState wp in wps)
            {
                NavMeshPath p2 = null;
                if (wp.level == level || wp.level == level + 1 || wp.level == level - 1)
                {
                    NavMesh.CalculatePath(gameObject.transform.position, wp.gameObject.transform.position, NavMesh.AllAreas, p2);
                    
                    if(p2.status == NavMeshPathStatus.PathPartial)
                    {
                        continue;
                    }

                    float newcost = CalculatePathCost(p2)/2.0f + wp.cost;
                    if (newcost < cost || cost == -1)
                    {
                        cost = newcost;
                        path = p2;
                        nextWayPoint = wp.gameObject;
                    }
                }
            }
        }
	}

    /// <summary>
    /// Code to calculate path cost
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>

    // return index for mask if it has exactly one bit set
    // otherwise returns -1
    int IndexFromMask(int mask)
    {
        for (int i = 0; i < 32; ++i)
        {
            if ((1 << i) == mask)
                return i;
        }
        return -1;
    }

    float CalculatePathCost(NavMeshPath path)
    {
        var corners = path.corners;
        if (corners.Length < 2)
            return Mathf.Infinity;

        var hit = new NavMeshHit();
        NavMesh.SamplePosition(corners[0], out hit, 0.1f, NavMesh.AllAreas);

        var pathCost = 0.0f;
        var costMultiplier = NavMesh.GetAreaCost(IndexFromMask(hit.mask));
        int mask = hit.mask;
        var rayStart = corners[0];

        for (int i = 1; i < corners.Length; ++i)
        {
            // the segment may contain several area types - iterate over each
            while (NavMesh.Raycast(rayStart, corners[i], out hit, hit.mask))
            {
                pathCost += costMultiplier * hit.distance;
                costMultiplier = NavMesh.GetAreaCost(IndexFromMask(hit.mask));
                mask = hit.mask;
                rayStart = hit.position;
            }

            // advance to next segment
            pathCost += costMultiplier * hit.distance;
            costMultiplier = NavMesh.GetAreaCost(IndexFromMask(hit.mask));
            mask = hit.mask;
            rayStart = hit.position;
        }

        return pathCost;
    }
}
