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
    public TextMeshProUGUI pointWhite;
    public TextMeshProUGUI pointBlack;

    public void OnRestartPress()
    {
        //Debug.Log(GameManager.instance.counterW);
        //int counterW = GameManager.instance.counterW;
        //int counterB = GameManager.instance.counterB;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameManager.instance.counterW = counterW;
        //GameManager.instance.counterB = counterB;
        //WinWCounter(counterW);
        //WinBCounter(counterB);
        //Debug.Log(GameManager.instance.counterW);
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

    public void WinWPoint(int counter)
    {
        pointWhite.text = counter.ToString();
    }

    public void WinBPoint(int counter)
    {
        pointBlack.text = counter.ToString();
    }
}
