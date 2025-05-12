using UnityEngine;
using Cinemachine;

public class VirtualCameraSetup : MonoBehaviour
{
    private void Awake()
    {
        // Tag this camera appropriately
        gameObject.tag = "VirtualCamera";
    }

    private void Start()
    {
        // Find the player using tag and the Singleton instance
        GameObject player = GameObject.FindWithTag("Player");
        
        if (player == null)
        {
            player = PlayerMovement.Instance?.gameObject;
            Debug.Log("Looking for player via singleton: " + (player != null ? "Found" : "Not found"));
        }
        
        if (player != null)
        {
            // Get the virtual camera component
            CinemachineVirtualCamera vCam = GetComponent<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                // Set the player as the follow target
                vCam.Follow = player.transform;
                Debug.Log("Camera follow target set to player: " + player.name);
            }
            else
            {
                Debug.LogError("No CinemachineVirtualCamera component found on " + gameObject.name);
            }
        }
        else
        {
            Debug.LogError("Player not found. Make sure it has the 'Player' tag.");
        }
    }
}