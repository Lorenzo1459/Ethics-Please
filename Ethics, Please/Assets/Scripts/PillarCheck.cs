using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PillarCheck : MonoBehaviour {
    public DisplayEmail displayEmail;
    public EmailManager emailManager;
    private ProposalData emailData;
    private ScoreManager scoreManager;

    private void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();
        emailManager = FindObjectOfType<EmailManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Start is called before the first frame update    
    void Update() {
        emailData = displayEmail.currentProposal;
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                GameObject clickedObject = hit.collider.gameObject;
                emailData = clickedObject.GetComponent<ProposalData>();

                if (emailData != null) {
                    Debug.Log("Component found: " + emailData);
                } else {
                    Debug.Log("Component not found.");
                }
            }
        }
    }

    public void CheckEthicalIssue(TipoProblema tipoProblema) {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == tipoProblema) {
            Debug.Log("Esse e-mail realmente tem problema ético " + tipoProblema + "!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
            scoreManager.AddScore(30);
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != tipoProblema) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é " + tipoProblema);
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
            scoreManager.AddScore(15);
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
            scoreManager.AddScore(-5);
        }

        emailManager.ShowNextEmail();
    }

    public void FalibilidadeCheck() {
        CheckEthicalIssue(TipoProblema.Falibilidade);
    }

    public void OpacidadeCheck() {
        CheckEthicalIssue(TipoProblema.Opacidade);
    }

    public void ViesCheck() {
        CheckEthicalIssue(TipoProblema.Vies);
    }

    public void DiscriminacaoCheck() {
        CheckEthicalIssue(TipoProblema.Discriminacao);
    }

    public void AutonomiaCheck() {
        CheckEthicalIssue(TipoProblema.Autonomia);
    }

    public void PrivacidadeCheck() {
        CheckEthicalIssue(TipoProblema.Privacidade);
    }

    public void ResponsabilidadeCheck() {
        CheckEthicalIssue(TipoProblema.Responsabilidade);
    }
}
