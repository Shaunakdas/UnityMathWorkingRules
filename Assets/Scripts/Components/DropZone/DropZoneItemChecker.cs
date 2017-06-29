using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneItemChecker : TargetItemChecker {
	public BaseElement element;
	public bool idCheck{ get; set; }
	public GameObject DropZoneHolderGO;
	public string filledText{ get; set; }
	// Use this for initialization
	void Start () {
		idCheck = false;
	}
	//Checker function called after the item is dropped in drop zone.
	public bool checkDropZoneItem(string text){
		Debug.Log ("checkDropZoneItem for checking "+text);
		if (idCheck) {
			if (DropZoneHolderGO.GetComponent<DropZoneHolder> ().checkDropZoneItem (text)) {
				filledText = text;return true;
			}
		} else {
			if (DropZoneHolderGO.GetComponent<DropZoneHolder> ().checkDropZoneItem (compositeText(text))) {
				filledText = text;return true;
			}
		}
		return false;
		
	}
	public string compositeText(string text){
		string textToCheck="";
		foreach (Transform childTransform in gameObject.transform.parent) {
			if (childTransform.gameObject != gameObject) {
				//Getting char of sibling dropzone
				string childFilledText = childTransform.gameObject.GetComponent<DropZoneItemChecker> ().filledText;
				textToCheck += (childFilledText != null) ? childFilledText : " ";
			} else {
				//Getting char of current dropzone
				textToCheck += text;
			}
		}
		return textToCheck;
	}
	// Update is called once per frame
	void Update () {
		
	}


}
