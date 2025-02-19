using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEmail : MonoBehaviour {
    private EmailManager emailManager;
    private ScoreManager scoreManager;

    public TMP_Text companyNameText;
    public TMP_Text projectTitleText;
    public TMP_InputField projectDescriptionText;
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f;

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
        }
    }

    public void AcceptEmail() {
        if (currentProposal == null) return;

        if (currentProposal.hasEthicalIssue) {
            Debug.Log("Você aceitou um e-mail problemático! Isso terá consequências...");
            StartCoroutine(FlashColor(Color.red));
            scoreManager.AddScore(currentProposal.nivelProblema == NivelProblema.Leve ? -15 : -30);
        } else {
            Debug.Log("Bom trabalho! Você aceitou um e-mail ético.");
            StartCoroutine(FlashColor(Color.green));
            scoreManager.AddScore(20);
        }

        StartCoroutine(CloseEmail());
    }

    public void RejectEmail() {
        if (currentProposal == null) return;

        if (currentProposal.hasEthicalIssue) {
            Debug.Log("Você corretamente rejeitou um e-mail problemático!");
            StartCoroutine(FlashColor(Color.green));
            scoreManager.AddScore(10);
        } else {
            Debug.Log("Você rejeitou um e-mail legítimo...");
            StartCoroutine(FlashColor(Color.red));
            scoreManager.AddScore(-10);
        }

        StartCoroutine(CloseEmail());
    }

    private IEnumerator CloseEmail() {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }

    public IEnumerator FlashColor(Color flashColor) {
        background.color = flashColor;
        yield return new WaitForSeconds(1.5f);
        background.color = defaultColor;
    }
}
