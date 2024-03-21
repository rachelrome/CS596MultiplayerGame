using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class NetworkCameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            cameraHolder.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            cameraHolder.transform.position = transform.position + offset;
        }
    }
}
