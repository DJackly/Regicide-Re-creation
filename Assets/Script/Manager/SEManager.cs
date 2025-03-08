using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;
    public List<AudioClip> SEList = new List<AudioClip>();
    public AudioSource audioSource;
    public AudioClip BGM;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        audioSource.clip = BGM;
        audioSource.Play();
    }
    public void ClickButton()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(SEList[0]);
    }
    public void MovingCard()
    {
        audioSource.volume = 0.7f;
        audioSource.PlayOneShot(SEList[1]);
    }
    public void ClickCard()
    {
        audioSource.volume = 0.7f;
        audioSource.PlayOneShot(SEList[2]);
    }
    public void UseItem()
    {
        audioSource.volume = 0.6f;
        audioSource.PlayOneShot(SEList[3]);
    }
    public void ActivateSuit()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(SEList[4]);
    }
    public void Attack()
    {
        audioSource.volume = 0.9f;
        audioSource.PlayOneShot(SEList[5]);
    }
    public void BossAttack()
    {
        audioSource.volume = 0.6f;
        audioSource.PlayOneShot(SEList[6]);
    }
    public void BossAttackReady()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(SEList[7]);
    }
    public void Victory()
    {
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(SEList[8]);
    }
    public void Defeat()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(SEList[9]);
    }
    public void GemCollected()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(SEList[10]);
    }
    public void Break()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(SEList[11]);
    }
    public void PauseBGM() { audioSource.Pause();  }
    public void ContinueBGM() { audioSource.UnPause();  }
}
