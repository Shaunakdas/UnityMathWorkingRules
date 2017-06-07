using System;
using UnityEngine;
using System.Collections.Generic;
public class BasicGOOperation : MonoBehaviour{	
	//Base methods
	/// <summary>
	/// Get Child GameObject of given name
	/// </summary>
	/// <param name="parentGameObject">Parent GameObject</param>
	/// <param name="withName">Name of child GameObject</param>
	static public GameObject getChildGameObject(GameObject parentGameObject, string withName) {
		//Author: Isaac Dart, June-13.
		Transform[] ts = parentGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}
	/// <summary>
	/// Instantiate NGUI Child GameObject with a given prefab and parent transform
	/// </summary>
	/// <param name="parent">Parent Transform</param>
	/// <param name="prefab">Prefab for child GameObject</param>
	public static GameObject InstantiateNGUIGO(GameObject prefab, Transform parent) {
		return NGUITools.AddChild(parent.gameObject,prefab);
		//		return (GameObject)Instantiate(prefab,parent);
	}
	/// <summary>
	/// Instantiate NGUI Child GameObject with a given prefab, parent transform and a given name
	/// </summary>
	/// <param name="parent">Parent Transform</param>
	/// <param name="prefab">Prefab for child GameObject</param>
	public static GameObject InstantiateNGUIGO(GameObject prefab, Transform parent, string name) {
		GameObject child = InstantiateNGUIGO (prefab, parent);
		child.name = name;
		return child;
		//		return (GameObject)Instantiate(prefab,parent);
	}
	/// <summary>
	/// Instantiate Unity GUI Child GameObject with a given prefab and parent transform
	/// </summary>
	/// <param name="parent">Parent Transform</param>
	/// <param name="prefab">Prefab for child GameObject</param>
	static public GameObject InstantiateUnityGO(GameObject prefab, Transform parent) {
		return (GameObject)Instantiate(prefab,parent);
		//		return (GameObject)Instantiate(prefab,parent);
	}
	/// <summary>
	/// Delete all GameObjects in a given list and clear list also.
	/// </summary>
	/// <param name="GOList">List of GameObjects</param>
	public static void destroyGOList(List<GameObject> GOList){
		GOList.ForEach (imageObject => Destroy (imageObject));
		GOList.Clear ();
	}
	/// <summary>
	/// Delete all child GameObjects of a given parent GameObject.
	/// </summary>
	/// <param name="GO">Parent GameObject</param>
	public static void destroyChildGOList(GameObject GO){
		foreach (Transform child in GO.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
	/// <summary>
	/// Get size of text based on its length.
	/// </summary>
	/// <param name="text">text</param>
	public static float getTextSize(string text){
		TextGenerationSettings settings = new TextGenerationSettings();
		settings.textAnchor = TextAnchor.MiddleCenter;
		settings.color = Color.red;
		settings.generationExtents = new Vector2(500.0F, 200.0F);
		settings.pivot = Vector2.zero;
		settings.richText = true;
		settings.font = Resources.Load<Font>("Arial");
		settings.fontSize = 70;
		settings.fontStyle = FontStyle.Normal;
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		TextGenerator generator = new TextGenerator();
		generator.Populate(text, settings);
		float width = generator.GetPreferredWidth(text, settings);
		Debug.Log("Preferred width of " +text +" :"+ width);
		return width;
	}
	/// <summary>
	/// Get size of text based on its length of text.
	/// </summary>
	/// <param name="text">text</param>
	public static float getNGUITextSize(string text){
		float width =text.ToCharArray().Length*25f;
		Debug.Log("Preferred width based on size of " +text +" :"+ width);
		return width;
	}
	/// <summary>
	/// Get random text generated from a list of all small cap letters.
	/// </summary>
	public static char GetRandomLetter()
	{
		string chars = "abcdefghijklmnopqrstuvwxyz";
		System.Random rand = new System.Random();

		int num = rand.Next(0, chars.Length -1);
		return chars[num];
	}
	/// <summary>
	/// Repositions all the table and grid in children GameObjects
	/// </summary>
	public static void RepositionChildTables(GameObject GO){
		foreach (UITable table in GO.GetComponentsInChildren<UITable>()){
			table.Reposition ();
		}
		foreach (UIGrid grid in GO.GetComponentsInChildren<UIGrid>()){
			grid.Reposition ();
		}
	}
	/// <summary>
	/// Repositions all the table and grid in all parents GameObjects
	/// </summary>
	public static void RepositionParentTables(GameObject GO){
		foreach (UITable table in GO.GetComponentsInParent<UITable>()){
			table.Reposition ();
		}
		foreach (UIGrid grid in GO.GetComponentsInParent<UIGrid>()){
			grid.Reposition ();
		}
	}


}


