using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerStart : MonoBehaviour
{

    public Text timerText;
    int time;

    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     System.Collections.IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(5);
        time = 5;
        timerText.text = "" + time;
        audioSource.Play(0);

        yield return new WaitForSeconds(1);
        time = 4;
        timerText.text = "" + time;
        audioSource.Play(0);


        yield return new WaitForSeconds(1);
        time = 3;
        timerText.text = "" + time;
        audioSource.Play(0);


        yield return new WaitForSeconds(1);
        time = 2;
        timerText.text = "" + time;
        audioSource.Play(0);


        yield return new WaitForSeconds(1);
        time = 1;
        timerText.text = "" + time;
        audioSource.Play(0);

        yield return new WaitForSeconds(1);
        timerText.text = "GO";
    }
}
