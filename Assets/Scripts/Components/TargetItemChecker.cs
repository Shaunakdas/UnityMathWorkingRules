using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetItemChecker : MonoBehaviour {
	public Paragraph ParagraphRef;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	virtual public void addToTargetList(){
	}
	//----------------------Animations ----------------------------
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void activateAnim(){

	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void deactivateAnim(){
	}
	/// <summary>
	/// Correct animation.
	/// </summary>
	virtual public void correctAnim(){

	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	virtual public void incorrectAnim(){

	}
}
