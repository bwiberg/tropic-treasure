using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript.InputSources;
using cakeslice;
using UnityEngine;

namespace TouchScript
{
    public class SpawnSpikes2 : MonoBehaviour
    {
        public GameObject Prefab;

        private void OnEnable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.TouchesBegan += touchesBeganHandler;
            }
        }

        private void OnDisable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.TouchesBegan -= touchesBeganHandler;
            }
        }

        private void spawnPrefabAt(Vector2 position)
        {
            var obj = Instantiate(Prefab) as GameObject;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(position);
            gameObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity);
            obj.name = string.Format("spikes");
            obj.transform.SetParent(gameObject.transform.parent);
            obj.transform.position = hit.point;
            obj.transform.rotation = transform.rotation;
        }

        private void touchesBeganHandler(object sender, TouchEventArgs e)
        {
            foreach (var point in e.Touches)
            {
                spawnPrefabAt(point.Position);
            }
        }
    }
}
