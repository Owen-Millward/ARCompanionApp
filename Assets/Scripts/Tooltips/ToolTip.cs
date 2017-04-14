using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ToolTip : MonoBehaviour, IInputClickHandler {

    [SerializeField]
    [Tooltip("Text to be displayed in the tool tip, end with 'Air Tap To Dismiss' ")]
    private string TextContent;
    [SerializeField]
    [Tooltip("GameObject ToolTip is to be shown above")]
    private Transform HoverAbove;

	[SerializeField]
	private AudioClip Clip;
	private AudioSource audioSource;

    public ToolTip(string textContent_, Transform hoverAbove_)
    {
        TextContent = textContent_;
        HoverAbove = hoverAbove_;
    }

	// Use this for initialization
	void Start () {



        gameObject.transform.position = HoverAbove.transform.position;
        gameObject.transform.position += new Vector3(0,HoverAbove.GetComponentInChildren<MeshRenderer>().bounds.size.y,0);
        gameObject.transform.rotation = HoverAbove.transform.rotation;
        gameObject.transform.Rotate(new Vector3(0, 180, 0));
        

		audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.clip = Clip;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playAudio(){
		
		audioSource.Play ();
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Destroy(gameObject);
    }
}
