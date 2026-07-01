using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (move > 0)
        {
            GameEventMonoBehaviour.LogEvent("Pergerakan", "Player");
            sr.flipX = false;
        }
        else if (move < 0)
        {
            sr.flipX = true;
        }
    }
}