using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Cell : BaseElement {
	public enum CellType {Text,DropZone,DropZoneRow,SelectableButton,SelectableSign,DragSource,FractionTable,ExponentTable,NumberLineLabel,NumberLineLabelAnswer};
	//Cell Prefabs
	public GameObject DragSourceCellPF,DropZoneCellPF,LatexTextCellPF,TextCellPF,TableCellPF,SelectBtnCellPF,SelectSignCellPF,NumLineLabelCellPF;

	public string CellId{ get; set; } 
	public string CellTag{ get; set; }
	public string DisplayText{ get; set; }
	public CellType Type{get; set;}
	public Cell(){
	}
	/// <summary>
	/// Set Cell  Type
	/// </summary>
	public void getCellType(string type_text){
	}
	/// <summary>
	/// Initializes a new instance of the Cell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Cell(HtmlNode cell_node){
//		CellList = new List<Cell> ();
		string type_text = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
//		Debug.Log ("Initializing Cell node of type "+type_text);
		getCellType (type_text);
	}
	public GameObject getPF(){
		GameObject prefab =null;
		switch (Type) {
		case CellType.DragSource:
			return DragSourceCellPF;
		case CellType.DropZone:
			return DropZoneCellPF;
		case CellType.DropZoneRow:
			return DropZoneCellPF;
		case CellType.ExponentTable:
			return prefab;
		case CellType.FractionTable:
			return prefab;
		case CellType.NumberLineLabel:
			return NumLineLabelCellPF;
		case CellType.NumberLineLabelAnswer:
			return NumLineLabelCellPF;
		case CellType.SelectableButton:
			return prefab;
		case CellType.SelectableSign:
			return prefab;
		case CellType.Text:
			return LatexTextCellPF;
		default:
			return prefab;
		}
	}
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	virtual public GameObject generateElementGO(GameObject parentGO){
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + prefabName)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		updateGOProp (cellGO);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
	virtual public void updateGOProp(GameObject ElementGO){
		//		Debug.Log ("Updating Text of Cell");
	}
}
