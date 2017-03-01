using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameParameters : MonoBehaviour {

    // world objects
    GameObject settings;
    GameObject target;

    // game parameters
    Globals.gameMode mode;
    int threshold;
    int killCount;
    int score;
    float time;


	// Use this for initialization
	void Start () {
        settings = GameObject.FindGameObjectWithTag("Settings");
        target = GameObject.FindGameObjectWithTag("Target");

        mode = settings.GetComponent<Globals>().getGameMode();
        threshold = settings.GetComponent<Globals>().getThreshold();

        if (mode != Globals.gameMode.TIME)
            time = 0;
        else
            time = threshold;
    }
	
	// Update is called once per frame
	void Update () {
        switch (mode)
        {
            // Win/Lose conditions
            case Globals.gameMode.KILLS:
                if (killCount > threshold)
                    SceneManager.LoadScene("StartScene");
                break;
            case Globals.gameMode.SCORE:
                // need to decide how to define score
                if (score > threshold)
                    SceneManager.LoadScene("StartScene");
                break;
            case Globals.gameMode.SURVIVAL:
                if (target.GetComponent<Target>().getHealth() <= 0)
                    SceneManager.LoadScene("StartScene");
                break;
            case Globals.gameMode.TIME:
                if (time <= 0)
                    SceneManager.LoadScene("StartScene");
                break;
        }

        if (mode != Globals.gameMode.TIME)
            time += Time.deltaTime;
        else
            time -= Time.deltaTime;
    }

    // GAME PARAMETER MANIPULATORS
    public void increaseKillCount(int killed)
    {
        killCount += killed;
    }

    public void resetKillCount(int killed)
    {
        killCount = 0;
    }

    public int getKillCount()
    {
        return killCount;
    }

    public void increaseScore(int val)
    {
        score += val;
    }

    public int getScore()
    {
        return score;
    }

    public float getTime()
    {
        return time;
    }

    // if we are in time objective mode, increase/decrease time left according to amt
    // return:
    //      -1 if it isn't time mode
    //      0 if time is increased
    //      1 if time is decreased 
    public int manipulateTime(float amt)
    {
        if(mode != Globals.gameMode.TIME)
            return -1;

        time += amt;
        if (amt > 0)
            return 0;
        else
            return 1;
    }
}
