using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject winUI;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI winWhite;
    public TextMeshProUGUI winBlack;

    public void OnRestartPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnWin(string playerName)
    {
        winText.text = playerName + " wins!";
        winUI.SetActive(true);
    }

    public void OnWinClose()
    {
        winUI.SetActive(false);
    }

    public void WinWCounter(int counter)
    {
        winWhite.text = counter.ToString();
    }

    public void WinBCounter(int counter)
    {
        winBlack.text = counter.ToString();
    }
}
