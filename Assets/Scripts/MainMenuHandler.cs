using UnityEngine;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject playerNameCanvas;
    public PlayerData playerData;
    public TMP_InputField playerNameInput;

    public void Start()
    {
        if (playerData.PlayerName != "Player")
        {
            playerNameCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
        }
    }

    public void SetPlayerName()
    {
        if (string.IsNullOrEmpty(playerNameInput.text)) return;
        playerData.PlayerName = playerNameInput.text;
        playerNameCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void OnSelect()
    {
        playerNameInput.text = "";
    }

    public void OnDeselect()
    {
        if (string.IsNullOrEmpty(playerNameInput.text)) playerNameInput.text = "Player";
    }
}
