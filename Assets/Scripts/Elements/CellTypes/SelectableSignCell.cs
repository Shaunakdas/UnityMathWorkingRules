using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class SelectableSignCell : SelectableButtonCell {
	//-------------Common Attributes -------------------
	public enum Sign{Positive,Negative};
	public Sign TargetSign{ get; set; }
	public Sign InputSign{ get; set; }


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public SelectableSignCell():base(){
	}
	public SelectableSignCell(string sign):base(){
		TargetSign = sign=="+"? Sign.Positive:Sign.Negative;
		prefabName = LocationManager.NAME_SELECT_SIGN_CELL;
	}
	/// Initializes a new instance of the SelectableSignCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public SelectableSignCell(HtmlNode cell_node):base(cell_node){
		//		CellList = new List<Cell> ();
		TargetSign = (cell_node.Attributes [AttributeManager.ATTR_ANSWER].Value)=="1"? Sign.Positive:Sign.Negative;
		prefabName = LocationManager.NAME_SELECT_SIGN_CELL;
	}



	//-------------Based on Element Attributes, creating GameObject -------------------

	protected override void setTriggerMethods(GameObject TableGO, GameObject ElementGO){
//		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		GameObject checkBoxGO = BasicGOOperation.getChildGameObject (TableGO, "CheckBox");
		SelSignOptionChecker itemChecker = ElementGO.GetComponent<SelSignOptionChecker> ();
		Debug.Log (itemChecker.GetType());
		EventDelegate.Set(ElementGO.GetComponent<UIButton>().onClick, delegate() { updateCheckBox(checkBoxGO);itemChecker.changeInputFlag(); });
	}
}
