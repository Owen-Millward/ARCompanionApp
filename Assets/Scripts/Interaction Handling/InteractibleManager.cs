using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

/// <summary>
/// InteractibleManager keeps tracks of which GameObject
/// is currently in focus.
/// </summary>
public class InteractibleManager : Singleton<InteractibleManager>
{
    public GameObject FocusedGameObject { get; private set; }

    private GameObject oldFocusedGameObject = null;

    void Start()
    {
        FocusedGameObject = null;
    }

    void Update()
    {
        oldFocusedGameObject = FocusedGameObject;

        if (GazeManager.Instance)
        {
            RaycastHit hitInfo = GazeManager.Instance.HitInfo;
            if (hitInfo.collider != null)
            {
                FocusedGameObject = hitInfo.collider.gameObject;
            }
            else
            {
                FocusedGameObject = null;
            }
        }
        else
        {
            FocusedGameObject = null;
        }

        if (FocusedGameObject != oldFocusedGameObject)
        {
            ResetFocusedInteractible();

            if (FocusedGameObject != null)
            {
                if (FocusedGameObject.GetComponent<Interactible>() != null)
                {
                  //  FocusedGameObject.SendMessage("GazeEntered");
                }
				if (FocusedGameObject.GetComponent<GazeCharge>() != null)
				{
				//	FocusedGameObject.SendMessage("GazeEntered");
				}
            }
        }
    }

    private void ResetFocusedInteractible()
    {
        if (oldFocusedGameObject != null)
        {
            if (oldFocusedGameObject.GetComponent<Interactible>() != null)
            {
               // oldFocusedGameObject.SendMessage("GazeExited");
            }
			if (oldFocusedGameObject.GetComponent<GazeCharge>() != null)
			{
				//oldFocusedGameObject.SendMessage("GazeExited");
			}
        }
    }
}