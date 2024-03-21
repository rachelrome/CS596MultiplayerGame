using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] spriteArray;

    public GameObject movingPlatform;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sr.sprite = spriteArray[1];
            movingPlatform.GetComponent<MovingPlatform>().SetSpeed(3.25f);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sr.sprite = spriteArray[0];
            movingPlatform.GetComponent<MovingPlatform>().SetSpeed(0);
        }
    }
}
