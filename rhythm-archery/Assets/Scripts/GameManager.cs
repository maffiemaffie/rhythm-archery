using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArrowThrower))]
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnFire()
    {
        float offset = GetComponent<BeatManager>().CurrentOffset;
        if (offset == -1) return;

        GetComponent<ArrowThrower>().Throw(offset);
    }
}
