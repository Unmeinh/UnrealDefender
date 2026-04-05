using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonRestart : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TriggerPressing);
        animator = gameObject.GetComponent<Animator>();
    }

    void Pressing()
    {
        RestartScene();
    }

    void TriggerPressing()
    {
        animator.Play("Pressing");
    }

    
    void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
