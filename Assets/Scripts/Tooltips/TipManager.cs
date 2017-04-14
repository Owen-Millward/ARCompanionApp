using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] ToolTips = new GameObject[10];
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
           
	}

    public void ShowTip(int tipNumber)
    {
		if (ToolTips [tipNumber] != null && ToolTips [tipNumber].GetComponent<MeshRenderer> ().enabled == false) {
			ToolTips [tipNumber].GetComponent<MeshRenderer> ().enabled = true;
			ToolTips [tipNumber].GetComponent<ToolTip> ().playAudio ();
		}
    }

    public void DismissTip(int tipNumber)
    {
		if (ToolTips [tipNumber] != null) {
			Destroy (ToolTips [tipNumber]);
		}
    }

	public void DismissAll()
	{
		foreach(GameObject tip in ToolTips)
		if (tip != null) {
			Destroy (tip);
		}
	}



}
