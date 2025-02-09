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
            Debug.Log("Esse e-mail realmente tem problema ético Falibilidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Falibilidade) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é Falibilidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect,"Yellow"));
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }
        

        displayEmail.NextEmail();
    }

    public void OpacidadeCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Opacidade) {
            Debug.Log("Esse e-mail realmente tem problema ético Opacidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Opacidade) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é Opacidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void ViesCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Vies) {
            Debug.Log("Esse e-mail realmente tem problema ético Vies!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Vies) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é Vies");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void DiscriminacaoCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Discriminacao) {
            Debug.Log("Esse e-mail realmente tem problema ético Discriminacao!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Discriminacao) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é Discriminacao");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void AutonomiaCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Autonomia) {
            Debug.Log("Esse e-mail realmente tem problema ético Autonomia!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Autonomia) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é Autonomia");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void PrivacidadeCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Privacidade) {
            Debug.Log("Esse e-mail realmente tem problema ético Privacidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Privacidade) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é Privacidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }

    public void ResponsabilidadeCheck() {
        if (emailData.hasEthicalIssue && emailData.tipoProblema == TipoProblema.Responsabilidade) {
            Debug.Log("Esse e-mail realmente tem problema ético Responsabilidade!!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        } else if (emailData.hasEthicalIssue && emailData.tipoProblema != TipoProblema.Responsabilidade) {
            Debug.Log("Esse e-mail realmente tem problema ético mas não é Responsabilidade");
            bool isCorrect = !emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect, "Yellow"));
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            bool isCorrect = emailData.hasEthicalIssue;
            StartCoroutine(displayEmail.FlashColor(isCorrect));
        }

        displayEmail.NextEmail();
    }
}
