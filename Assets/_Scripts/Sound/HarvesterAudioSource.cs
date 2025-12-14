using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterAudioSource : MonoBehaviour
{
    public GameObject allAudioSource;

    private void OnEnable()
    {
        FindAudioSource();
    }

    [System.Obsolete]
    public void FindAudioSource()
    {
        allAudioSource = transform.Find("All Audio Sources").gameObject;
    }
}
