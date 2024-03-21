using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GemDisplay : MonoBehaviour
{
    [Header("Dynamic")] 
    private TextMeshProUGUI gt;

    private AudioSource source;
    public AudioClip allGemsCollectedSound;

   
    public int gemCount = 0;
    private bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        gt = GetComponent<TextMeshProUGUI>();  
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        gt.text = "Gems: " + gemCount;

        if (gemCount == 5 && !isTriggered)
        {
            isTriggered = true;

            source.PlayOneShot(allGemsCollectedSound);
        }
    }

    public void IncreaseGemCount()
    {
        gemCount += 1;
    }
}
