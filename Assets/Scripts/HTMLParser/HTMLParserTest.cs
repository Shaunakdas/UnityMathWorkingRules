using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using UnityEngine.SceneManagement;

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
//		BasicGOOperation.scale =  NGUITools.GetRoot (gameObject).transform.localScale;
//		HTMLParser parser = new HTMLParser ();
//		string text = "";
//		if (AndroidOrganizer.isAndroid ()) {
//			string quesObject = AndroidOrganizer.getSharedPrefs ("QuestionText");
//			if ((quesObject != null) && quesObject.Length > 0) {
//				text = quesObject;
//			} else {
//				//Setting a short question for Integrated Build
//				text = "<body code='working-rule-for-rounding-to-nearest-hundreds/thousands'> <p type='comprehension' align='vertical'> <line type='text' locationType='top'> Round 242 to the nearest hundred. </line> </p><p type='question_step' align='vertical' correctType='single_correct'> <line type='text' locationType='top'>Among 200 and 300 which number is more closest to 242</line> <line type='table'> <trow> <tcell type='selectable_button' answer='1'>200</tcell> <tcell type='selectable_button' answer='0'>300</tcell> </trow> </line> </p></body>";
//			}
//		} else {
//			//Setting a long question for Unity Editor
////			text = "<body code='working-rule-for-rounding-to-nearest-hundreds/thousands'><p type='comprehension' align='vertical'><line type='text' locationType='top'> Round 242 to the nearest hundred. </line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Write number of digits in each number</line><line type='post_submit_text'>We know that number with least amount of digits is the smallest</line><line type='table'><trow><tcell type='text'>250105</tcell><tcell type='drop_zone' answer='6;4;48'></tcell></trow><trow><tcell type='text'>1056</tcell><tcell type='drop_zone' answer='4'></tcell></trow><trow><tcell type='text'>255006</tcell><tcell type='drop_zone' answer='6'></tcell></trow></line><line type='table' locationType='bottom'><trow type='drag_source_line' sourceType='integer' start='0' end='9'></trow></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>To get the largest number, you should order all the numbers in </line><line type='post_submit_text'>To make the largest number, put the biggest digit in the leftmost place</line><line type='table' col='1'><trow><tcell type='selectable_button' answer='0'>increasing order</tcell><tcell type='text'>or</tcell><tcell type='selectable_button' answer='1'>decreasing order</tcell><tcell type='text'>?</tcell></trow></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Select 2 hundreds nearest to 242</line><line type='range_table' col='4' start='1' end='10' count='10' sortOrder='increasing' correctItemType='factor' correctTarget='9'></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Select 2 hundreds nearest to 242</line><line type='table' col='4'><trow><tcell type='selectable_button' answer='0'>0</tcell><tcell type='selectable_button' answer='0'>100</tcell><tcell type='selectable_button' answer='1'>200</tcell><tcell type='selectable_button' answer='1'>300</tcell><tcell type='selectable_button' answer='0'>400</tcell><tcell type='selectable_button' answer='0'>500</tcell><tcell type='selectable_button' answer='0'>600</tcell><tcell type='selectable_button' answer='0'>700</tcell><tcell type='selectable_button' answer='0'>800</tcell><tcell type='selectable_button' answer='0'>900</tcell></trow></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Among 200 and 300 which number is more closest to 242</line><line type='table'><trow><tcell type='selectable_button' answer='1'>200</tcell><tcell type='selectable_button' answer='0'>300</tcell></trow></line></p></body>";
//			text = "<body code='working-rule-for-rounding-to-nearest-hundreds/thousands'> <p type='comprehension' align='vertical'> <line type='text' locationType='top'> Round 242 to the nearest hundred. </line> </p><p type='question_step' align='vertical' correctType='single_correct'> <line type='text' locationType='top'>Among 200 and 300 which number is more closest to 242</line> <line type='table'> <trow> <tcell type='selectable_button' answer='1'>200</tcell> <tcell type='selectable_button' answer='0'>300</tcell> </trow> </line> </p></body>";
//		}
		//		string text = System.IO.File.ReadAllText (@"Assets/Data/Question_Data.html");
		string text = QuestionImporter.getNextQuestion();
		var html = new HtmlDocument ();
		html.LoadHtml (@text);
//		parser.getParagraphList (html);
//		ParagraphList = parser.ParagraphList;
//		ParagraphList.ForEach( x=> x.generateElementGO(gameObject));

		ComprehensionBody body = new ComprehensionBody(html.DocumentNode.Element(AttributeManager.TAG_BODY));
		body.generateElementGO (gameObject);
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
	public void RepositionAfterEndOfFrame(){
		StartCoroutine (WaitForEnd());
	}
	// Update is called once per frame
	void Update () {
		
	}
	//To be called from Android Activity
	public void reloadScene(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
