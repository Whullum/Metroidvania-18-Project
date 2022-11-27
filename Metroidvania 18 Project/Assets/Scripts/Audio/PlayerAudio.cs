using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AK.Wwise.Event _sfxPlayerDoubleJump;
    public AK.Wwise.Event _sfxPlayerInitialJump;
    public AK.Wwise.Event _sfxPlayerDash;

    public void PostWwiseEvent(AK.Wwise.Event wwiseEvent)
    {
        wwiseEvent.Post(gameObject);
    }
}
