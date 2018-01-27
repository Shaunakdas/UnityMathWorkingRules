using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class Row : BaseElement {

	//-------------Common Attributes -------------------
	public enum RowType {Default,DragSourceLine,HorizontalScroll};
	public RowType Type = RowType.Default;


	public Paragraph.AlignType RowAlign;

	//List of child cells
	public List<Cell> CellList{get; set;}
	/// <summary>
	/// Gets or sets the max width of the grid cell if DragSourceLine
	/// </summary>
	/// <value>The width of the max grid cell.</value>
	public float maxGridCellWidth{get; set;}


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public Row(){
		CellList = new List<Cell> ();
	}
	public void getAlignType(){
		RowAlign = Paragraph.ParagraphAlign;
		prefabName = (RowAlign == Paragraph.AlignType.Horizontal)?LocationManager.NAME_VERTI_DRAG_SOURCE_LINE_ROW:LocationManager.NAME_HORI_DRAG_SOURCE_LINE_ROW;

	}
	/// <summary>
	/// Set Row  Type
	/// </summary>
	public void getRowType(string type_text){
		maxGridCellWidth = 0;
		Type = (RowType)System.Enum.Parse (typeof(RowType),StringWrapper.ConvertToPascalCase(type_text),true);
		switch (Type) {
		case RowType.DragSourceLine:
			prefabName = LocationManager.NAME_HORI_DRAG_SOURCE_LINE_ROW;
			break;
		case RowType.HorizontalScroll:
			prefabName = LocationManager.NAME_HORIZONTAL_SCROLL_ROW;
			break;
		default:
			prefabName = LocationManager.NAME_HORIZONTAL_SCROLL_ROW;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the Row class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Row(HtmlNode row_node){
		htmlNode = row_node;
		maxGridCellWidth = 0;
		CellList = new List<Cell> ();
		HtmlAttribute attr_tag = row_node.Attributes [AttributeManager.ATTR_TYPE];
		if (attr_tag != null) {
			string type_text = row_node.Attributes [AttributeManager.ATTR_TYPE].Value;
//			Debug.Log ("Initializing Row node of type " + type_text);
			getRowType (type_text);
			if (Type == RowType.DragSourceLine)
				initDragSourceCellList (row_node);
		} else {
//			Debug.Log ("Initializing Row node");
		}

		setAnalyticsIdFromAttr (row_node);
		parseChildNode (row_node);
	}

	/// <summary>
	/// Parses the Row Node to generate Cell nodes
	/// </summary>
	override public void parseChildNode(HtmlNode row_node){
		//		HtmlNodeCollection node_list = para_node.SelectNodes ("//" + HTMLParser.LINE_TAG);
		IEnumerable<HtmlNode> node_list = row_node.Elements(AttributeManager.TAG_CELL) ;

		if (node_list != null) {
			//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.LINE_TAG);

			foreach (HtmlNode cell_node in node_list) {
				Cell newCell = new Cell (cell_node);
				string cell_type = cell_node.Attributes [AttributeManager.ATTR_TYPE].Value;
//				Debug.Log ("cell_type" + cell_type);
				switch (cell_type) {
				case "text": 
					if (Type == RowType.Default) {
						newCell = new TextCell (cell_node);
					} else if (Type == RowType.HorizontalScroll) {
						newCell = new TextCell (cell_node);
					}else {
						newCell = new DragSourceCell (cell_node);
						//Checking For Grid Cell Max Width
						Debug.Log("max size based on "+BasicGOOperation.getNGUITextSize("1056")+cell_node.InnerText);
						maxGridCellWidth = Mathf.Max(maxGridCellWidth, BasicGOOperation.getNGUITextSize(cell_node.InnerText));
					}
					break;
				case "drag_source": 
					newCell = new DragSourceCell (cell_node);
					//Checking For Grid Cell Max Width
					Debug.Log("max size based on "+BasicGOOperation.getNGUITextSize(cell_node.InnerText)+cell_node.InnerText);
					maxGridCellWidth = Mathf.Max(maxGridCellWidth, BasicGOOperation.getNGUITextSize(cell_node.InnerText));
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
					newCell = new SelectableButtonCell (cell_node);
					break;
				case "fraction_table": 
					newCell = new FractionCell (cell_node);
					break;
				case "exponent_table": 
					newCell = new ExponentCell (cell_node);
					break;
				}
				//Populate Child Row nodes inside Cell Node
//				newCell.parseCell (cell_node);
				newCell.Type = (Cell.CellType)System.Enum.Parse (typeof(Cell.CellType),StringWrapper.ConvertToPascalCase(cell_type),true);
				//Add Cell node into CellList
				newCell.Parent = this;
				CellList.Add (newCell);

			}
		}
	}
	override public void  setChildParagraphRef(){
		foreach (Cell cell in CellList) {
			cell.ParagraphRef = this.ParagraphRef;
			cell.setChildParagraphRef ();
			if(cell.Type == Cell.CellType.DragSource)
				Type = RowType.DragSourceLine;
		}
		addToParaDragSourceList ();
	}
	override public void  setChildAnalyticsId(){
		int index = 0;
		foreach (Cell cell in CellList) {
			cell.AnalyticsId = index;
			cell.setChildAnalyticsId ();
			index++;
		}
	}
	public void initDragSourceCellList(HtmlNode row_node){
		HtmlAttribute attr_tag = row_node.Attributes [AttributeManager.ATTR_START];
		if (attr_tag != null) {
			int startInt = int.Parse (row_node.Attributes [AttributeManager.ATTR_START].Value);
			int endInt = int.Parse (row_node.Attributes [AttributeManager.ATTR_END].Value);
			string sourceType = row_node.Attributes [AttributeManager.ATTR_SOURCE_TYPE].Value;
			Debug.Log ("sourceType"+sourceType);
			//Initiate Creation of Cell List 
			switch (sourceType) {
			case "integer":
				for (int a = startInt; a < endInt+1; a = a + 1)
				{
//					Cell newCell = new DragSourceCell ("text",a.ToString());
//					newCell.Parent = this;
//					//Checking For Grid Cell Max Width
//					maxGridCellWidth = Mathf.Max(maxGridCellWidth, BasicGOOperation.getNGUITextSize(a.ToString()));
//					CellList.Add (newCell);
					maxGridCellWidth = addNewCell(a.ToString(),this,maxGridCellWidth,CellList);
				}
				break;
			case "prime":
				for (int a = startInt; a < endInt+1; a = a + 1)
				{
					if (isPrime (a)) {
						maxGridCellWidth = addNewCell(a.ToString(),this,maxGridCellWidth,CellList);
					}
				}
				break;
			case "decimal":
				//Adding decimal point
				maxGridCellWidth = addNewCell(".",this,maxGridCellWidth,CellList);
				//Adding given integers
				for (int a = startInt; a < endInt+1; a = a + 1)
				{
					maxGridCellWidth = addNewCell(a.ToString(),this,maxGridCellWidth,CellList);
				}
				break;
			default:
				for (int a = startInt; a < endInt+1; a = a + 1)
				{
					maxGridCellWidth = addNewCell(a.ToString(),this,maxGridCellWidth,CellList);
				}
				break;
			}
		}

	}
	protected float addNewCell(string _target, Row _row, float _maxGridCellWidth, List<Cell> _cellList){
		Cell newCell = new DragSourceCell ("text",_target);
		newCell.Parent = _row;
		_cellList.Add (newCell);
		//Checking For Grid Cell Max Width
		return Mathf.Max(_maxGridCellWidth, BasicGOOperation.getNGUITextSize(_target));
	}
	public void addToParaDragSourceList (){
		if ((Type == RowType.DragSourceLine)&&(ParagraphRef.DragSourceTableList.IndexOf(Parent as TableLine)<0)) {
			ParagraphRef.DragSourceTableList.Add (Parent as TableLine);
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
	override public int siblingIndex(){
		for (int i = 0; i < (Parent as Line).RowList.Count; i++) {
			if ((Parent as Line).RowList [i] == this)
				return i;
		}
		return 0;
	}

	//----------------------Score Values ----------------------------
	override public void  setChildScoreValues(){
//		Debug.Log ("Calling setChildScoreValues in row");
		foreach (Cell cell in CellList) {
//			Debug.Log ("Calling setChildScoreValues for cell"+cell.AnalyticsId.ToString()+" in row");
			cell.setChildScoreValues ();
		}
	}
	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent GameObject.</param>
	override public GameObject generateElementGO(GameObject parentGO){
		getAlignType ();
		GameObject lineGO;GameObject rowGO;GameObject HorizontalScrollView;
		//Based on row index and row type add row to the top/center/bottom of ContentTableGO
		switch (Type) {
		case RowType.Default:
			//Updating Column count of Parent Table based on cell
			updateTableColumn(parentGO);
			break;
		case RowType.DragSourceLine:
			GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_ROW_TYPE + prefabName)as GameObject;
			rowGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
			//making Grid child of ScrollView as parent
			HorizontalScrollView = BasicGOOperation.getChildGameObject (rowGO, "ScrollView");
			parentGO = BasicGOOperation.getChildGameObject (HorizontalScrollView, "Grid");
			initGOProp (rowGO);
			break;
		case RowType.HorizontalScroll:
			GameObject horizontalScrollPrefab = Resources.Load (LocationManager.COMPLETE_LOC_ROW_TYPE + prefabName)as GameObject;
			rowGO = BasicGOOperation.InstantiateNGUIGO (horizontalScrollPrefab, parentGO.transform);
			//making Grid child of ScrollView as parent
			HorizontalScrollView = BasicGOOperation.getChildGameObject (rowGO, "ScrollView");
			parentGO = BasicGOOperation.getChildGameObject (HorizontalScrollView, "Table");
			initGOProp (rowGO);
			break;

		default:
			//Updating Column count of Parent Table based on cell
			if (Parent.GetType () == typeof(TableLine)) {
				TableLine tableElement = Parent as TableLine;
				Debug.Log ("Number of columns " + CellList.Count);
				if (tableElement.ColumnCount == null) {
					parentGO.GetComponent<UITable> ().columns = CellList.Count;
					tableElement.ColumnCount = CellList.Count;
				}
			}
			break;
		}
		foreach (Cell cell in CellList) {
			GameObject elementGO = cell.generateElementGO (parentGO);
//			Debug.Log (elementGO.GetComponent<SelBtnItemChecker> ().correctFlag);
		}
		updateGOProp (ElementGO);
		return parentGO;
	}
	public void updateTableColumn (GameObject parentGO){
//		Debug.Log (Parent.GetType ());
		if (Parent.GetType () == typeof(TableLine)) {
			TableLine tableElement = Parent as TableLine;
//			Debug.Log ("Number of columns of " +parentGO.name+ CellList.Count + tableElement.ColumnCount);
			if (tableElement.ColumnCount == 0) {
				parentGO.GetComponent<UITable> ().columns = CellList.Count;
				tableElement.ColumnCount = CellList.Count;
			} 
		} else if(Parent.GetType () == typeof(FractionCell)) {
			FractionCell tableElement = Parent as FractionCell;
//			Debug.Log ("Number of columns of " +parentGO.name+ CellList.Count + tableElement.ColumnCount);
			if (tableElement.ColumnCount == 0) {
				parentGO.GetComponent<UITable> ().columns = CellList.Count;
				tableElement.ColumnCount = CellList.Count;
			} 
		}
	}
	override protected void initGOProp(GameObject _elementGO){
		//		Debug.Log ("Updating Grid cell width");
		ElementGO = _elementGO;
		if (Type == RowType.DragSourceLine) {
			//making Grid child of ScrollView as parent
			GameObject HorizontalScrollView = BasicGOOperation.getChildGameObject (_elementGO, "ScrollView");
			GameObject gridGO = BasicGOOperation.getChildGameObject (HorizontalScrollView, "Grid");
			gridGO.GetComponent<UIGrid> ().cellWidth = Mathf.Max(80f,maxGridCellWidth+30f);

		}
	}
	override protected void updateGOProp(GameObject _elementGO){
		if (Type == RowType.DragSourceLine) {
			widenDragDropContainer (ElementGO);
		}
	}
	public void widenDragDropContainer(GameObject _rowGO){
		//making Container width same as TableLine
		GameObject container = BasicGOOperation.getChildGameObject(_rowGO,"Container");
		GameObject grid = BasicGOOperation.getChildGameObject(_rowGO,"Grid");
		Debug.Log (grid.transform.childCount);
		Debug.Log (container.gameObject);
		Debug.Log (container.GetComponent<UIWidget> ().width);
		Debug.Log (_rowGO.transform.parent.gameObject);
		Debug.Log (BasicGOOperation.ElementSize(grid).x);
		Debug.Log (Paragraph.ParagraphAlign);
		if (Paragraph.ParagraphAlign == Paragraph.AlignType.Vertical) {
			container.GetComponent<UIWidget> ().width = (int)((grid.transform.childCount) * (grid.GetComponent<UIGrid> ().cellWidth));
		} else {
			container.GetComponent<UIWidget> ().height = (int)((grid.transform.childCount) * (grid.GetComponent<UIGrid> ().cellHeight));
		}
	}
}
