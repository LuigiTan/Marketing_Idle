using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Clicker : MonoBehaviour
{
    private PlayerInputs playerInputs;

    private bool canClick;

    public int score = 0;
    private int upgradeCost = 15;
    private int autoClickerInst = 0;
    private float timer = 0f;
    private float timerUpgrade = 5;
    private int timerCost = 20;


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI autoclickerUpgradeText;
    public TextMeshProUGUI autoclickerTimerText;
    public Image autoclicker;
    public Image autoclickerTimer;
    public GameObject winPanel;

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Player.Enable();
        winPanel.SetActive(false);
    }

    void Update()
    {
        if (!canClick)
        {
            canClick = playerInputs.Player.Click.triggered;
        }

        if (playerInputs.Player.Click.triggered)
        {
            score++;
        }

        if (score < upgradeCost)
        {
            autoclicker.color = Color.gray;
        }
        else if (score >= upgradeCost)
        {
            autoclicker.color = Color.white;
            if (playerInputs.Player.Upgrade.triggered)
            {
                score -= upgradeCost;
                upgradeCost = upgradeCost + (upgradeCost/2);
                autoClickerInst++;
            }
        }

        if (score < timerCost)
        {
            autoclickerTimer.color = Color.gray;
        }
        else if (score >= timerCost)
        {
            autoclickerTimer.color = Color.white;
            if (playerInputs.Player.Timer.triggered)
            {
                score -= timerCost;
                timerCost = timerCost + (timerCost/2);
                timerUpgrade -= 0.5f;
            }
        }

        if (autoClickerInst > 0)
        {
            timer += Time.deltaTime;
            if (timer >= timerUpgrade)
            {
                score += autoClickerInst;
                timer = 0;
            }
        }

        scoreText.text = "Bread - " + score;
        autoclickerUpgradeText.text = "AutoClick \n" + upgradeCost;
        autoclickerTimerText.text = "Reduce Time \n" + timerCost;
        Debug.Log(timerUpgrade);

        if (score >= 420)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0;

            if (playerInputs.Player.Restart.triggered) Restart();
            if (playerInputs.Player.Quit.triggered) Quit();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
