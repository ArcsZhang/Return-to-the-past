using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;

    public int playerIndex;
    public Vector2 resepawnPosition = Vector2.zero;
    public Sprite[] sprites;
    public Sprite[] deadSpirtes;
    public TimeManager timeManager;
    public bool dead;
    public List<ControlRecord> recording = new List<ControlRecord>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeManager = FindObjectOfType<TimeManager>();
        resepawnPosition = new Vector2(transform.position.x, transform.position.y);
    }
    void Start()
    {

    }

    void Update()
    {
        ProcessInputs();
        Action();
    }

    void ProcessInputs()
    {
        if (timeManager.currentRound == playerIndex)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;
            Vector3 interactDirection = Vector3.zero;
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                interactDirection = (mouseWorldPos - transform.position).normalized;
            }
            recording.Add(new ControlRecord(moveDirection, new Vector2(interactDirection.x, interactDirection.y)));
        }
    }

    void Action()
    {
        if (!dead && timeManager.currentRound >= playerIndex)
        {
            if (timeManager.frameIndex < recording.Count)
            {
                rb.velocity = new Vector2(recording[timeManager.frameIndex].move.x * moveSpeed, recording[timeManager.frameIndex].move.y * moveSpeed);
            }
            else if (timeManager.frameIndex == recording.Count)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    public void Respawn()
    {
        transform.position = new Vector3(resepawnPosition.x, resepawnPosition.y, 0);
        if (timeManager.currentRound < playerIndex)
        {
            dead = true;
            recording = new List<ControlRecord>();
            spriteRenderer.sprite = deadSpirtes[playerIndex - 1];
        }
        else if (timeManager.currentRound == playerIndex)
        {
            dead = false;
            recording = new List<ControlRecord>();
            spriteRenderer.sprite = sprites[playerIndex - 1];
        }
        else
        {
            dead = false;
            spriteRenderer.sprite = sprites[playerIndex - 1];
        }
    }

    public void Die()
    {
        dead = true;
        spriteRenderer.sprite = deadSpirtes[playerIndex - 1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Coin" && !timeManager.loose && !dead)
        {
            if (playerIndex == timeManager.currentRound)
            {
                timeManager.initNext();
            }
            else
            {
                timeManager.playSound(1);
                timeManager.loose = true;
            }
        }
    }

    public class ControlRecord
    {
        public Vector2 move;
        public Vector2 interact;

        public ControlRecord(Vector2 move, Vector2 interact)
        {
            this.move = new Vector2(move.x, move.y);
            this.interact = new Vector2(interact.x, interact.y);
        }
    }
}