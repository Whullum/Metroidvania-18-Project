using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header ("Player Movement Audio")]
    public AK.Wwise.Event _sfxPlayerDoubleJump;
    public AK.Wwise.Event _sfxPlayerInitialJump;
    public AK.Wwise.Event _sfxPlayerDash;

    [Header("Player Gun Audio")]
    public AK.Wwise.Event _sfxPlayerGunShot;
    public AK.Wwise.Event _sfxPlayerReload;


    [Header("Player Gun Switches")]
    public AK.Wwise.Switch _switchGunConstantBurst;
    public AK.Wwise.Switch _switchGunShotgun;
    public AK.Wwise.Switch _switchGunArcStream;

    public void PostWwiseEvent(AK.Wwise.Event wwiseEvent)
    {
        wwiseEvent.Post(gameObject);
    }

    public void SetWwiseSwitch(AK.Wwise.Switch wwiseSwitch)
    {
        wwiseSwitch.SetValue(gameObject);
    }
}
