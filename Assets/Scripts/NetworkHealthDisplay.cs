using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkHealthDisplay : NetworkBehaviour
{
    public NetworkVariable<int> HealthPoint = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        HealthPoint.Value = 100;
    }
}
