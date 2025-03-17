using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public TMP_Text ethicalScore;    
    public GameObject endgamePanel;    
    private DisplayEmail displayEmail;
    private PillarCheck pillarCheck;
    public GameObject historyPanel;
    public float score = 0;

    // Estat�sticas de jogo
    private int emailsAceitos = 0;
    private int emailsRejeitados = 0;
    private int decisoesCorretas = 0;
    private int decisoesErradas = 0;
    private int pilaresCorretos = 0;
    private int penalizacoes = 0;
    private float totalPenalizacoes = 0;
    private float totalPontosFeitos = 0;
    private float tempoTotalDecisoes = 0;
    private float tempoTotalJogo = 0;
    private int numeroDecisoes = 0;

    private GameObject statsPanel; // Refer�ncia ao painel de estat�sticas
    private bool zerouJogo = false;

    private void Awake() {
        endgamePanel.SetActive(false);
    }

    void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();
        pillarCheck = FindObjectOfType<PillarCheck>();        
        tempoTotalJogo = 0;
        UpdateScoreText();                        
    }

    void Update() {
        tempoTotalJogo += Time.deltaTime;
        UpdateScoreText();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            MostrarEstatisticasUI();
        }
    }

    public void AddScore(float value) {
        score += value;
        UpdateScoreText();

        if (value < 0) {
            totalPenalizacoes += value;
        } else {
            totalPontosFeitos += value;
        }

        // Verifica se o jogo atingiu 500 pontos para exibir estat�sticas finais e "FIM DE JOGO"
        if (score >= 500 && zerouJogo == false) {
            StartCoroutine(Wait(1.5f));
            zerouJogo = true;
            MostrarEstatisticasUI();
            endgamePanel.SetActive(true);            
            if (pillarCheck.gameObject.activeInHierarchy == true) {
                pillarCheck.gameObject.SetActive(false);
            }
            if (historyPanel.activeSelf == true) {
                historyPanel.SetActive(false);
            }
            if (displayEmail.IsEmailOpen() == true) {
                StartCoroutine(displayEmail.CloseEmail(.5f));
            }
        }
    }

    private void UpdateScoreText() {
        ethicalScore.text = "Placar �tico = " + score;
    }

    // M�todos para registrar estat�sticas
    public void RegistrarEmailAceito(bool foiCorreto) {
        emailsAceitos++;
        if (foiCorreto) {
            decisoesCorretas++;
        } else {
            decisoesErradas++;
        }
    }

    public void RegistrarEmailRejeitado(bool foiCorreto) {
        emailsRejeitados++;
        if (foiCorreto) {
            decisoesCorretas++;
        } else {
            decisoesErradas++;
        }
    }

    public void RegistrarPilarCorreto() {
        pilaresCorretos++;
    }

    public void RegistrarPenalizacao() {
        penalizacoes++;
    }

    public void RegistrarTempoDecisao(float tempo) {
        tempoTotalDecisoes += tempo;
        numeroDecisoes++;
    }

    private bool isShowingStats = false;

    private void MostrarEstatisticasUI() {
        if (isShowingStats) {
            // Remove o painel de estat�sticas se j� estiver sendo exibido
            Destroy(statsPanel);
            isShowingStats = false;
        } else {
            float tempoMedio = numeroDecisoes > 0 ? tempoTotalDecisoes / numeroDecisoes : 0;

            TimeSpan tempoTotal = TimeSpan.FromSeconds(tempoTotalJogo);
            string tempoTotalFormatado = tempoTotal.ToString(@"hh\:mm\:ss");

            // Cria o painel de estat�sticas
            statsPanel = new GameObject("StatsPanel");
            Canvas canvas = statsPanel.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(statsPanel.transform, false);
            RectTransform panelRectTransform = panel.AddComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            panelRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
            panelRectTransform.sizeDelta = new Vector2(500, 400);

            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0.8f, 0.8f, 0.8f, 0.9f);

            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(panel.transform, false);
            RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
            textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            textRectTransform.pivot = new Vector2(0.5f, 0.5f);
            textRectTransform.sizeDelta = new Vector2(500, 400);

            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = $"=== Estat�sticas ===\n\n" +
                        $"Tempo de jogo - {tempoTotalFormatado}\n" +
                        $"E-mails aceitos: {emailsAceitos}\n" +
                        $"E-mails rejeitados: {emailsRejeitados}\n" +
                        $"Decis�es corretas: {decisoesCorretas}\n" +
                        $"Decis�es erradas: {decisoesErradas}\n" +
                        $"Pilares identificados corretamente: {pilaresCorretos}\n" +
                        $"Pontos totais: {totalPontosFeitos}\n" +
                        $"Total em penaliza��es: {totalPenalizacoes}\n";
            text.fontSize = 28;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;
            text.verticalAlignment = VerticalAlignmentOptions.Top;

            isShowingStats = true;
        }
    }

    public void ContinueGame() {        
        // Oculta o texto "FIM DE JOGO" e o bot�o "Continue"
        endgamePanel.SetActive(false);
        

        // Remove o painel de estat�sticas
        if (statsPanel != null) {
            Destroy(statsPanel);
            isShowingStats = false;
        }
    }

    private IEnumerator Wait(float delay) {
        yield return new WaitForSeconds(delay);        
    }

    public float GetCurrentScore() {
        return score;
    }
}