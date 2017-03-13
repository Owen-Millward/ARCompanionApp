using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Windows;
using UnityEngine.Networking;
using Unity3dAzure.StorageServices;

/// <summary>
/// This keeps track of the various parts of the recording and text display process.
/// </summary>

[RequireComponent(typeof(AudioSource), typeof(MicrophoneManager))]
public class Communicator : MonoBehaviour
{
	// Azure Storage Service Components
	[SerializeField]
	private string ACC_NAME = "dangerzonecommunication"; // Account Name
	[SerializeField]
	private string KEY = "oxNwpGrXuExAaY33F/4cgcXyA/RxwYFW5oWntusg0DzYePg3ZYyK/D2lxPzIaIHHbr0ckBdcQL5DEWmbUYcodQ=="; // Access Key

	// Storage REST Client and Service
	private StorageServiceClient _client;
	private BlobService _service;

	// Voice Audio Buttons
    [Tooltip("The button to be selected when the user wants to record audio and dictation.")]
    public Button RecordButton;
    [Tooltip("The button to be selected when the user wants to stop recording.")]
    public Button RecordStopButton;
    [Tooltip("The button to be selected when the user wants to play audio.")]
    public Button PlayButton;
    [Tooltip("The button to be selected when the user wants to stop playing.")]
    public Button PlayStopButton;

	// Audio Clips
    [Tooltip("The sound to be played when the recording session starts.")]
    public AudioClip StartListeningSound;
    [Tooltip("The sound to be played when the recording session ends.")]
    public AudioClip StopListeningSound;

    [Tooltip("The icon to be displayed while recording is happening.")]
    public GameObject MicIcon;

	// Audio Sources
    private AudioSource voiceInputAudio;
    private AudioSource startAudio;
    private AudioSource stopAudio;

    public enum Message
    {
        PressMic,
        PressStop,
        SendMessage
    };

    private MicrophoneManager microphoneManager;
	public bool recording = false;

    void Start()
    {
		// Create Storage Service client with name and access key
		_client = new StorageServiceClient(ACC_NAME, KEY);

		// Create Service from client
		_service = _client.GetBlobService();

		// Create Audio Sources 
        voiceInputAudio = gameObject.GetComponent<AudioSource>();
        startAudio = gameObject.AddComponent<AudioSource>();
        stopAudio = gameObject.AddComponent<AudioSource>();
        startAudio.playOnAwake = false;
        startAudio.clip = StartListeningSound;
        stopAudio.playOnAwake = false;
        stopAudio.clip = StopListeningSound;

		// Get Microphone manager
        microphoneManager = GetComponent<MicrophoneManager>();
    }

    void Update()
    {
        // If the audio has stopped playing and the PlayStop button is still active,  reset the UI.
		if (!voiceInputAudio.isPlaying && PlayStopButton.enabled)
        {
            PlayStop();
        }
    }

	private void ConvertAndSend()
	{
		// Convert voice clip to WAV and send to Azure Storage
		string filepath = SaveToWav (voiceInputAudio.clip);

		// Upload to storage service
		UploadAudio ("voiceaudio", filepath);
	}

    public void Record()
    {
       // if (RecordButton.IsOn())
        {
            // Turn the microphone on, which returns the recorded audio.
			voiceInputAudio.clip = microphoneManager.StartRecording();

            // Set proper UI state and play a sound.
            SetUI(true, Message.PressStop, startAudio);

			recording = true;

            //RecordButton.gameObject.SetActive(false);
            //RecordStopButton.gameObject.SetActive(true);
        }
    }

    public void RecordStop()
    {
        //if (RecordStopButton.IsOn())
        {
            // Turn off the microphone.
            microphoneManager.StopRecording();

            // Set proper UI state and play a sound.
            SetUI(false, Message.SendMessage, stopAudio);

			recording = false;

			// Send to Azure
			ConvertAndSend ();

            //PlayButton.SetActive(true);
            //RecordStopButton.SetActive(false);
        }
    }

    public void Play()
    {
       // if (PlayButton.IsOn())
        {
            PlayButton.gameObject.SetActive(false);
            PlayStopButton.gameObject.SetActive(true);

			//voiceInputAudio.Play();
        }
    }

    public void PlayStop()
    {
        //if (PlayStopButton.IsOn())
        {
            PlayStopButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);

			voiceInputAudio.Stop();	
        }
    }

    void ResetAfterTimeout()
    {
        // Set proper UI state and play a sound.
        SetUI(false, Message.PressMic, stopAudio);

       RecordStopButton.gameObject.SetActive(false);
       RecordButton.gameObject.SetActive(true);

		recording = false;
    }

    private void SetUI(bool enabled, Message newMessage, AudioSource soundToPlay)
    {
      
        MicIcon.SetActive(enabled);

        //soundToPlay.Play();
    }

	// Audio/REST Functions
	public string SaveToWav(AudioClip clip)
	{
		string filepath;
		byte[] bytes = WavUtility.FromAudioClip (clip, out filepath, true);
		Debug.Log ("Saved audio bytes: " + bytes.Length + " filepath:" + filepath);
		return filepath;
	}

	public void UploadAudio(string containerName, string filename)
	{
		if (!filename.EndsWith (".wav")) 
		{
			Debug.Log ("File is not .wav format");
			return;
		}
		byte[] bytes = File.ReadAllBytes (filename);
		StartCoroutine(_service.PutAudioBlob (UploadAudioCompleted, bytes, containerName, filename, "audio/wav"));
	}

	private void UploadAudioCompleted(RestResponse response)
	{
		if (response.IsError)
		{
			Debug.Log("Insert Audio Error Status: " + response.StatusCode + " Message: " + response.ErrorMessage);
		}
		else
		{
			Debug.Log ("Insert Audio Completed: " + response.Content);
		}
	}

	public void DeleteAudioClip(string url)
	{
		StartCoroutine (DeleteAudioURL (url));
	}

	public IEnumerator DeleteAudioURL(string url)
	{
		UnityWebRequest www = UnityWebRequest.Delete (url);
		yield return www.Send ();
		if (www.isError) {
			Debug.Log ("Error Deleting Audio from URL: " + www.error);
		} 
		else 
		{
			Debug.Log ("Deleted Audio Clip " + url);
		}
	}
}