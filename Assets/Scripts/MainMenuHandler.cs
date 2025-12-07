using UnityEngine;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject playerNameCanvas;
    public PlayerData playerData;
    public TMP_InputField playerNameInput;

    public void SetPlayerName()
    {
        if (string.IsNullOrEmpty(playerNameInput.text)) return;
        playerData.PlayerName = playerNameInput.text;
        playerNameCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
}
