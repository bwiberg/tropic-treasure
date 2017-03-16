﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrigger : MonoBehaviour {

    Collider[] inTrap;
    bool isSet;
    public bool isPlaced;
    public float duration;

    // Use this for initialization
    void Start () {
        isSet = false;
        isPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            duration -= Time.deltaTime;
            if (duration < 0)
                Destroy(gameObject);
        }

        Vector3 bot = transform.position + new Vector3(0, -5.0f, 0);
        Vector3 top = transform.position + new Vector3(0, 0.2f, 0);

        inTrap = Physics.OverlapCapsule(bot, top, transform.localScale.z / 2.0f);
        if (isSet) {
            for (int i = 0; i < inTrap.Length; i++)
            {
                if (inTrap[i].gameObject.GetComponent<SimpleAgent>())
                {
                    isSet = false;
                    iTween.MoveBy(gameObject, iTween.Hash("amount", new Vector3(0f, 3.5f, 0f), "time", 0.5f, "oncomplete", "spikesUp"));
                    break;
                }
            }
        }
	}

    void spikesUp()
    {
        for (int i = 0; i < inTrap.Length; i++)
        {
            if (inTrap[i].gameObject.GetComponent<SimpleAgent>())
            {
                GameObject.Destroy(inTrap[i].gameObject);
            }
        }
        iTween.MoveBy(gameObject, iTween.Hash("amount", new Vector3(0f, -3.5f, 0f), "time", 2.0f, "oncomplete", "setTrap"));
    }

    public void setTrap()
    {
        isSet = true;
    }

    public void pause()
    {
        iTween.Pause(gameObject,true);
    }

    public void resume()
    {
        iTween.Resume(gameObject,true);
    }
}
