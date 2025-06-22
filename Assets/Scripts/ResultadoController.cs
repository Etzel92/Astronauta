using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultadoController : MonoBehaviour
{
    public TMP_Text resultText;
    private int player1Score;
    private int player2Score;

    void Start()
    {
        // Obtener los puntajes de los jugadores desde el GameController
        player1Score = PlayerPrefs.GetInt("Player1Score", 0);
        player2Score = PlayerPrefs.GetInt("Player2Score", 0);

        // Mostrar los resultados
        ShowResults();
    }

    void ShowResults()
    {
        if (player1Score > player2Score)
        {
            resultText.text = "¡Jugador 1!\nPts: " + player1Score;
        }
        else if (player2Score > player1Score)
        {
            resultText.text = "¡Jugador 2!\nPts: " + player2Score;
        }
        else
        {
            resultText.text = "¡Empate!\nPts: " + player1Score;
        }
    }
}
