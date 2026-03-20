using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int hp;
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;

    private Rigidbody rb;
    private InputAction jumpAction;
    private bool isOnGround = true;
    private int jumpCount = 0;
    private bool haveDoubleJump = true;

    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");

        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Exam 02 - Double Jump
        if (jumpCount >= 2)
        {
            haveDoubleJump = false;
            jumpCount = 0; return;
        }

        if (jumpAction.triggered && !gameOver && haveDoubleJump)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            isOnGround = false;
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);
            jumpCount++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            haveDoubleJump = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Exam 04 - PlayerHP
            //explosionParticle.Play();
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(crashSfx);
            hp--;

            if (hp <= 0)
            {
                Debug.Log("Game Over!");
                gameOver = true;
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                dirtParticle.Stop();
            }
        }
    }

}