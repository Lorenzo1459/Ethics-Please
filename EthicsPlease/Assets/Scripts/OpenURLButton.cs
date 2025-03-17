using UnityEngine;
using UnityEngine.UI; // Necess�rio para interagir com a UI do Unity

public class OpenURLButton : MonoBehaviour {
    public Button yourButton;  // Refer�ncia ao bot�o
    public string url = "https://forms.gle/PnVVtxRn78htchNs9";  // Link do formul�rio ou p�gina

    void Start() {
        // Verifica se o bot�o foi atribu�do
        if (yourButton != null) {
            // Adiciona a fun��o ao evento de clique do bot�o
            yourButton.onClick.AddListener(OpenURL);
        }
    }

    // M�todo que ser� chamado ao clicar no bot�o
    void OpenURL() {
        // Abre o link na web
        Application.OpenURL(url);
    }
}