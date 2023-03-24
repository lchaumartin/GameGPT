using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public string restartMessageText = "Press R to restart";
    public Font restartMessageFont;
    public int restartMessageFontSize = 24;
    public Color restartMessageColor = Color.white;

    private Text restartMessage;
    private bool canRestart = false;

    private void OnEnable()
    {
        // Create the restart message text object
        GameObject restartMessageObject = new GameObject("RestartMessage");
        restartMessageObject.transform.SetParent(transform);
        restartMessageObject.transform.localPosition = Vector3.zero;
        restartMessage = restartMessageObject.AddComponent<Text>();
        restartMessage.text = restartMessageText;
        restartMessage.font = restartMessageFont;
        restartMessage.fontSize = restartMessageFontSize;
        restartMessage.color = restartMessageColor;
        restartMessage.alignment = TextAnchor.MiddleCenter;
        canRestart = true;
    }

    private void OnDisable()
    {
        // Destroy the restart message text object
        Destroy(restartMessage.gameObject);

        // Set the canRestart flag to false
        canRestart = false;
    }

    private void Update()
    {
        if (canRestart && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
