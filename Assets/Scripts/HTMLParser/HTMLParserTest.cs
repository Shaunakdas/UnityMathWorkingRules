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
		BasicGOOperation.scale =  NGUITools.GetRoot (gameObject).transform.localScale;
		HTMLParser parser = new HTMLParser ();

		string text = System.IO.File.ReadAllText (@"Assets/Data/Question_Data.html");
		var html = new HtmlDocument ();
		html.LoadHtml (@text);
		parser.getParagraphList (html);
		ParagraphList = parser.ParagraphList;
		ParagraphList.ForEach( x=> x.generateElementGO(gameObject));
//		generateParaUI ();
		StartCoroutine (WaitForEnd());
	}

	//Waiting for the end and reposition all the child tables and grids
	public IEnumerator WaitForEnd(){
		yield return new WaitForEndOfFrame();
		Debug.Log ("Waited enough");
		BasicGOOperation.RepositionChildTables (gameObject);
		BasicGOOperation.RepositionChildTables (gameObject);
	}
	// Update is called once per frame
	void Update () {
		
	}

}
