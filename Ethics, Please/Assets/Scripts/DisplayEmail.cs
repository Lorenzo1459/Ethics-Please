using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEmail : MonoBehaviour {
    private EmailManager emailManager;
    private ScoreManager scoreManager;

    public TMP_Text companyNameText;
    public TMP_Text projectTitleText;
    public TMP_Text projectDescriptionText;

    private Image background;
    private Color defaultColor;

    private int currentEmailIndex = -1;
    public ProposalData currentProposal;

    void Awake() {
        emailManager = FindObjectOfType<EmailManager>();

        background = GetComponent<Image>();
        defaultColor = background.color;
    }

    void Start() {        
        scoreManager = FindObjectOfType<ScoreManager>();
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
        }
    }

    public void AcceptEmail() {
        if (currentProposal == null) return;

        if (currentProposal.hasEthicalIssue) {
            Debug.Log("Você aceitou um e-mail problemático! Isso terá consequências...");
            StartCoroutine(FlashColor(false));
            scoreManager.AddScore(currentProposal.nivelProblema == NivelProblema.Leve ? -15 : -30);
        } else {
            Debug.Log("Bom trabalho! Você aceitou um e-mail ético.");
            StartCoroutine(FlashColor(true));
            scoreManager.AddScore(20);
        }

        StartCoroutine(CloseAndRequestNext());
    }

    public void RejectEmail() {
        if (currentProposal == null) return;

        if (currentProposal.hasEthicalIssue) {
            Debug.Log("Você corretamente rejeitou um e-mail problemático!");
            StartCoroutine(FlashColor(true));
            scoreManager.AddScore(10);
        } else {
            Debug.Log("Você rejeitou um e-mail legítimo...");
            StartCoroutine(FlashColor(false));
            scoreManager.AddScore(-10);
        }

        StartCoroutine(CloseAndRequestNext());
    }

    private IEnumerator CloseAndRequestNext() {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
        emailManager.RequestNextEmail();
    }

    public IEnumerator FlashColor(bool isCorrect, string optional = "Default") {
        if (optional == "Default") {
            background.color = isCorrect ? Color.green : Color.red;
            yield return new WaitForSeconds(1.5f); // Espera 1.5s
            background.color = defaultColor; // Retorna à cor original
        } else if (optional == "Yellow") {
            background.color = Color.yellow;
            yield return new WaitForSeconds(1.5f); // Espera 1.5s
            background.color = defaultColor; // Retorna à cor original
        }
    }
}
