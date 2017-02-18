using UnityEngine;
using System.Collections;

public class Moveteddy : MonoBehaviour {

	private int move_forward = 10;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(move_forward > 0)
		{
		    transform.position = transform.position + new Vector3(0.0f, 0.05f, 0.0f);
		    move_forward = move_forward - 1;
		    if(move_forward == 0)
		        move_forward = -10;
		}
		else 
		{
			transform.position = transform.position + new Vector3(-0.0f, -0.05f, -0.0f);
			move_forward = move_forward + 1;
		    if(move_forward == 0)
		        move_forward = 10;
		}
	}
}
