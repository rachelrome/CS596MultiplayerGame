using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButtonBehavior : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] spriteArray;

    public GameObject platformHolder;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sr.sprite = spriteArray[1];
            platformHolder.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sr.sprite = spriteArray[0];
            platformHolder.SetActive(false);
        }
    }
}
