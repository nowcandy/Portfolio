using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMng : MonoBehaviour
{
    public static AudioMng Instance;

    public List<AudioSource> Bgms;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void BgmPause(string bgm)
    {
        Bgms.Find(x => x.name == bgm).Pause();
    }

    public void BgmOn(string bgm)
    {
        Bgms.Find(x => x.name == bgm).Play();
    }

    public void BgmOff(string bgm)
    {
        Bgms.Find(x => x.name == bgm).Stop();
    }
}
