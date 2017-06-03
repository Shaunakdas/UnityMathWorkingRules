using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationLine  : Line {
	public Type CombinationType;
	public bool OutputVisible{ get; set; }
	public int CorrectAnswer{get; set;}
//	public List<Row> RowList {get; set;}
	//Constructor
	public CombinationLine(string outputVisible, string correctAnswer, string type){	
		OutputVisible = (outputVisible=="1")?true : false;
		CorrectAnswer = int.Parse (correctAnswer);
		switch (type) {
		case "combination_product": 
			CombinationType = Type.CombinationProduct;
			break;
		case "combination_product_sum": 
			CombinationType = Type.CombinationProductSum;
			break;
		case "combination_sum": 
			CombinationType = Type.CombinationSum;
			break;
		}
	}
}
