using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTrigger : Singleton<MathTrigger> {

	protected MathTrigger () {} // guarantee this will be always a singleton only - can't use the constructor!

	public void InitLevel(){
		GameObject.Find ("UI Root").GetComponent<HTMLParserTest> ().comprehensionBody.nextParaTrigger ();
	}
}
