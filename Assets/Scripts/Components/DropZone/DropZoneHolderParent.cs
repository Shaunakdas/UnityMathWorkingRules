using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHolderParent : MonoBehaviour {
	List<GameObject> dropZoneHolderList;
	public void addDropZoneHolder(GameObject dropZoneHolderGO){
		dropZoneHolderList.Add (dropZoneHolderGO);
	}
	public void dropEvent(bool inputCorrect){
		if (inputCorrect) {
			Debug.Log ("Drag Item was correctly dropped");
		} else {
			Debug.Log ("Drag Item was incorrecly dropped.");
			searchCorrectHolder ();
		}

	}
	public void searchCorrectHolder(){
		foreach (GameObject dropZoneholderItem in dropZoneHolderList) {
			if (dropZoneholderItem.GetComponent<DropZoneHolder> ().TargetTextList [0].Length > 0) {
				Debug.Log ("Correct DropZone Holder reached");
			}
		}
	}
	void Awake(){
		dropZoneHolderList = new List<GameObject> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
