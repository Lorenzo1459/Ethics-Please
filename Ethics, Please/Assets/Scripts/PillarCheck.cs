using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PillarCheck : MonoBehaviour
{
    public DisplayEmail displayEmail;
    private ProposalData emailData;
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
    public void FalibilidadeCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Falibilidade ) {
            Debug.Log("Esse e-mail realmente tem problema �tico Falibilidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Falibilidade) {
            Debug.Log("Esse e-mail realmente tem problema �tico mas n�o � Falibilidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect,"Yellow"));
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }
        

        displayEmail.NextEmail();
    }

    public void OpacidadeCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Opacidade) {
            Debug.Log("Esse e-mail realmente tem problema �tico Opacidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Opacidade) {
            Debug.Log("Esse e-mail realmente tem problema �tico mas n�o � Opacidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void ViesCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Vies) {
            Debug.Log("Esse e-mail realmente tem problema �tico Vies!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Vies) {
            Debug.Log("Esse e-mail realmente tem problema �tico mas n�o � Vies");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void DiscriminacaoCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Discriminacao) {
            Debug.Log("Esse e-mail realmente tem problema �tico Discriminacao!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Discriminacao) {
            Debug.Log("Esse e-mail realmente tem problema �tico mas n�o � Discriminacao");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void AutonomiaCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Autonomia) {
            Debug.Log("Esse e-mail realmente tem problema �tico Autonomia!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Autonomia) {
            Debug.Log("Esse e-mail realmente tem problema �tico mas n�o � Autonomia");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void PrivacidadeCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Privacidade) {
            Debug.Log("Esse e-mail realmente tem problema �tico Privacidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Privacidade) {
            Debug.Log("Esse e-mail realmente tem problema �tico mas n�o � Privacidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void ResponsabilidadeCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Responsabilidade) {
            Debug.Log("Esse e-mail realmente tem problema �tico Responsabilidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Responsabilidade) {
            Debug.Log("Esse e-mail realmente tem problema �tico mas n�o � Responsabilidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }
}
