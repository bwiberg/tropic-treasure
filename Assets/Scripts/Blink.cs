using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
// this toggles a component (usually an Image or Renderer) on and off for an interval to simulate a blinking effect
public class Blink : MonoBehaviour {

    // this is the UI.Text or other UI element you want to toggle
    //public MaskableGraphic imageToToggle;
 
    public float interval = 1f;
    public float startDelay = 0.5f;
	public int numberOfBlinks = 3;
    public bool currentState = true;
    public bool defaultState = true;
	public bool isBlinking{get; set;}
	private int blinkingCounter = 0;
 
    void Start()
    {
    	isBlinking = false;
    	gameObject.SetActive(defaultState);
    }
 
    public void StartBlink()
    {
    	// do not invoke the blink twice - needed if you need to start the blink from an external object
        if (isBlinking)
            return;
		gameObject.SetActive(defaultState);

        isBlinking = true;
        InvokeRepeating("ToggleState", startDelay, interval);
    }
 
    public void ToggleState()
    {
    	currentState = !currentState;
		gameObject.SetActive(currentState);
		blinkingCounter += 1;
		if(blinkingCounter == 2*numberOfBlinks)
		{
			gameObject.SetActive(false);
			CancelInvoke();
			isBlinking = false;
			blinkingCounter = 0;
		}
    }
     
}