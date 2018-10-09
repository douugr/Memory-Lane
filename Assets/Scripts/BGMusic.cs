using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour {


    private static BGMusic instance = null;

    public static BGMusic Instance
    {
        get { return instance; }
    }
    public void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
            GetComponent<AudioSource>().Play();
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
