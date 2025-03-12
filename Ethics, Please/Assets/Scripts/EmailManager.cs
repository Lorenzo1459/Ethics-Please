using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmailManager : MonoBehaviour {
    public ProposalDatabase proposalDatabase;
    private SFXManager sFXManager;
    public TMP_Text emailCountText; // Contador de e-mails no botão
    public GameObject historyPanel; // Painel do histórico
    public Transform historyContent; // Parent dos botões do histórico
    public Button historyButtonPrefab; // Prefab do botão de histórico

    private Queue<int> emailQueue = new Queue<int>();
    private List<int> indexList = new List<int>();
    private List<EmailHistoryEntry> emailHistory = new List<EmailHistoryEntry>(); // Histórico de e-mails   
    private Dictionary<int, Color> emailButtonColors = new Dictionary<int, Color>();

    private int emailCount = 0;
    private float emailCooldown = 10f;
    private DisplayEmail displayEmail;
    private ProposalData currentEmailData;

    private void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();
        sFXManager = FindObjectOfType<SFXManager>();

        sFXManager.SetVolume(.3f);

        for (int i = 0; i < proposalDatabase.proposals.Count; i++) {
            indexList.Add(i);
        }

        ShuffleEmailIndices();
        AddEmailsToQueue();

        StartCoroutine(DelayedFirstEmail(3f));
        StartCoroutine(GenerateEmailsOverTime());

        if (historyPanel == null || historyContent == null) {
            Debug.LogError("Referência de historyPanel ou historyContent está nula!");
        }
    }

    private void Update() {
        /*if (Input.GetKeyDown(KeyCode.Space)) {
            ShowNextEmail();
        }
        if (displayEmail.IsEmailOpen() == true && Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine(displayEmail.CloseEmail(0f));
        }*/
    }

    private void ShuffleEmailIndices() {
        for (int i = 0; i < indexList.Count; i++) {
            int tmp = indexList[i];
            int r = Random.Range(i, indexList.Count);
            indexList[i] = indexList[r];
            indexList[r] = tmp;
        }
    }

    private void AddEmailsToQueue() {
        foreach (int index in indexList) {
            emailQueue.Enqueue(index);
        }
    }

    private IEnumerator GenerateEmailsOverTime() {
        while (true) {
            yield return new WaitForSeconds(emailCooldown);
            if (emailQueue.Count > 0) {
                emailCount++;
                sFXManager.PlaySFX(0); // 0 - New Email
                UpdateEmailCountUI();
            }
        }
    }

    private void UpdateEmailCountUI() {
        if (emailCountText != null) {
            Transform parentObject = emailCountText.transform.parent;

            if (emailCount > 0) {
                emailCountText.text = $"{emailCount}";
                if (parentObject != null) parentObject.gameObject.SetActive(true);
            } else {
                if (parentObject != null) parentObject.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator DelayedFirstEmail(float delay) {
        yield return new WaitForSeconds(delay);
        emailCount++;
        sFXManager.PlaySFX(0); // 0 - New Email
        UpdateEmailCountUI();
    }

    public void ShowNextEmail() {
        if (displayEmail.IsEmailOpen()) {
            Debug.Log("Feche o e-mail atual antes de abrir outro.");
            return;
        }

        if (emailCount > 0 && emailQueue.Count > 0) {
            int nextEmailIndex = emailQueue.Dequeue();
            currentEmailData = proposalDatabase.proposals[nextEmailIndex];
            displayEmail.DisplayEmailByIndex(nextEmailIndex);
            emailCount--;
            UpdateEmailCountUI();
        } else {
            Debug.Log("Nenhum e-mail disponível no momento.");
        }
    }

    public void RegisterDecision(string selectedText, TipoProblema chosenProblem, string result) {
        if (currentEmailData == null || emailHistory.Count == 0) return;

        List<string> unethicalParts = new List<string>(currentEmailData.trechoAntietico);

        // Atualiza a última entrada do histórico
        emailHistory[emailHistory.Count - 1] = new EmailHistoryEntry(
            currentEmailData.companyName,
            currentEmailData.projectTitle,
            currentEmailData.projectDescription,
            selectedText,
            currentEmailData.tipoProblema,
            chosenProblem,
            result,
            unethicalParts
        );

        //UpdateHistoryUI();
    }

    /*public string HighlightUnethicalParts(string text, List<string> unethicalParts) {
        foreach (string part in unethicalParts) {
            text = text.Replace(part, $"<color=red>{part}</color>");
        }
        return text;
    }*/
    public string HighlightUnethicalParts(string text, List<string> unethicalParts) {
        foreach (string part in unethicalParts) {
            // Wrap the unethical part with <mark> and <color> tags
            text = text.Replace(part, $"<mark=#FF000080><color=red>{part}</color></mark>");
        }
        return text;
    }

    public void ToggleHistoryPanel() {
        if (historyPanel != null) {
            historyPanel.SetActive(!historyPanel.activeSelf);
        }
    }

    public void UpdateHistoryUI() {
        if (historyContent == null) {
            Debug.LogWarning("historyContent está nulo! Certifique-se de que ele foi atribuído no Inspector.");
            return;
        }

        // Desativar os botões antigos sem destruí-los
        foreach (Transform child in historyContent) {
            child.gameObject.SetActive(false);
        }

        // Aguarde um frame antes de recriar os botões (evita erros)
        StartCoroutine(GenerateHistoryButtons());
    }

    private IEnumerator GenerateHistoryButtons() {
        if (historyContent == null) yield break; // Sai da função se historyContent foi destruído

        for (int i = 0; i < emailHistory.Count; i++) {
            int emailIndex = i;
            Button newButton = Instantiate(historyButtonPrefab, historyContent);
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();

            if (buttonText != null) {
                buttonText.text = emailHistory[emailIndex].emailTitleText;
            }

            if (emailButtonColors.TryGetValue(emailIndex, out Color buttonColor)) {
                newButton.GetComponent<Image>().color = buttonColor;
            }

            newButton.onClick.AddListener(() => ShowHistoryEmail(emailIndex));
        }
    }

    private void ShowHistoryEmail(int index) {
        if (index >= 0 && index < emailHistory.Count) {
            displayEmail.DisplayEmailHistory(emailHistory[index]);
        }
    }

    public List<EmailHistoryEntry> GetEmailHistory() {
        return emailHistory;
    }

    public void SaveEmailToHistory(Color buttonColor) {
        int emailIndex = emailHistory.Count;
        emailHistory.Add(new EmailHistoryEntry(
            currentEmailData.companyName,
            currentEmailData.projectTitle,
            currentEmailData.projectDescription,
            "", // Ainda sem seleção
            currentEmailData.tipoProblema,
            TipoProblema.Nenhum, // Sem resposta ainda
            "Pendente",
            currentEmailData.trechoAntietico
        ));

        emailButtonColors[emailIndex] = buttonColor;        
    }

    public void SaveAndUpdateHistory(Color buttonColor) {
        SaveEmailToHistory(buttonColor);
        UpdateHistoryUI();
    }
}

// Classe que representa um e-mail no histórico
public class EmailHistoryEntry {
    public string emailText;
    public string selectedText;
    public TipoProblema correctProblem;
    public TipoProblema chosenProblem;
    public string result;
    public string companyNameText;
    public string emailTitleText;
    public List<string> unethicalParts;

    public EmailHistoryEntry(string companyNameText, string emailTitleText, string emailText, string selectedText, TipoProblema correctProblem, TipoProblema chosenProblem, string result, List<string> unethicalParts) {
        this.emailText = emailText;
        this.selectedText = selectedText;
        this.correctProblem = correctProblem;
        this.chosenProblem = chosenProblem;
        this.result = result;
        this.companyNameText = companyNameText;
        this.emailTitleText = emailTitleText;
        this.unethicalParts = unethicalParts;
    }
}
