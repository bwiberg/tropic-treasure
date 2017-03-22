using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using TouchScript;
using TouchScript.InputSources;

public class spawnSpikes : MonoBehaviour {

    public GameObject spawnObject;
    public float duration;
    public bool placeTrap;
    public GameObject ts;

	private bool hasPlacedTrap;
    private GameObject spikeField;

	// Use this for initialization
	void Start () {
        placeTrap = false;
	}
	
	// Update is called once per frame
	void Update () {
        /*if(Input.touchCount > 0)
        {
            RaycastHit hit;
            var touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (touch.phase == TouchPhase.Began
                && gameObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
            {
                spikeField = GameObject.Instantiate(spawnObject, hit.point, Quaternion.identity);
                spikeField.name = string.Format("spikes_{0}", spikeCount);
                spikeField.transform.SetParent(gameObject.transform.parent);
            }
        }*/

        if (placeTrap)
        {
            ts.GetComponent<StandardInput>().enabled = false;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0)
                && gameObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
            {
                spikeField = GameObject.Instantiate(spawnObject, hit.point, Quaternion.identity);
                spikeField.name = string.Format("spikes");
                spikeField.transform.SetParent(gameObject.transform.parent);

				hasPlacedTrap = true;
            }
            if (Input.GetMouseButton(0)
                && gameObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
            {
                spikeField.transform.position = hit.point;
            }
            if (hasPlacedTrap && Input.GetMouseButtonUp(0)
                && gameObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
            {
                ts.GetComponent<StandardInput>().enabled = true;
                spikeField.GetComponent<Outline>().enabled = false;
                spikeField.GetComponent<spikeTrigger>().setTrap();
                spikeField.GetComponent<spikeTrigger>().duration = duration;
                spikeField.GetComponent<spikeTrigger>().isPlaced = true;
                //Destroy(spikeField.gameObject, duration);
                placeTrap = false;
            }
        }
    }

    public void setTrap()
    {
        placeTrap = true;
		hasPlacedTrap = false;
    }
}
