using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    GameObject settings;

    // treasure parameters
    int health;

	// Use this for initialization
	void Start () {
        settings = GameObject.FindGameObjectWithTag("Settings");
        health = settings.GetComponent<Globals>().getWealth();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SimpleAgent>())
        {
            GameObject.Destroy(other.gameObject);
            damage(1);
        }
    }

    // health API
    public void damage(int dmg)
    {
        if(dmg > 0 )
            health -= dmg;
    }

    public void heal(int amount)
    {
        if (amount > 0)
            health -= amount;
    }

    public int getHealth()
    {
        return health;
    }
}
