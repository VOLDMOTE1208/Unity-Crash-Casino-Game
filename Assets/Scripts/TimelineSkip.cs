using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

public class TimelineSkip : MonoBehaviour
{
    public PlayableDirector director;
    public KeyCode key = KeyCode.Space;
    public float time;

    bool _skipped;

    void Update()
    {
        if(!_skipped && Input.GetKeyDown(key))
        {
            _skipped = true;

            director.time = time;
        }
    }
}
