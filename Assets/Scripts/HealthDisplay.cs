using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [Header("Dynamic")]
    private TextMeshProUGUI gt;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        gt.text = "Health: " + player.GetComponent<PlayerMovement>().GetHealth();
    }
}
