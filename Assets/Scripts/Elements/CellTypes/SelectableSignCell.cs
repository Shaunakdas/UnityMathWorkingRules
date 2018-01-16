using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class SelectableSignCell : SelectableButtonCell {
	//-------------Common Attributes -------------------
	public enum Sign{Positive,Negative};
	public Sign TargetSign{ get; set; }
	public Sign InputSign{ get; set; }
	public string filledText{ get; set; }


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public SelectableSignCell():base(){
		prefabName = LocationManager.NAME_SELECT_SIGN_CELL;
	}
	public SelectableSignCell(string sign):base(){
		genCorrectFlag(sign);
		prefabName = LocationManager.NAME_SELECT_SIGN_CELL;
		Debug.Log ("SelectableSignCell"+correctFlag);
	}
	/// Initializes a new instance of the SelectableSignCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public SelectableSignCell(HtmlNode cell_node):base(cell_node){
		//		CellList = new List<Cell> ();
		TargetSign = (cell_node.Attributes [AttributeManager.ATTR_ANSWER].Value)=="1"? Sign.Positive:Sign.Negative;
		filledText = (cell_node.Attributes [AttributeManager.ATTR_ANSWER].Value)=="1"? "+":"-";
		prefabName = LocationManager.NAME_SELECT_SIGN_CELL;
	}
	public void genCorrectFlag(string sign){
		filledText = sign;
		TargetSign = sign=="+"? Sign.Positive:Sign.Negative;
		correctFlag = sign=="+"? false:true;
	}


	//-------------Based on Element Attributes, creating GameObject -------------------

	protected override void setTriggerMethods(GameObject TableGO, GameObject ElementGO){
//		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		GameObject checkBoxGO = BasicGOOperation.getChildGameObject (TableGO, "CheckBox");
		SelSignOptionChecker itemChecker = ElementGO.GetComponent<SelSignOptionChecker> ();
		itemChecker.filledText = filledText;
		EventDelegate.Set(ElementGO.GetComponent<UIButton>().onClick, delegate() { updateCheckBox(checkBoxGO);itemChecker.changeInputFlag(); });
	}

	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	public virtual SelSignOptionChecker updateItemChecker(GameObject _elementGO, DropZoneQuestionChecker question){
		SelSignOptionChecker option= _elementGO.GetComponent<SelSignOptionChecker> ();
		//Ref Variables of Option
		option.addParentChecker(question); option.ContainerElem = this;

		return option;
	}
}
