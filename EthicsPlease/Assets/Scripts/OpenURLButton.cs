using UnityEngine;
using UnityEngine.UI; // Necessário para interagir com a UI do Unity

public class OpenURLButton : MonoBehaviour {
    public Button yourButton;  // Referência ao botão
    public string url = "https://forms.gle/PnVVtxRn78htchNs9";  // Link do formulário ou página

    void Start() {
        // Verifica se o botão foi atribuído
        if (yourButton != null) {
            // Adiciona a função ao evento de clique do botão
            yourButton.onClick.AddListener(OpenURL);
        }
    }

    // Método que será chamado ao clicar no botão
    void OpenURL() {
        // Abre o link na web
        Application.OpenURL(url);
    }
}