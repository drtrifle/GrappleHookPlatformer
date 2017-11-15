using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private Text timerText;
    private float time = 0f;
    private bool isTimerRunning = true;

    // Use this for initialization
    private void Start() {
        timerText = GetComponent<Text>();
    }

    private void Update() {

        if (isTimerRunning) {
            time += Time.deltaTime;
        }

        timerText.text = FormatTime(time);
    }

    private string FormatTime(float time) {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }

    public void StartTimer() {
        isTimerRunning = true;
    }

    public void StopTimer() {
        isTimerRunning = false;
    }
}
