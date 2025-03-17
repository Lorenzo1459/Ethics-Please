using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    private float delayTime = 5f; // Tempo de delay em segundos
    private float timer = 0f;

    void Update() {
        // Incrementa o timer
        timer += Time.deltaTime;

        // Verifica se o delay já passou e se qualquer tecla foi pressionada
        if (timer >= delayTime && Input.anyKeyDown) {
            // Carrega a cena "Ingame"
            SceneManager.LoadScene("Ingame");
        }
    }
}