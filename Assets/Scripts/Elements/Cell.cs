using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Cell : Line {
	public Paragraph.AlignType DragAlign;

	public enum CellType {Text,DropZone,DropZoneRow,SelectableButton,SelectableSign,DragSource,FractionTable,ExponentTable,NumberLineLabel,NumberLineLabelAnswer};
	//Cell Prefabs
	public GameObject DragSourceCellPF,DropZoneCellPF,LatexTextCellPF,TextCellPF,TableCellPF,SelectBtnCellPF,SelectSignCellPF,NumLineLabelCellPF;

	public string CellId{ get; set; } 
	public string CellTag{ get; set; }
	public string DisplayText{ get; set; }
	public new CellType Type{get; set;}
	public Cell(){
		RowList = new List<Row> ();
	}
	public Cell(string type_text){
		RowList = new List<Row> ();
		getCellType (type_text);
	}
	/// <summary>
	/// Set Cell  Type
	/// </summary>
	virtual public void getCellType(string type_text){
	}
	/// <summary>
	/// Initializes a new instance of the Cell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Cell(HtmlNode cell_node){
		string type_text = cell_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		Debug.Log ("Initializing Cell node of type "+type_text);
		getCellType (type_text);
		parseChildNode (cell_node);
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

	virtual public void  getAlignType(){
		Paragraph paraObj = (Paragraph)this.Parent.Parent.Parent;
		DragAlign = paraObj.ParagraphAlign;
	}
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	override public GameObject generateElementGO(GameObject parentGO){
		Debug.Log (RowList == null);
		getAlignType ();
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + prefabName)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		ElementGO = cellGO;
		initGOProp (cellGO);
		foreach (Row row in RowList) {
			row.generateElementGO (cellGO);
		}
		updateGOProp (cellGO);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
	override public void updateGOProp(GameObject ElementGO){
		//		Debug.Log ("Updating Text of Cell");
	}
}
