using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseElement : MonoBehaviour {
	public string prefabName{get; set;}
	//Parent BaseElement
	public BaseElement Parent{get; set;}
	virtual public void updateGOProp(GameObject ElementGO){
	}

}
