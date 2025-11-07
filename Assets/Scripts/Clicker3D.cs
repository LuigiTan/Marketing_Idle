using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Clicker3D : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public GameObject clickableCube;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI autoclickerUpgradeText;
    public TextMeshProUGUI autoclickerTimerText;
    public Image autoclicker;
    public Image autoclickerTimer;
    public GameObject winPanel;

    [Header("Gameplay Settings")]
    public int score = 0;
    private int upgradeCost = 15;
    private int autoClickerInst = 0;
    private float timer = 0f;
    private float timerUpgrade = 5f;
    private int timerCost = 20;

    // Input system
    private PlayerInputs playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void Update()
    {
        HandleMouseClick();
        HandleUpgrades();
        HandleAutoClick();
        UpdateUI();
        HandleWinCondition();
    }

    private void HandleMouseClick()
    {
        if (playerInputs.Player.Click.triggered)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == clickableCube)
                {
                    score++;
                }
            }
        }
    }

    private void HandleUpgrades()
    {
        // AutoClicker upgrade
        if (score >= upgradeCost)
        {
            autoclicker.color = Color.white;
            if (playerInputs.Player.Upgrade.triggered)
            {
                score -= upgradeCost;
                upgradeCost += upgradeCost / 2;
                autoClickerInst++;
            }
        }
        else
        {
            autoclicker.color = Color.gray;
        }

        // Timer upgrade
        if (score >= timerCost)
        {
            autoclickerTimer.color = Color.white;
            if (playerInputs.Player.Timer.triggered)
            {
                score -= timerCost;
                timerCost += timerCost / 2;
                timerUpgrade = Mathf.Max(0.5f, timerUpgrade - 0.5f);
            }
        }
        else
        {
            autoclickerTimer.color = Color.gray;
        }
    }

    private void HandleAutoClick()
    {
        if (autoClickerInst > 0)
        {
            timer += Time.deltaTime;
            if (timer >= timerUpgrade)
            {
                score += autoClickerInst;
                timer = 0f;
            }
        }
    }

    private void UpdateUI()
    {
        scoreText.text = "Bread - " + score;
        autoclickerUpgradeText.text = "AutoClick \n" + upgradeCost;
        autoclickerTimerText.text = "Reduce Time \n" + timerCost;
    }

    private void HandleWinCondition()
    {
        if (score >= 420)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;

            if (playerInputs.Player.Restart.triggered) Restart();
            if (playerInputs.Player.Quit.triggered) Quit();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
