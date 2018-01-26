using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEXFinish : TexDrawLib.TEXDrawMeshEffectBase {
	public bool tableReposition=false;
	public  void triggerReposition(){
		tableReposition = true;
	}
	#if !(UNITY_5_2_1 || UNITY_5_2_2)
	public override void ModifyMesh(VertexHelper h) {
		
		if (tableReposition) {
			Debug.Log (gameObject.name+": ModifyMesh called. Repositioning ParentTable");
			if(gameObject.transform.parent.gameObject.GetComponents<UITable> ().Length>0)
				gameObject.GetComponentInParent<UITable> ().Reposition ();
			if(gameObject.transform.parent.parent.gameObject.GetComponents<UITable> ().Length>0)
				gameObject.transform.parent.parent.gameObject.GetComponent<UITable> ().Reposition ();
			tableReposition = false;
		}
	}
	#endif
}
