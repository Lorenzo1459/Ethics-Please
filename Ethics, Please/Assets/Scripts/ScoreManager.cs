using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public TMP_Text ethicalScore;
    public float score = 0;


    void Start() {
        UpdateScoreText();
    }

    void Update() {
        UpdateScoreText();
    }

    public void AddScore(float value) {
        score += value;
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        ethicalScore.text = "Ethical Score = " + score;
    }
}
