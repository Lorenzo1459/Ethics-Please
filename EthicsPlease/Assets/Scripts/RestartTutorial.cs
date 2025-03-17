using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartTutorial : MonoBehaviour {
    public Image fillImage; // Refer�ncia � imagem de carregamento
    public float tempoParaReiniciar = 2f; // Tempo necess�rio segurando "R"
    private float tempoSegurando = 0f;
    public GameObject tuto;

    void Start() {
        if (fillImage != null) {
            fillImage.fillAmount = 1f; // Come�a cheia
        }
    }

    void Update() {
        // Verifica se fillImage ainda existe
        if (fillImage != null) {
            if (Input.GetKey(KeyCode.R)) {
                tempoSegurando += Time.deltaTime;
                fillImage.fillAmount = 1f - (tempoSegurando / tempoParaReiniciar); // Diminui gradualmente

                if (tempoSegurando >= tempoParaReiniciar) {
                    //tuto.SetActive(true);
                    tuto.GetComponent<Dialogue>().RestartTutorial();
                }
            } else {
                tempoSegurando = 0f;
                fillImage.fillAmount = 1f; // Reseta para cheia se soltar "R"
            }
        }
    }
}
