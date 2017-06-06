using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Row : BaseElement {
	public enum RowType {Default,DragSource};
	public RowType Type{ get; set; }

	public List<Cell> CellList{get; set;}

	//Constructor
	public Row(){

	}
	/// <summary>
	/// Set Row  Type
	/// </summary>
	public void getRowType(string type_text){
		Type = RowType.Default;
		if (type_text == "drag_source_line") {
			Type = RowType.DragSource;
			prefabName = LocationManager.NAME_DRAG_SOURCE_LINE_ROW;

		}
	}
	/// <summary>
	/// Initializes a new instance of the Row class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Row(HtmlNode row_node){
		Type = RowType.Default;
		CellList = new List<Cell> ();
		HtmlAttribute attr_tag = row_node.Attributes [HTMLParser.ATTR_TYPE];
		if (attr_tag != null) {
			string type_text = row_node.Attributes [HTMLParser.ATTR_TYPE].Value;
			Debug.Log ("Initializing Row node of type " + type_text);
			getRowType (type_text);
			initDragSourceCellList (row_node);
		} else {
			Debug.Log ("Initializing Row node");
		}
		parseRow (row_node);
	}

	/// <summary>
	/// Parses the Row Node to generate Cell nodes
	/// </summary>
	public void parseRow(HtmlNode para_node){
		//		HtmlNodeCollection node_list = para_node.SelectNodes ("//" + HTMLParser.LINE_TAG);
		IEnumerable<HtmlNode> node_list = para_node.Elements(HTMLParser.CELL_TAG) ;

		if (node_list != null) {
			//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.LINE_TAG);

			foreach (HtmlNode cell_node in node_list) {
				Cell newCell = new Cell (cell_node);
				string cell_type = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
				switch (cell_type) {
				case "text": 
					if (Type == RowType.Default) {
						newCell = new TextCell (cell_node);
					} else {
						newCell = new DragSourceCell (cell_node);
					}
					break;
				case "drag_source": 
					newCell = new DragSourceCell (cell_node);
					break;
				case "drop_zone_row": 
					newCell = new DropZoneRowCell (cell_node);
					break;
				case "drop_zone": 
					newCell = new DropZoneRowCell (cell_node);
					break;
				case "number_line_label": 
					newCell = new NumberLineLabelCell (cell_node);
					break;
				case "number_line_label_answer": 
					newCell = new NumberLineLabelCell (cell_node);
					break;
				case "selectable_sign": 
					newCell = new SelectableSignCell (cell_node);
					break;
				case "selectable_button": 
					newCell = new SelectableSignCell (cell_node);
					break;
				case "fraction_table": 
					newCell = new TableCell (cell_node);
					break;
				case "exponent_table": 
					newCell = new TableCell (cell_node);
					break;
				}
				//Populate Child Row nodes inside Cell Node
//				newCell.parseCell (cell_node);
				//Add Cell node into CellList
				newCell.Parent = this;
				CellList.Add (newCell);

			}
		}
	}
	public void initDragSourceCellList(HtmlNode row_node){
		HtmlAttribute attr_tag = row_node.Attributes [HTMLParser.ATTR_START];
		if (attr_tag != null) {
			int startInt = int.Parse (row_node.Attributes [HTMLParser.ATTR_START].Value);
			int endInt = int.Parse (row_node.Attributes [HTMLParser.ATTR_END].Value);
			string sourceType = row_node.Attributes [HTMLParser.ATTR_SOURCE_TYPE].Value;
			//Initiate Creation of Cell List 
			switch (sourceType) {
			case "integer":
				for (int a = startInt; a < endInt+1; a = a + 1)
				{
					Cell newCell = new DragSourceCell ("text",a.ToString());
					CellList.Add (newCell);
				}
				break;
			case "prime":
				for (int a = startInt; a < endInt+1; a = a + 1)
				{
					if (isPrime (a)) {
						Cell newCell = new DragSourceCell ("text",a.ToString());
						CellList.Add (newCell);
					}
				}
				break;
			default:
				for (int a = startInt; a < endInt+1; a = a + 1)
				{
					Cell newCell = new DragSourceCell ("text",a.ToString());
					CellList.Add (newCell);
				}
				break;
			}
		}
	}
	bool isPrime(int number)
	{
		if (number == 1) return false;
		if (number == 2) return true;
		for (int i = 2; i <= Mathf.Ceil(Mathf.Sqrt(number)); ++i)  {
			if (number % i == 0)  return false;
		}
		return true;
	}
}
