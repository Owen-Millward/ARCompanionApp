using UnityEngine;
using System;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using Pathfinding.Serialization.JsonFx;
using Unity3dAzure.AppServices;
using UnityEngine.UI;
//using Tacticsoft;
//using Prefabs;
using UnityEngine.SceneManagement;

public class PowerUpController : MonoBehaviour//, ITableViewDataSource 
{
	// This script handles the communication with the azure app service for the HoloLens app

	// App URL
	[SerializeField] // Forces unity to serialise this field
	private string APP_URL = "http://dangerzoneabertay.azurewebsites.net";

	// App Service REST Client
	private MobileServiceClient _client;

	// App Service Tables defined using a DataModel
	private MobileServiceTable<SpawnFlag> _SFtable;

	// Local spawn flag models for updating the server
	SpawnFlag sheild = new SpawnFlag();
	SpawnFlag shock = new SpawnFlag();
	SpawnFlag purge = new SpawnFlag();

	// Prefabs
	public GameObject AzureTree;
	public GameObject buttonPrefab;

	// Bool for spawning button
	bool spawnButton;

	// Use this for initialization
	void Start () 
	{
		// Create App Service client (Using factory Create method to force 'https' url)
		_client = MobileServiceClient.Create(APP_URL); // new MobileServiceClient(APP_URL);

		// Get App Service 'SpawnFlags' table
		_SFtable = _client.GetTable<SpawnFlag>("SpawnFlags");

		// Initialise local spawnflag model
		sheild.name = "Sheild";
		sheild.flag = false;
		shock.name = "Shockwave";
		shock.flag = false;
		purge.name = "Purge";
		purge.flag = false;

		// Not inserted yet
		spawnButton = false;

		// Insert flags to the spawn flag table
		Insert(_SFtable, sheild);
		Insert(_SFtable, shock);
		Insert (_SFtable, purge);
	}

	// Update is called once per frame
	void Update () 
	{
//		if (spawnButton) 
//		{
//			GameObject newButton = Instantiate(buttonPrefab) as GameObject;
//			newButton.transform.SetParent (canvas.transform, false);
//			spawnButton = false;
//		}
	}

	public void SheildButton()
	{
		sheild.flag = true;
		UpdateFlag (_SFtable, sheild);
	}

	public void ShockButton()
	{
		shock.flag = true;
		UpdateFlag (_SFtable, shock);
	}

	public void PurgeButton()
	{
		purge.flag = true;
		UpdateFlag (_SFtable, purge);
	}

	// REST FUNCTIONS

	public void Lookup(MobileServiceTable<SpawnFlag> table, SpawnFlag item)
	{
		table.Lookup<SpawnFlag>(item.id, OnLookupCompleted);
	}

	private void OnLookupCompleted(IRestResponse<SpawnFlag> response)
	{
		Debug.Log("OnLookupItemCompleted: " + response.Content);
		if (response.StatusCode == HttpStatusCode.OK)
		{
			SpawnFlag item = response.Data;
		}
		else
		{
			ResponseError err = JsonReader.Deserialize<ResponseError>(response.Content);
			Debug.Log("Lookup Error Status:" + response.StatusCode + " Code:" + err.code.ToString() + " " + err.error);
		}
	}

	public void Insert(MobileServiceTable<SpawnFlag> table, SpawnFlag item)
	{ // Inserts new spawn flag into Azure table

		if (Validate(item))
		{
			table.Insert<SpawnFlag>(item, OnInsertCompleted);
		}
	}

	private void OnInsertCompleted(IRestResponse<SpawnFlag> response)
	{
		if (response.StatusCode == HttpStatusCode.Created)
		{
			Debug.Log( "OnInsertItemCompleted: " + response.Data );

			// Tree button or music button?
			if (response.Data.name == "Sheild")
				sheild = response.Data; // If inserted successfully, azure will return an ID
			else if (response.Data.name == "Shockwave")
				shock = response.Data;
			else if (response.Data.name == "Purge")
				purge = response.Data;

			// Is this the first insert?
			if (!spawnButton)
			{
				// Instantiate button
//				spawnButton = true;
			}
		}
		else
		{
			Debug.Log("Insert Error Status:" + response.StatusCode + " Uri: "+response.ResponseUri );
		}
	}

	public void Delete(MobileServiceTable<SpawnFlag> table, SpawnFlag item)
	{
		// Deletes spawn flag from azure table
		if (Validate(item))
		{
			table.Delete<SpawnFlag>(item.id, OnDeleteCompleted);
		}

	}

	private void OnDeleteCompleted(IRestResponse<SpawnFlag> response)
	{
		if (response.StatusCode == HttpStatusCode.OK)
		{
			Debug.Log("OnDeleteItemCompleted");
		}
		else
		{
			Debug.Log("Delete Error Status:" + response.StatusCode + " " + response.ErrorMessage + " Uri: " + response.ResponseUri);
		}
	}

	public void UpdateFlag(MobileServiceTable<SpawnFlag> table, SpawnFlag item)
	{
		// Updates item in the table
		if (Validate(item))
		{
			table.Update<SpawnFlag>(item, OnUpdateFlagCompleted);
		}
	}

	private void OnUpdateFlagCompleted(IRestResponse<SpawnFlag> response)
	{
		if (response.StatusCode == HttpStatusCode.OK)
		{
			Debug.Log("OnUpdateItemCompleted: " + response.Content);
		}
		else
		{
			Debug.Log("Update Error Status:" + response.StatusCode + " " + response.ErrorMessage + " Uri: " + response.ResponseUri);
		}
	}

	public void Read(MobileServiceTable<SpawnFlag> table)
	{
		table.Read<SpawnFlag>(OnReadCompleted);
	}

	private void OnReadCompleted(IRestResponse<List<SpawnFlag>> response)
	{
		if (response.StatusCode == HttpStatusCode.OK)
		{
			Debug.Log("OnReadCompleted data: " + response.ResponseUri +" data: "+ response.Content);
			List<SpawnFlag> items = response.Data;
			Debug.Log("Read items count: " + items.Count);
		}
		else
		{
			Debug.Log("Read Error Status:" + response.StatusCode + " Uri: "+response.ResponseUri );
		}
	}

	// Checks if flag is valid before sending
	private bool Validate(SpawnFlag flag)
	{
		bool isUsernameValid = true;//, isScoreValid = true;
		// Validate username
		if (String.IsNullOrEmpty(flag.name))
		{
			isUsernameValid = false;
			Debug.Log("Error, player username required");
		}

		return (isUsernameValid);// && isScoreValid);
	}
		
}