using UnityEngine;
using UnityEngine.SceneManagement; // If you want to redirect to the login screen

public class LogoutButtonHandler : MonoBehaviour
{
    public void OnLogoutButtonClick()
    {
        UserManager.Instance.Logout(
            onLogoutSuccess: () =>
            {
                Debug.Log("Logout completed. Redirecting to login screen...");
                SceneManager.LoadScene("LoginScene"); // Redirect to login scene
            },
            onLogoutFailure: (error) =>
            {
                Debug.LogError($"Logout failed: {error}");
                // Show an error message to the user if needed
            });
    }
}
