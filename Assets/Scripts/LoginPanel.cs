using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI errorTextField;
    public Button loginButton;
    public Transform postLoginPanel;
        
    private void Start()
    {
        loginButton.onClick.AddListener(Login);
    }

    private void Login()
    {
        if (Database.main.TryGetAccount(usernameInputField.text, passwordInputField.text, out AccountManager.currentAccount))
        {
            gameObject.SetActive(false);
            postLoginPanel.gameObject.SetActive(true);
            AccountManager.loggedIn = true;
        }
        else
        {
            errorTextField.gameObject.SetActive(true);
        }
    }
    
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}