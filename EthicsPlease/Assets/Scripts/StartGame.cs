using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    private float delayTime = 2f; // Tempo de delay em segundos
    private float timer = 0f;
    public GameObject tutorial;
    private EmailManager emailManager;
    public bool isClickable = false;
    public GameObject StartingText;

    private void Awake() {
        tutorial.SetActive(false);        
        emailManager = FindObjectOfType<EmailManager>();
    }

    void Update() {
        // Incrementa o timer
        timer += Time.deltaTime;
        // Verifica se o delay já passou e se qualquer tecla foi pressionada
        if (timer >= delayTime && Input.anyKeyDown && isClickable == true) {            
            tutorial.SetActive(true);
            emailManager.CallDelayedEmail(3f);
            this.gameObject.SetActive(false);
        }

        if (StartingText.activeSelf == false) {            
            isClickable = true;
        }
    }
}