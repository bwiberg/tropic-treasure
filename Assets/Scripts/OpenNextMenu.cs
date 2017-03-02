using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNextMenu : StateMachineBehaviour {

	public string currentMenu;
	public string nextMenu;
	private GameObject[] menus;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (menus == null)
		  	menus = animator.gameObject.GetComponent<MenuLinker>().menus;
		  	foreach (GameObject menu in menus)
		  	{
		  		if(menu.name == currentMenu)
		  		{
		  			menu.SetActive(false);
		  		}
		  		if(menu.name == nextMenu)
		  		{
		  			menu.SetActive(true);
		  		}
		  	}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
