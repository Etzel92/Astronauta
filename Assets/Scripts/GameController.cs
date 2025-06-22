using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public Player1Controller player1;
    public Player2Controller player2;
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;
    public TMP_Text timerText;
    public TMP_Text levelText1;
    public TMP_Text levelText2;
    public float levelDuration = 600f;  // 10 minutos para todo el juego
    private float timeRemaining;
    private int player1Score = 0;
    private int player2Score = 0;
    public AudioClip levelUpSound;
    private AudioSource audioSource;
    private Vector2[] rocketPositions1 = new Vector2[]
    {
        new(205, -303),
        new (314, -171),
        new (0, -314),
        new (0, -194),
        new (227, 194),
        new (388, 314),
        new (138, -430),
        new (374, -447),
        new (263, -50),
        new (189, -420)
    };
    private Vector2[] rocketPositions2 = new Vector2[]
    {
        new (-205, -303),
        new (-314, -171),
        new (0, -314),
        new (0, -194),
        new (-227, 194),
        new (-388, 314),
        new (-138, -430),
        new (-374, -447),
        new (-263, -50),
        new (-189, -420)
    };
    public Vector2[] astronautPositions1 = new Vector2[]
    {
        new (-377, -229),
        new (-377, -229),
        new (-377, -229),
        new (-370, -57),
        new (-370, -57),
        new (-370, -57),
        new (-370, -57),
        new (-370, -57),
        new (-370, -57),
        new (-370, -57)
    };
    public Vector2[] astronautPositions2 = new Vector2[]
    {
        new (377, -229),
        new (377, -229),
        new (377, -229),
        new (370, -57),
        new (370, -57),
        new (370, -57),
        new (370, -57),
        new (370, -57),
        new (370, -57),
        new (370, -57)
    };
    private Vector2[] planetPositions1 = new Vector2[]
    {
        new (-377, -345),
        new (-377, -345),
        new (-377, -345),
        new (-369, -173),
        new (-369, -173),
        new (-369, -173),
        new (-369, -173),
        new (-369, -173),
        new (-369, -173),
        new (-369, -173)
    };
    private Vector2[] planetPositions2 = new Vector2[]
    {
        new (377, -345),
        new (377, -345),
        new (377, -345),
        new (369, -173),
        new (369, -173),
        new (369, -173),
        new (369, -173),
        new (369, -173),
        new (369, -173),
        new (369, -173)
    };
    public GameObject rocket1;
    public GameObject rocket2;
    public GameObject astronaut1;
    public GameObject astronaut2;
    public GameObject planet1;
    public GameObject planet2;

    void Start()
    {
        timeRemaining = levelDuration;
        UpdateScoreText();
        MoveObjectsForPlayer(1, player1.currentLevel); // Mueve los objetos del jugador 1 a la posición inicial
        MoveObjectsForPlayer(2, player2.currentLevel); // Mueve los objetos del jugador 2 a la posición inicial
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        // Convertir tiempo restante a formato mm:ss
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;

        if (timeRemaining <= 0 || player1.currentLevel > 10 || player2.currentLevel > 10)
        {
            EndGame();
        }
    }

    public bool IsMouseOnLeftSide()
    {
        return Input.mousePosition.x < Screen.width / 2;
    }

    public bool IsInputOnLeftSide(Vector2 position)
    {
        return position.x < Screen.width / 2;
    }

    public Vector2[] GetTouchPositions()
    {
        List<Vector2> touchPositions = new List<Vector2>();

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                touchPositions.Add(touch.position);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            touchPositions.Add(Input.mousePosition);
        }

        return touchPositions.ToArray();
    }

    public void PlayerScored(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Score += 100;
            player1.currentLevel++;
            levelText1.text = "Nivel: " + player1.currentLevel;  // Actualiza el texto del nivel
            MoveObjectsForPlayer(1, player1.currentLevel); // Mueve los objetos del jugador 1 al siguiente nivel
            PlayLevelUpSound();
        }
        else if (playerNumber == 2)
        {
            player2Score += 100;
            player2.currentLevel++;
            levelText2.text = "Nivel: " + player2.currentLevel;  // Actualiza el texto del nivel
            MoveObjectsForPlayer(2, player2.currentLevel); // Mueve los objetos del jugador 2 al siguiente nivel
            PlayLevelUpSound();
        }

        UpdateScoreText();
    }

private void PlayLevelUpSound()
{
    if (levelUpSound != null && audioSource != null)
    {
        audioSource.PlayOneShot(levelUpSound);
    }
}
    void UpdateScoreText()
    {
        player1ScoreText.text = "Pts: " + player1Score;
        player2ScoreText.text = "Pts: " + player2Score;
    }

    public void EndGame()
    {
        // Guardar los puntajes en PlayerPrefs
        PlayerPrefs.SetInt("Player1Score", player1Score);
        PlayerPrefs.SetInt("Player2Score", player2Score);

        // Cambiar a la escena de resultados
        SceneManager.LoadScene("Resultado");
    }

    void MoveObjectsForPlayer(int playerNumber, int level)
    {
        if (level <= 10)
        {
            if (playerNumber == 1)
            {
                Vector2 newRocketPosition = rocketPositions1[level - 1];
                rocket1.transform.localPosition = new Vector3(newRocketPosition.x, newRocketPosition.y, rocket1.transform.localPosition.z);

                Vector2 newAstronautPosition = astronautPositions1[level - 1];
                astronaut1.transform.localPosition = new Vector3(newAstronautPosition.x, newAstronautPosition.y, astronaut1.transform.localPosition.z);

                Vector2 newPlanetPosition = planetPositions1[level - 1];
                planet1.transform.localPosition = new Vector3(newPlanetPosition.x, newPlanetPosition.y, planet1.transform.localPosition.z);
            }
            else if (playerNumber == 2)
            {
                Vector2 newRocketPosition = rocketPositions2[level - 1];
                rocket2.transform.localPosition = new Vector3(newRocketPosition.x, newRocketPosition.y, rocket2.transform.localPosition.z);

                Vector2 newAstronautPosition = astronautPositions2[level - 1];
                astronaut2.transform.localPosition = new Vector3(newAstronautPosition.x, newAstronautPosition.y, astronaut2.transform.localPosition.z);

                Vector2 newPlanetPosition = planetPositions2[level - 1];
                planet2.transform.localPosition = new Vector3(newPlanetPosition.x, newPlanetPosition.y, planet2.transform.localPosition.z);
            }
        }
    }
}
