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
	public GameObject CombinationLinePF,NumLineDropLinePF,PrimeDivLinePF,TextLinePF,LatexTextLinePF;
	//Paragraph Prefabs
	public GameObject QuestionStepParaPF,ComprehensionParaPF;
	//Row Prefabs
	public GameObject DragSourceLineRowPF;
	//GameObjects
	// Use this for initialization
	void Start () {
		HTMLParser parser = new HTMLParser ();

		string text = System.IO.File.ReadAllText (@"Assets/Data/Question_Data.html");
		var html = new HtmlDocument ();
		html.LoadHtml (@text);
		parser.getParagraphList (html);
		ParagraphList = parser.ParagraphList;
	}
	public void generateUI(){
		Paragraph para = ParagraphList [0];
		if (para.ParagraphStep == Paragraph.StepType.QuestionStep) {
			//Paragrah is of type QuestionStep

		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
