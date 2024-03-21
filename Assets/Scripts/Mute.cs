using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mute : MonoBehaviour
{
    bool isMuted;

    private void Start()
    {
        isMuted = false;
    }
    public void MutePressed()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
