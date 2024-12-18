using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public AccountDeletionManager accountDeletionManager;

    public void OnDeleteAccountButtonClick()
    {
        accountDeletionManager.ShowConfirmationPanel();
    }
}
