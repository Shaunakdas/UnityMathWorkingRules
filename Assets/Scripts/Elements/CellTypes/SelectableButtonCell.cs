using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class SelectableButtonCell : Cell {
	public bool correctFlag;
	public string DisplayText;
	//Constructor
	public SelectableButtonCell(string type, string answer, string displayText){
		getCellType (type);
		correctFlag = answer=="1"? true:false;
		DisplayText = displayText;
	}
	/// <summary>
	/// Set SelectableButtonCell  Type
	/// </summary>
	public void getCellType(string type_text){
		if(type_text == "selectable_button") Type = CellType.SelectableButton;
	}
	/// <summary>
	/// Initializes a new instance of the SelectableButtonCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public SelectableButtonCell(HtmlNode cell_node){
		//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		Debug.Log ("Initializing SelectableButtonCell node of type "+type_text);
		getCellType (type_text);
		correctFlag = (cell_node.Attributes [AttributeManager.ATTR_ANSWER].Value)=="1"? true:false;
		prefabName = LocationManager.NAME_SELECT_BTN_CELL;
		DisplayText = cell_node.InnerText;
	}
	override public void updateGOProp(GameObject ElementGO){
		//		Debug.Log ("Updating Text of Cell" + DisplayText);
		updateSelectBtnGO(ElementGO, DisplayText);
	}
	//Static method which can be used by any class initiating SelectButton
	public static void updateSelectBtnGO(GameObject ElementGO, string text){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		GameObject labelGO = BasicGOOperation.getChildGameObject (TableGO, "Label");
		//Setting text
		labelGO.GetComponent<UILabel> ().text = text;
		//Setting width based on text width
		ElementGO.GetComponent<UISprite>().width =(int) Mathf.Max(55f,BasicGOOperation.getNGUITextSize(text)+26f);
		GameObject checkBoxGO = BasicGOOperation.getChildGameObject (TableGO, "CheckBox");
		EventDelegate.Set(ElementGO.GetComponent<UIButton>().onClick, delegate() { updateCheckBox(checkBoxGO); });
	}
	public static void updateCheckBox(GameObject checkBoxGO){
//		checkBoxGO.GetComponent<UIToggle> ().value = !checkBoxGO.GetComponent<UIToggle> ().value;
		checkBoxGO.GetComponent<UIToggle> ().Set(!checkBoxGO.GetComponent<UIToggle> ().value,true);
	}
		

}
