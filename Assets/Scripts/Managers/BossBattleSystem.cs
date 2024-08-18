using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossBattleSystem : MonoBehaviour
{
    private string[] options = { "Rock", "Paper", "Scissors" };

    private int playerRounds = 0;
    private int bossRounds = 0;
    private const int roundsNeeded = 3;

    [Header("UI")]
    public TextMeshProUGUI textResult;
    public TextMeshProUGUI textScore;
    public Animator animatorBoss, animatorPlayer;  
    public GameObject victoryPanel; 
    public GameObject defeatPanel;

    [Header("Sprites")]
    public Image imagePlayerChoice, imageBossChoice;
    private Sprite defaultPlayerSprite;
    private Sprite defaultBossSprite;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    [Header("Sounds")]
    private AudioSource audioSource;
    public AudioClip rockSound;
    public AudioClip paperSound;
    public AudioClip scissorsSound;
    public AudioClip bossSound;

    public void OnRockClick()
    {
        audioSource.PlayOneShot(rockSound);
        PlayRound("Rock");
    }

    public void OnPaperClick()
    {
        audioSource.PlayOneShot(paperSound);
        PlayRound("Paper");
    }

    public void OnScissorsClick()
    {
        audioSource.PlayOneShot(scissorsSound);
        PlayRound("Scissors");
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        defaultPlayerSprite = imagePlayerChoice.sprite;
        defaultBossSprite = imageBossChoice.sprite;

        textResult.text = "Welcome to the final battle!";
        UpdateScore();
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    void UpdateScore()
    {
        textScore.text = $"Score - Player: {playerRounds}, Boss: {bossRounds}";
    }

    void PlayRound(string playerChoice)
    {
        string bossChoice = BossChoices();
        textResult.text = $"Boss choices: {bossChoice}\n";

        imagePlayerChoice.sprite = GetSpriteForChoice(playerChoice);
        imageBossChoice.sprite = GetSpriteForChoice(bossChoice);

        string result = StateWinner(playerChoice, bossChoice);
        if (result == "Draw")
        {
            textResult.text += "It's a draw!";
        }
        else if (result == "Player")
        {
            textResult.text += "You won this round!";
            playerRounds++;
        }
        else
        {
            textResult.text += "Boss won this round!";
            bossRounds++;
        }

        UpdateScore();

        if (playerRounds == roundsNeeded || bossRounds == roundsNeeded)
        {
            if (playerRounds == roundsNeeded)
            {
                textResult.text += "\nCongrats! You defeated the boss!.";
                audioSource.PlayOneShot(bossSound);
                animatorBoss.SetTrigger("Death");  
                ShowPanelVictory();
            }
            else
            {
                textResult.text += "\nYou have been defeated. Try again!";
                audioSource.PlayOneShot(bossSound);
                animatorPlayer.SetTrigger("hit");
                ShowDefeatPanel();
            }
        }
    }

    void ResetGame()
    {
        imagePlayerChoice.gameObject.SetActive(true);
        imageBossChoice.gameObject.SetActive(true);
        playerRounds = 0;
        bossRounds = 0;
        textResult.text = "Welcome to the final battle!";
        UpdateScore();
        defeatPanel.SetActive(false);

        imagePlayerChoice.sprite = defaultPlayerSprite;
        imageBossChoice.sprite = defaultBossSprite;

        imagePlayerChoice.gameObject.SetActive(true);
        imageBossChoice.gameObject.SetActive(true);
    }

    void ShowPanelVictory()
    {
        imagePlayerChoice.gameObject.SetActive(false);
        imageBossChoice.gameObject.SetActive(false);

        victoryPanel.SetActive(true);
        Invoke("GoToMainMenu", 3f);  
    }

    void ShowDefeatPanel()
    {
        imagePlayerChoice.gameObject.SetActive(false);
        imageBossChoice.gameObject.SetActive(false);

        defeatPanel.SetActive(true);
        Invoke("ResetGame", 3f);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    string StateWinner(string player, string boss)
    {
        if (player == boss)
        {
            return "Draw";
        }
        else if ((player == "Rock" && boss == "Scissors") || (player == "Paper" && boss == "Rock") || (player == "Scissors" && boss == "Paper"))
        {
            return "Player";
        }
        else
        {
            return "Boss";
        }
    }

    string BossChoices()
    {
        // Elige al azar
        return options[Random.Range(0, options.Length)];
    }

    Sprite GetSpriteForChoice(string choice)
    {
        if (choice == "Rock")
        {
            return rockSprite; 
        }
        else if (choice == "Paper")
        {
            return paperSprite; 
        }
        else if (choice == "Scissors")
        {
            return scissorsSprite; 
        }

        return null;
    }
}
