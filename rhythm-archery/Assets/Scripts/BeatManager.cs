using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatManager : MonoBehaviour
{
    private AudioSource audioData;
    private float lastPlayheadPosition;
    public Beatmap beatmap;

    private Stack<float> beatStack = new Stack<float>();

    [SerializeField]
    private string hitPath;

    [SerializeField]
    private float tolerance = 0.5f;

    public BeatPlaybackState State
    {
        get
        {
            if (beatStack.Count == 0) return BeatPlaybackState.Standby;
            return BeatPlaybackState.Open;
        }
    }

    public float CurrentOffset
    {
        get
        {
            if (beatStack.Count == 0) return -1;
            return Mathf.Abs(beatStack.Peek() - audioData.time) / tolerance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(hitPath))
        {
            string hitmapJSON = File.ReadAllText(hitPath);
            Hitmap hitmap = JsonUtility.FromJson<Hitmap>(hitmapJSON);

            beatmap = new Beatmap(hitmap.hits);
            beatmap.AddOffset(-tolerance, (sender, e) => { beatStack.Push(((BeatEvent)sender).Timestamp); });
            beatmap.AddOffset(tolerance, (sender, e) => { beatStack.Pop(); });
            beatmap.AddOffset(0f, (sender, e) => { Debug.Log("hi maffie"); });
        }

        audioData = GetComponent<AudioSource>();
        lastPlayheadPosition = audioData.time;

        audioData.PlayDelayed(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioData.isPlaying) return;

        beatmap.Step(lastPlayheadPosition, audioData.time);
        lastPlayheadPosition = audioData.time;
    }

    [System.Serializable]
    private struct Hitmap
    {
        public List<float> hits;

        public Hitmap(List<float> hits)
        {
            this.hits = hits;
        }
    }

    public enum BeatPlaybackState
    {
        Standby,
        Open
    }
}
