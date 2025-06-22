using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player1Controller : MonoBehaviour
{
    public float maxJumpForce = 100000f;
    public float chargeRate = 5000f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector3 startPosition;
    public int currentLevel = 1;
    private float currentChargeTime = 0f;
    private bool hasScored = false;
    public RectTransform playerPanel;
    public GameController gameController;
    public Image chargeBar;  // Referencia a la barra de carga
    public Image levelUpImage1;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        Vector2[] touchPositions = gameController.GetTouchPositions();
        bool isCharging = false;

        foreach (Vector2 touchPosition in touchPositions)
        {
            if (gameController.IsInputOnLeftSide(touchPosition))
            {
                isCharging = true;
                currentChargeTime += Time.deltaTime * chargeRate;
                break;
            }
        }

        if (!isCharging && currentChargeTime > 0 && isGrounded)
        {
            float jumpForce = Mathf.Min(currentChargeTime, maxJumpForce);
            rb.AddForce(new Vector2(jumpForce, jumpForce), ForceMode2D.Impulse);
            currentChargeTime = 0f;
        }

        // Actualizar la barra de carga
        if (chargeBar != null)
        {
            chargeBar.fillAmount = currentChargeTime / maxJumpForce;
        }

        if (transform.position.y < -10)
        {
            Respawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.collider.CompareTag("Rocket1") && !hasScored)
        {
            hasScored = true;
            AdvanceLevel();
        }
        else if (collision.collider.CompareTag("Time"))
        {
            Respawn();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void AdvanceLevel()
    {
        if (currentLevel < 10)  // Cambia esto de 11 a 10
        {
            StartCoroutine(ShowlevelUpImage1()); // Inicia la corrutina para mostrar la imagen
            FindObjectOfType<GameController>().PlayerScored(1);
            Respawn();
            hasScored = false;
        }
        else
        {
            // El jugador ha completado todos los niveles
            FindObjectOfType<GameController>().EndGame();
        }
    }

    IEnumerator ShowlevelUpImage1()
    {
        levelUpImage1.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        levelUpImage1.gameObject.SetActive(false);
    }

    void Respawn()
    {
        if (currentLevel <= 10)  // Añade esta comprobación
        {
            Vector2 respawnPosition = FindObjectOfType<GameController>().astronautPositions1[currentLevel - 1];  // o astronautPositions2 para Player2Controller
            transform.localPosition = new Vector3(respawnPosition.x, respawnPosition.y, startPosition.z);
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Si por alguna razón currentLevel es mayor que 10, usa la última posición conocida
            Vector2 respawnPosition = FindObjectOfType<GameController>().astronautPositions1[9];  // o astronautPositions2 para Player2Controller
            transform.localPosition = new Vector3(respawnPosition.x, respawnPosition.y, startPosition.z);
            rb.velocity = Vector2.zero;
        }
    }
}
