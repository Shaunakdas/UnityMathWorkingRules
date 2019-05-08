using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using HtmlAgilityPack;


/// <summary>
/// File to get question text from files or from UserPreferences if android
/// </summary>
public static class QuestionImporter  {
	//To check where questions should be imported from
	public enum ImportSettings {File,Android,Trial};
	public static ImportSettings importMode{get; set;}
	//User inputted standard for Testing
	public static int inputStandard{ get; set; }
	//User inputted chapter for Testing
	public static int inputChapter{ get; set; }
	//While Complete Testing, index of question currently being tested
	public static int questionArrIndex{ get; set; }
	//While Complete Testing, Arrays of Game screenplays based on user input and QuestionSource folder.
	static ArrayList questionArr = new ArrayList();

	static void setUserInput(){
		//Default Values
		importMode = ImportSettings.Trial;
		inputStandard = -1;
		inputChapter = -1;
		questionArrIndex = -1;
		//User Input Values
		importMode = ImportSettings.File;
		inputStandard = 6;
		inputChapter = -1;
		questionArrIndex = -1;
	}
	public static void getQuestionArray(){
		if ((inputStandard != -1)&&(inputChapter != -1)){
			string file = "QuestionSource/screenplays/standard_"+inputStandard+"/chapter_"+inputChapter+".html";
			loadFromFile (file);
		} else if (inputStandard != -1){
			string folder = "QuestionSource/screenplays/standard_"+inputStandard+"/";
			loadFromFolder(folder);
			//Go through the folder, make an array of chapter files
			//Go through each chapter files, add all html to questionArr
		} else {
//			GetFileStructure();
			string parent = "QuestionSource/screenplays/";
			//For each folder: folder+standard_i
			foreach (string folder in Directory.GetDirectories(parent)) {
				Debug.Log ("Folder"+folder);
				loadFromFolder(folder);
			}
		}
	}
	/// <summary>
	/// Loads from html.
	/// </summary>
	/// <param name="parentNode">Parent node.</param>
	static void loadFromHtml(string parentNode){
		var html = new HtmlDocument ();
		html.LoadHtml (parentNode);
		Debug.Log (html.DocumentNode.SelectNodes("//body")[0].OuterHtml);
		// For each questionHtml
		foreach (HtmlNode body in html.DocumentNode.SelectNodes("//body")) {
			questionArr.Add (body.OuterHtml);
		}
	}
	/// <summary>
	/// Loads from file.
	/// </summary>
	/// <param name="file">File.</param>
	static void loadFromFile(string file){
		if ((file.Contains ("chapter_"))&&(!file.Contains ("meta"))) {
			Debug.Log ("File2"+file);
			loadFromHtml(System.IO.File.ReadAllText (@file));
		}
	}
	/// <summary>
	/// Loads from folder.
	/// </summary>
	/// <param name="folder">Folder.</param>
	static void loadFromFolder(string folder){
		if (folder.Contains ("standard_")) {
			//For each file chapter_j
			foreach (string file in Directory.GetFiles(folder)) {
				loadFromFile (file);
			}
		}
	}
	static void populateQuestions(){
		if (questionArr.Count == 0) {
			setUserInput ();
			if (importMode == ImportSettings.File) {
				getQuestionArray ();
			}
		}
	}
	public static string getNextQuestion(){
		populateQuestions ();
		if (importMode == ImportSettings.File) {
			questionArrIndex++;
			return questionArr [questionArrIndex].ToString ();
		} else if (importMode == ImportSettings.Android) {
			if (AndroidOrganizer.isAndroid ()) {
				string quesObject = AndroidOrganizer.getSharedPrefs ("QuestionText");
				if ((quesObject != null) && quesObject.Length > 0) {
					return quesObject;
				} else {
					//Setting a short question for Integrated Build
					return "<body code='working-rule-for-rounding-to-nearest-hundreds/thousands'> <p type='comprehension' align='vertical'> <line type='text' locationType='top'> Round 242 to the nearest hundred. </line> </p><p type='question_step' align='vertical' correctType='single_correct'> <line type='text' locationType='top'>Among 200 and 300 which number is more closest to 242</line> <line type='table'> <trow> <tcell type='selectable_button' answer='1'>200</tcell> <tcell type='selectable_button' answer='0'>300</tcell> </trow> </line> </p></body>";
				}
			}
		} else if (importMode == ImportSettings.Trial) {
			//Setting a long question for Unity Editor
			//			return "<body code='working-rule-for-rounding-to-nearest-hundreds/thousands'><p type='comprehension' align='vertical'><line type='text' locationType='top'> Round 242 to the nearest hundred. </line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Write number of digits in each number</line><line type='post_submit_text'>We know that number with least amount of digits is the smallest</line><line type='table'><trow><tcell type='text'>250105</tcell><tcell type='drop_zone' answer='6;4;48'></tcell></trow><trow><tcell type='text'>1056</tcell><tcell type='drop_zone' answer='4'></tcell></trow><trow><tcell type='text'>255006</tcell><tcell type='drop_zone' answer='6'></tcell></trow></line><line type='table' locationType='bottom'><trow type='drag_source_line' sourceType='integer' start='0' end='9'></trow></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>To get the largest number, you should order all the numbers in </line><line type='post_submit_text'>To make the largest number, put the biggest digit in the leftmost place</line><line type='table' col='1'><trow><tcell type='selectable_button' answer='0'>increasing order</tcell><tcell type='text'>or</tcell><tcell type='selectable_button' answer='1'>decreasing order</tcell><tcell type='text'>?</tcell></trow></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Select 2 hundreds nearest to 242</line><line type='range_table' col='4' start='1' end='10' count='10' sortOrder='increasing' correctItemType='factor' correctTarget='9'></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Select 2 hundreds nearest to 242</line><line type='table' col='4'><trow><tcell type='selectable_button' answer='0'>0</tcell><tcell type='selectable_button' answer='0'>100</tcell><tcell type='selectable_button' answer='1'>200</tcell><tcell type='selectable_button' answer='1'>300</tcell><tcell type='selectable_button' answer='0'>400</tcell><tcell type='selectable_button' answer='0'>500</tcell><tcell type='selectable_button' answer='0'>600</tcell><tcell type='selectable_button' answer='0'>700</tcell><tcell type='selectable_button' answer='0'>800</tcell><tcell type='selectable_button' answer='0'>900</tcell></trow></line></p><p type='question_step' align='vertical' correctType='single_correct'><line type='text' locationType='top'>Among 200 and 300 which number is more closest to 242</line><line type='table'><trow><tcell type='selectable_button' answer='1'>200</tcell><tcell type='selectable_button' answer='0'>300</tcell></trow></line></p></body>";
			return "<body code='working-rule-for-rounding-to-nearest-hundreds/thousands'> <p type='comprehension' align='vertical'> <line type='text' locationType='top'> Round 242 to the nearest hundred. </line> </p><p type='question_step' align='vertical' correctType='single_correct'> <line type='text' locationType='top'>Among 200 and 300 which number is more closest to 242</line> <line type='table'> <trow> <tcell type='selectable_button' answer='1'>200</tcell> <tcell type='selectable_button' answer='0'>300</tcell> </trow> </line> </p></body>";
		}
		return "";
	}
}