﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEmail : MonoBehaviour {
    private EmailManager emailManager;
    private ScoreManager scoreManager;
    private SFXManager sFXManager;

    public TMP_Text companyNameText;
    public TMP_Text projectTitleText;
    public TMP_InputField projectDescriptionText;
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f;

    private Image background;
    private Color defaultColor;

    private int currentEmailIndex = -1;
    public ProposalData currentProposal;
    public PillarCheck pillarCheck;

    public Button acceptButton;
    public Button rejectButton;
    public Button closeButton;
    public GameObject resultFeedback;
    private Coroutine resultFeedbackCoroutine;
    private bool isProcessingDecision = false; // Variável de controle

    void Awake() {
        emailManager = FindObjectOfType<EmailManager>();
        background = GetComponent<Image>();
        defaultColor = background.color;
    }

    void Start() {
        scoreManager = FindObjectOfType<ScoreManager>();
        sFXManager = FindObjectOfType<SFXManager>();
        this.gameObject.SetActive(false);
    }

    private void Update() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            // Move o ScrollRect para cima ou para baixo
            scrollRect.verticalNormalizedPosition += scroll * scrollSpeed;
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
        }
    }

    public void DisplayEmailByIndex(int index) {
        ProposalDatabase proposalDatabase = emailManager.proposalDatabase;
        if (index >= 0 && index < proposalDatabase.proposals.Count) {
            this.gameObject.SetActive(true);
            ProposalData proposal = proposalDatabase.proposals[index];

            companyNameText.text = proposal.companyName;
            projectTitleText.text = proposal.projectTitle;
            projectDescriptionText.text = proposal.projectDescription.Replace("\\n", "\n");

            currentProposal = proposal;
            currentEmailIndex = index;

            // Resetar a posição do ScrollRect para o topo
            if (scrollRect != null) {
                scrollRect.verticalNormalizedPosition = 1f; // 1 = topo, 0 = fundo
            }
        }

        pillarCheck.NovoEmailCarregado();
        SetButtonsVisibility(true);
    }

    public void DisplayEmailData(ProposalData proposal) {
        if (proposal == null) {
            Debug.LogWarning("Tentativa de exibir um e-mail nulo!");
            return;
        }

        this.gameObject.SetActive(true);

        companyNameText.text = proposal.companyName;
        projectTitleText.text = proposal.projectTitle;
        projectDescriptionText.text = proposal.projectDescription.Replace("\\n", "\n");

        currentProposal = proposal;
        
        if (scrollRect != null) {
            scrollRect.verticalNormalizedPosition = 1f; // 1 = topo, 0 = fundo
        }

        SetButtonsVisibility(true);
    }

    public void DisplayEmailHistory(EmailHistoryEntry entry) {
        this.gameObject.SetActive(true);
        companyNameText.text = entry.companyNameText;
        projectTitleText.text = entry.emailTitleText;
        string highlightedText = emailManager.HighlightUnethicalParts(entry.emailText, entry.unethicalParts);
        projectDescriptionText.text =
            "<b>Problema correto</b>: " + entry.correctProblem.ToString() +
            "\n<b>Problema Escolhido:</b> " + entry.chosenProblem.ToString() +
            "\n<b>Trecho escolhido:</b>" + entry.selectedText +            
            "\n<b>Resultado:</b> " + entry.result + "\n\n" +
            "<b>Corpo do e-mail:</b>\n"
            + highlightedText;
        
        if (scrollRect != null) {
            scrollRect.verticalNormalizedPosition = 1f; // 1 = topo, 0 = fundo
        }

        SetButtonsVisibility(false);
    }

    private void SetButtonsVisibility(bool isNewEmail) {
        if (acceptButton != null) {
            acceptButton.gameObject.SetActive(isNewEmail);
        }
        if (rejectButton != null) {
            rejectButton.gameObject.SetActive(isNewEmail);
        }
        if (closeButton != null) {
            closeButton.gameObject.SetActive(true); // Botão Fechar sempre visível
        }
    }

    public void AcceptEmail() {
        if (isProcessingDecision || currentProposal == null) return; // Evita cliques múltiplos

        isProcessingDecision = true; // Bloqueia cliques adicionais

        if (currentProposal.hasEthicalIssue) {
            Debug.Log("Você aceitou um e-mail problemático! Isso terá consequências...");
            scoreManager.RegistrarEmailAceito(false);
            scoreManager.RegistrarPenalizacao();
            StartCoroutine(FlashColor(Color.red));
            sFXManager.PlaySFX(2); // 2 - Errado
            emailManager.SaveEmailToHistory(Color.red, "Aceitou problemático!");
            CallResultFeedback("Errado", "Aceitou antietico");
            scoreManager.AddScore(currentProposal.nivelProblema == NivelProblema.Leve ? -15 : -30);
        } else {
            Debug.Log("Bom trabalho! Você aceitou um e-mail ético.");
            scoreManager.RegistrarEmailAceito(true);
            StartCoroutine(FlashColor(Color.green));
            sFXManager.PlaySFX(1); // 1 - Certo
            emailManager.SaveEmailToHistory(Color.green, "Aceitou ético!");
            CallResultFeedback("Correto", "Aceitou etico");
            scoreManager.AddScore(20);
        }

        StartCoroutine(CloseEmail(1.5f));
    }

    public void RejectEmail() {
        if (scoreManager.GetCurrentScore() >= 150) {
            CallResultFeedback("Tente novamente", "Usar pilares");
            return;
        }
        if (isProcessingDecision || currentProposal == null) return; // Evita cliques múltiplos

        isProcessingDecision = true; // Bloqueia cliques adicionais

        if (currentProposal.hasEthicalIssue) {
            Debug.Log("Você corretamente rejeitou um e-mail problemático!");
            scoreManager.RegistrarEmailRejeitado(true);
            StartCoroutine(FlashColor(Color.green));
            sFXManager.PlaySFX(1); // 1 - Certo
            emailManager.SaveEmailToHistory(Color.green, "Recusou ético!");
            CallResultFeedback("Correto", "Rejeitou antietico");
            scoreManager.AddScore(20);
        } else {
            Debug.Log("Você rejeitou um e-mail legítimo...");
            scoreManager.RegistrarEmailRejeitado(false);
            scoreManager.RegistrarPenalizacao();
            StartCoroutine(FlashColor(Color.red));
            sFXManager.PlaySFX(2); // 2 - Errado
            emailManager.SaveEmailToHistory(Color.red, "Recusou problemático!");
            CallResultFeedback("Errado", "Rejeitou etico");
            scoreManager.AddScore(-10);
        }
        StartCoroutine(CloseEmail(1.5f));
    }

    public void CallCloseEmail() {
        StartCoroutine(CloseEmail(0.1f));
    }

    public IEnumerator CloseEmail(float time) {
        yield return new WaitForSeconds(time);

        // Desativa o painel de feedback ao fechar o e-mail
        if (resultFeedback != null) {
            resultFeedback.SetActive(false);
            background.color = defaultColor;
        }

        // Interrompe a corrotina de feedback, se estiver em execução
        if (resultFeedbackCoroutine != null) {
            StopCoroutine(resultFeedbackCoroutine);
            resultFeedbackCoroutine = null;
        }

        emailManager.UpdateHistoryUI();
        this.gameObject.SetActive(false);
        isProcessingDecision = false; // Reativa os botões
    }

    public void CallResultFeedback(string result, string caso) {
        // Interrompe a corrotina de feedback anterior, se estiver em execução
        if (resultFeedbackCoroutine != null) {
            StopCoroutine(resultFeedbackCoroutine);
        }

        // Inicia a nova corrotina de feedback
        resultFeedbackCoroutine = StartCoroutine(ResultFeedback(result, caso));
    }

    public IEnumerator ResultFeedback(string result, string caso) {
        if (resultFeedback != null) {
            resultFeedback.SetActive(true);

            // Configura a cor e o texto do feedback
            switch (result) {
                case "Correto":
                    resultFeedback.GetComponent<Image>().color = new Color(0.2235f, 0.8588f, 0.2510f, 1f); // Verde
                    break;
                case "Quase":
                    resultFeedback.GetComponent<Image>().color = new Color(0.8588f, 0.6980f, 0.2235f, 1f);  // Amarelo          
                    break;
                case "Errado":
                    resultFeedback.GetComponent<Image>().color = new Color(0.8588f, 0.2235f, 0.2627f, 1f);  // Vermelho         
                    break;
                case "Tente novamente":
                    resultFeedback.GetComponent<Image>().color = new Color(0.349f, 0.345f, 0.337f, 1f); // Cinza
                    break;
                default:
                    Debug.Log("Resultado não encontrado.");
                    break;
            }

            switch (caso) {
                case "Acertou ambos":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Parabéns! Você acertou o trecho e o pilar!";
                    break;
                case "Acertou sem selecao":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Bom trabalho! Você acertou o pilar!";
                    break;
                case "Errou pilar sem selecao":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Quase! Há um problema ética, mas não desse pilar.";
                    break;
                case "Aceitou etico":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Bom trabalho! Você aceitou um e-mail ético.";
                    break;
                case "Rejeitou etico":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Poxa... Você rejeitou um e-mail ético!";
                    break;
                case "Aceitou antietico":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Você aceitou um e-mail problemático! Isso terá consequências...";
                    break;
                case "Rejeitou antietico":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Você corretamente rejeitou um e-mail problemático!";
                    break;
                case "Errou pilar":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Você acertou o trecho, mas não é esse pilar!";
                    break;
                case "Errou trecho":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Você acertou o pilar, mas não é esse trecho!";
                    break;
                case "Errou ambos":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Era um e-mail problemático, mas não nesse trecho e nem desse pilar!";
                    break;
                case "Selecao grande":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Seleção muito grande! Tente selecionar um trecho mais específico.";
                    break;
                case "Selecao pequena":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "Seleção muito pequena! Tente selecionar um trecho maior.";
                    break;
                case "Usar pilares":
                    resultFeedback.GetComponentInChildren<TMP_Text>().text = "À partir de agora, justifique sua recusa usando os pilares éticos!";
                    break;
                default:
                    Debug.Log("Caso não encontrado.");
                    break;
            }

            yield return new WaitForSeconds(1.5f);

            // Desativa o painel de feedback após o tempo definido
            if (resultFeedback != null) {
                resultFeedback.SetActive(false);
            }
        }
    }

    public bool IsEmailOpen() {
        return this.gameObject.activeSelf; // ou outra lógica que você usa para exibir o e-mail
    }

    public IEnumerator FlashColor(Color flashColor) {
        background.color = flashColor;
        yield return new WaitForSeconds(1.5f);
        background.color = defaultColor;
    }
}