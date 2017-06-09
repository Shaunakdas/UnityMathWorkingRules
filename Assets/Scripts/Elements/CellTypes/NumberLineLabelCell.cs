using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class NumberLineLabelCell : Cell {
	public enum LabelType{Label,LabelAnswer}
	public LabelType LineLabel{ get; set; }
	public int LabelIndex{ get; set; }
	public string DisplayText{ get; set; }
	//Constructor
	public NumberLineLabelCell(string type, string labelIndex){
		getCellType (type);
		LabelIndex = int.Parse(labelIndex);
	}
	/// <summary>
	/// Set NumberLineLabelCell  Type
	/// </summary>
	public void getCellType(string type_text){
		switch (type_text) {
		case "number_line_label": 
			Type = CellType.NumberLineLabel;
			LineLabel = LabelType.Label;
			break;
		case "number_line_label_answer": 
			Type = CellType.NumberLineLabelAnswer;
			LineLabel = LabelType.Label;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the NumberLineLabelCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public NumberLineLabelCell(HtmlNode cell_node){
		//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		Debug.Log ("Initializing NumberLineLabelCell node of type "+type_text);
		getCellType (type_text);
		LabelIndex = int.Parse(cell_node.Attributes [AttributeManager.ATTR_LABEL_INDEX].Value);
		prefabName = LocationManager.NAME_NUM_LINE_LABEL_CELL;
		DisplayText = cell_node.InnerText;
	}
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	override public GameObject generateElementGO(GameObject parentGO){
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + prefabName)as GameObject;

		NumberLineDropLine numberLineDropLine = (NumberLineDropLine)Parent.Parent;
		GameObject cellGO = numberLineDropLine.addChildGOToParentGO (this);
//		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		updateGOProp (cellGO);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
}
