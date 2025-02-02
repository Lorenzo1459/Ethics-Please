using UnityEngine;

public enum ProposalProblem {
    Falibilidade,
    Opacidade,
    Vies,
    Discriminacao,
    Autonomia,
    Privacidade,
    Responsabilidade,
    Nenhum
}


[CreateAssetMenu(fileName = "NewProposal", menuName = "Emails/NewProposal")]
public class ProposalData : ScriptableObject {
    public string companyName;
    public string projectTitle;
    public string projectDescription;
    public bool hasEthicalIssue; // True if the proposal has an ethical issue
    public ProposalProblem problema;
}
