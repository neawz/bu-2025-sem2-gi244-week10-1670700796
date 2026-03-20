using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;

    private float normalSpeed = 10f;
    private float dashSpeed = 20f;
    private float leftBound = -15;

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Exam 03 - Dash
        if (playerController.isDashing)
        {
            speed = dashSpeed;
        }
        else if (!playerController.isDashing)
        {
            speed = normalSpeed;
        }

        if (!playerController.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
