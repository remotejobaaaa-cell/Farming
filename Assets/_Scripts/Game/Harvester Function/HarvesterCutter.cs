using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterCutter : MonoBehaviour
{
    public GameObject dustParticle;
    private bool isSoundPlaying;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crops"))
        {
            other.gameObject.SetActive(false);
            dustParticle.GetComponent<ParticleSystem>().Play();
            if (!this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
                isSoundPlaying = true;
            }
            if (isSoundPlaying == true)
            {
                isSoundPlaying = false;
                Invoke("DisableAudio", 5f);
            }
        }
    }
    
    public void DisableAudio()
    {
        this.GetComponent<AudioSource>().Stop();
    }
}
