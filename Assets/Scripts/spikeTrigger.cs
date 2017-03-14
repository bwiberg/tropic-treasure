using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("agent should die\n");
        if (other.gameObject.GetComponent<SimpleAgent>())
        {
            Animator spikeAnim =  GetComponentInChildren<Animator>();
            spikeAnim.SetTrigger("trigger");
            GameObject.Destroy(other.gameObject,1f);
        }
    }
}
