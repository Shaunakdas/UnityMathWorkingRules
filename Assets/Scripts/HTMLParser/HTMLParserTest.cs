using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class HTMLParserTest : MonoBehaviour {
	//List of Paragraphs of questions
	public List<Paragraph> ParagraphList;

	//Prefabs
	//Cell Prefabs
	public GameObject DragSourceCellPF,DropZoneCellPF,LatexTextCellPF,TextCellPF,TableCellPF,SelectBtnCellPF,SelectSignCellPF,NumLineLabelCellPF;
	//Line Prefabs
	public GameObject CombinationLinePF,NumLineDropLinePF,PrimeDivLinePF,TextLinePF,LatexTextLinePF,TableLinePF;
	//Paragraph Prefabs
	public GameObject QuestionStepParaPF,ComprehensionParaPF;
	//Row Prefabs
	public GameObject DragSourceLineRowPF;
	//Miscellanous PRefabs;
	//GameObjects
	// Use this for initialization
	public GameObject CenterContentScrollViewPF;
	void Start () {
		HTMLParser parser = new HTMLParser ();

		string text = System.IO.File.ReadAllText (@"Assets/Data/Question_Data.html");
		var html = new HtmlDocument ();
		html.LoadHtml (@text);
		parser.getParagraphList (html);
		ParagraphList = parser.ParagraphList;
	}
	public void generateParaUI(){
		Paragraph para = ParagraphList [0];
		if (para.ParagraphStep == Paragraph.StepType.QuestionStep) {
			//Paragraph is of type QuestionStep

			//Adding QuestionStepParaPF to the root GameObject
			GameObject QuestionStepParaGO = BasicGOOperation.InstantiateNGUIGO(QuestionStepParaPF,gameObject.transform);

			GameObject ParaContentTableGO = BasicGOOperation.getChildGameObject (QuestionStepParaGO, "ParaContentTable");
			//Getting LineList
			List<Line> LineList = para.LineList;

			//Checking for CenterContentScrollView
			GameObject CenterContentGO = ParaContentTableGO;
			foreach (Line line in LineList) if (line.LineLocation == Line.LocationType.Default)
			{
				CenterContentGO = BasicGOOperation.InstantiateNGUIGO(CenterContentScrollViewPF,ParaContentTableGO.transform);
					break;
			}

			foreach (Line line in LineList) {
				generateLineUI (line,ParaContentTableGO,CenterContentGO );
			}
		}
	}
	public void generateLineUI(Line line, GameObject ContentTableGO, GameObject CenterContentGO){
		GameObject parentGO = ContentTableGO;
		//Based on line index and line type add line to the top/center/bottom of ContentTableGO
		switch (line.LineLocation) {
		case Line.LocationType.Top:
			//Adding lineGO to the ContentTableGO at the top
			parentGO = ContentTableGO;
			break;
		case Line.LocationType.Default:
			//Adding QuestionStepParaPF to the root GameObject
			parentGO = CenterContentGO;
			break;
		case Line.LocationType.Bottom:
			parentGO = ContentTableGO;
			break;
		}
		GameObject lineGO = BasicGOOperation.InstantiateNGUIGO(line.getPF(),parentGO.transform);

		foreach (Row row in line.RowList) {
			generateRowUI (row,lineGO );
		}
	}

	public void generateRowUI(Row row, GameObject lineGO){
		GameObject parentGO = lineGO;
		//Based on row index and row type add row to the top/center/bottom of ContentTableGO
		switch (row.Type) {
		case Row.RowType.Default:
			//keeping lineGO as parent
			parentGO = lineGO;
			//Updating Column count of Parent Table based on cell
			if (row.Parent.GetType () == typeof(Line)) {
				lineGO.GetComponent<UITable> ().columns = row.CellList.Count;
			}
			break;
		case Row.RowType.DragSource:
			//making Grid child of ScrollView as parent
			GameObject HorizontalScrollView = BasicGOOperation.getChildGameObject (lineGO, "ScrollView");
			parentGO = BasicGOOperation.getChildGameObject (HorizontalScrollView, "Grid");
			break;
		}

		foreach (Cell cell in row.CellList) {
			generateCellUI (cell,parentGO );
		}
	}
	public void generateCellUI(Cell cell, GameObject parentGO){
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO(cell.getPF(),parentGO.transform);
		if (cell.GetType () = typeof(TableCell)) {
			//Handle TableCell here
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
