using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Menu Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsMenu;

    private void Start()
    {
        ShowMainMenu();
    }

    // 🎮 PLAY BUTTON
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
        Debug.Log("Changing Scene");
    }

    // ❌ QUIT BUTTON
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Quit Game (won't close in editor)");
#endif
    }

    // 📜 CREDITS BUTTON
    public void ShowCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    // 🔙 BACK FROM CREDITS
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
}