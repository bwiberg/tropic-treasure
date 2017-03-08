using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.AI;

public class CircularTempleDescription {
	public GameObject toGameObject(GameObject templePrefab, GameObject levelPrefab, GameObject segmentPrefab, GameObject obstaclePrefab) {
		var templeGameObject = GameObject.Instantiate(templePrefab) ;
		templeGameObject.name = "Circular Temple";

        var levelIndex = 0;
        foreach (var level in Levels) {
            var levelGameObject = level.toGameObject(levelIndex++, levelPrefab, segmentPrefab, obstaclePrefab);
			levelGameObject.transform.SetParent(templeGameObject.transform);
        }

		return templeGameObject;
    }

	public readonly List<CircularLevelDescription> Levels = new List<CircularLevelDescription>();
}
