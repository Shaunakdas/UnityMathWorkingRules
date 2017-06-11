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
	override public GameObject generateElementGO(GameObject parentGO){
		getAlignType ();
		return generateSelBtnCellGO (parentGO, DisplayText);
	}
	static public GameObject generateSelBtnCellGO(GameObject parentGO, string text){
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_SELECT_BTN_CELL)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		updateSelectBtnGO(cellGO, text);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
	override public void updateGOProp(GameObject ElementGO){
		//		Debug.Log ("Updating Text of Cell" + DisplayText);
		updateSelectBtnGO(ElementGO, DisplayText);
	}
	//Static method which can be used by any class initiating SelectButton
	public static void updateSelectBtnGO(GameObject ElementGO, string text){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		updateText (ElementGO, text);
		//Setting width based on text width
		resizeToFit(ElementGO);
		GameObject checkBoxGO = BasicGOOperation.getChildGameObject (TableGO, "CheckBox");
		EventDelegate.Set(ElementGO.GetComponent<UIButton>().onClick, delegate() { updateCheckBox(checkBoxGO); });
	}
	public static void updateCheckBox(GameObject checkBoxGO){
//		checkBoxGO.GetComponent<UIToggle> ().value = !checkBoxGO.GetComponent<UIToggle> ().value;
		checkBoxGO.GetComponent<UIToggle> ().Set(!checkBoxGO.GetComponent<UIToggle> ().value,true);
	}
	public static void updateTableCol(GameObject ElementGO, int col){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		TableGO.GetComponent<UITable> ().columns = col;
	}
	public static void updateText(GameObject ElementGO, string text){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		GameObject labelGO = BasicGOOperation.getChildGameObject (TableGO, "Label");
		//Setting text
		labelGO.GetComponent<UILabel> ().text = text;
	}
	public static void updateTextSize(GameObject ElementGO, int size){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		GameObject labelGO = BasicGOOperation.getChildGameObject (TableGO, "Label");
		//Setting text
		labelGO.GetComponent<UILabel> ().fontSize = size;
	}
	public static void resizeToFit(GameObject ElementGO){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		Debug.Log ("Name of ElemtnGO" + ElementGO.name + "NAme of targetGO" + TableGO.GetComponentInChildren<UILabel>().text);
		TableGO.GetComponent<UITable> ().Reposition ();
		BasicGOOperation.ResizeToFitTargetGO (ElementGO, TableGO);
		ElementGO.GetComponent<UIWidget> ().width = (int)(ElementGO.GetComponent<UIWidget> ().width + 25f);
		ElementGO.GetComponent<UIWidget> ().height = (int)(ElementGO.GetComponent<UIWidget> ().height + 10f);
	}
		

}
