using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLineDisplay  {

	//Prefabs
	public GameObject smallMarkerPF,numberMarkerPF;

	//ScreenValues
	public float screenDimensionHeight{get; set;}
	public float screenDimensionWidth{get; set;}
	public float screenPadding{get; set;}
	public float startYPosition{get; set;}
	public Vector2 edgePadding{ get; set; }

	//Question Values
	public int IntegerCount{get;set;}
//	public int endInteger{get;set;}
	public int numberBreak{get;set;}

	//To be calculated
	public float cellWidth{get; set;}
	public float allocatedHeight{get; set;}
	public float cellHeight{get; set;}
	public int totalBreakerCount{get;set;}

	//Post Questions
	public float correctAnswer{get;set;}
	public float errorTolerance{get; set;}
	public float userSelectorYPosition{get; set;}
	public float correctSelectorYPosition{get; set;}

	//Constructor
	public NumberLineDisplay(){
	}
	public void defaultValues(){
		screenDimensionHeight=936f; screenDimensionWidth=520f;
		IntegerCount=10; numberBreak=5; correctAnswer = 5f;
		screenPadding=0f; startYPosition = -screenDimensionHeight/2;
		errorTolerance = 25f;edgePadding= new Vector2(70f,111f);
	}
	public void initNumberLineCalculations(){
		//To be calculated
		cellWidth = 40f;
		allocatedHeight = (screenDimensionHeight / 2) - screenPadding + Mathf.Abs(startYPosition)-edgePadding.x;
		totalBreakerCount = 1+(numberBreak*(IntegerCount-1));
		cellHeight = allocatedHeight/totalBreakerCount;
	}


}
