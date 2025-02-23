using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class BoxController : MonoBehaviour
{
    public Vector2 resepawnPosition = Vector2.zero;
    public SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;



    // Start is called before the first frame update
    void Awake()
    {
        gameObject.tag = "Box";
        gameObject.layer = 7;
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        resepawnPosition = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.zero;
    }

    public void Respawn()
    {
        transform.position = new Vector3(resepawnPosition.x, resepawnPosition.y, 0);
        gameObject.layer = 7;
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    public void Disappear()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}
