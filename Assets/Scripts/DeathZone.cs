using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip gameOver;//assign
    public MainManager Manager;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        audioSource.PlayOneShot(gameOver);
        Destroy(other.gameObject);
        Manager.GameOver();
    }
}
