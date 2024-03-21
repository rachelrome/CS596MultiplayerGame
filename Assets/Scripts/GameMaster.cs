using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    [SerializeField] private Vector2 lastCheckpointPos;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector2 GetLastCheckpointPos()
    {
        return lastCheckpointPos;
    }

    public void SetLastCheckpointPos(Vector2 pos)
    {
        lastCheckpointPos = pos;
    }
}
