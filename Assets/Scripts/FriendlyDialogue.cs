using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyDialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueBox.SetActive(true);
            text.SetActive(true);
        }
    }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueBox.SetActive(false);
        text.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueBox.SetActive(true);
            text.SetActive(true);
        }
    }
}
