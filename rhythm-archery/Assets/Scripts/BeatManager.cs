using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    private AudioSource audioData;
    private float lastPlayheadPosition;
    private Beatmap beatmap;

    [SerializeField]
    private string hitPath;

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(hitPath))
        {
            string hitmap = File.ReadAllText(hitPath);
            List<float> hits = JsonUtility.FromJson<List<float>>("{\"hits:\":" + hitmap + "}");

            Debug.Log(hits.Count);

            beatmap = new Beatmap(hits);
        }

        audioData = GetComponent<AudioSource>();
        audioData.Play();
        lastPlayheadPosition = audioData.time;
    }

    // Update is called once per frame
    void Update()
    {
        beatmap.Step(lastPlayheadPosition, audioData.time);
        lastPlayheadPosition = audioData.time;
    }
}
