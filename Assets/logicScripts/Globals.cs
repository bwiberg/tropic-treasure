using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {

    public enum gameMode { TIME, KILLS, SCORE, SURVIVAL}

    ///////////////////// GLOBAL PARAMETERS /////////////////////

    // general settings
    int volume;
    
    // game type
    gameMode mode;
    int threshold;

    // difficulty setting
    int wealth; // initial health
    float spawnRate;

	// Use this for initialization
	void Start () {
        volume = 100;
        mode = gameMode.SURVIVAL;
        threshold = 10;

        wealth = 1;
        spawnRate = 1;
        GameObject.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    ///////////////////// GLOBAL GETTERS/SETTERS /////////////////////

    public int getVolume()
    {
        return volume;
    }

    public void setVolume(int amp)
    {
        if (amp < 0)
            volume = 0;
        else if (amp > 100)
            volume = 100;
        else
            volume = amp;
    }

    public void setGameMode(gameMode objective)
    {
        mode = objective;
    }

    public gameMode getGameMode()
    {
        return mode;
    }

    public int getWealth()
    {
        return wealth;
    }

    public void setWealth(int health)
    {
        wealth = health;
    }

    public float getSpawnRate()
    {
        return spawnRate;
    }

    public void setSpawnRate(float rate)
    {
        spawnRate = rate;
    }

    public int getThreshold()
    {
        return threshold;
    }

    public void setThreshold(int val)
    {
        threshold = val;
    }
}
