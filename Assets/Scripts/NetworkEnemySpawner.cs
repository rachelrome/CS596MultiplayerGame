using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkEnemySpawner : NetworkBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject enemyPrefab;

    private const int MaxPrefabCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnEnemyStart;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnEnemyStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnEnemyStart;
        NetworkObjectPool.Singleton.InitializePool();
        for (int i = 0; i < 5; i++){
            SpawnEnemyServerRPC();
        }

        StartCoroutine(SpawnOverTime());
    }

    [ServerRpc]
    private void SpawnEnemyServerRPC()
    {
        //var instance = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        //var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        //instanceNetworkObject.Spawn();

        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        obj.GetComponent<Enemy_Behavior>().enemyPrefab = enemyPrefab;
        obj.GetComponent<Enemy_Behavior>().ResetEnemy();
        obj.GetComponent<SpriteRenderer>().color = Color.white;

        if (!obj.IsSpawned)
        {
            obj.Spawn(true);
        }
        
    }

    private IEnumerator SpawnOverTime()
    {
        while (NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            yield return new WaitForSeconds(2f);
            if(NetworkObjectPool.Singleton.GetCurrentPrefabCount(enemyPrefab) < MaxPrefabCount)
            SpawnEnemyServerRPC();
        }
    }
    
}
