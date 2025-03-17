using System.Collections.Generic;
using UnityEngine;

public enum TipoProblema {
    Falibilidade,
    Opacidade,
    Vies,
    Discriminacao,
    Autonomia,
    Privacidade,
    Responsabilidade,
    Nenhum
}

public enum NivelProblema {
    Nenhum,
    Leve,
    Moderado,
    Grave
}


[CreateAssetMenu(fileName = "NewProposal", menuName = "Emails/NewProposal")]
public class ProposalData : ScriptableObject {
    public string companyName;
    public string projectTitle;
    [TextArea(10,40)]
    public string projectDescription;
    public bool hasEthicalIssue; // True if the proposal has an ethical issue
    public TipoProblema tipoProblema;
    public NivelProblema nivelProblema;
    public List<string> trechoAntietico;
}
