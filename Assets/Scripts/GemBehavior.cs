using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GemBehavior : NetworkBehaviour
{
    private AudioSource source;

    //public GemDisplay gemDisplay;
    public AudioClip gemSound;
    private bool isTriggered = false;
    private GemDisplay gemDisplay;

    // Start is called before the first frame update

    void Start()
    {
        source = GetComponent<AudioSource>();
        gemDisplay = GameObject.Find("GemsDisplay").GetComponent<GemDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            gemDisplay.IncreaseGemCount();
            source.PlayOneShot(gemSound);
            Destroy(gameObject, .6f);
        }
    }
}
