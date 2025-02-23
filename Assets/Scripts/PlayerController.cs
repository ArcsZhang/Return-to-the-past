using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;

    public int playerIndex;
    public Vector2 resepawnPosition = Vector2.zero;
    public Sprite[] sprites;
    public Sprite[] deadSpirtes;
    public TimeManager timeManager;
    public ItemHolder itemHolder;
    public bool dead;
    public List<ControlRecord> recording = new List<ControlRecord>();

    // items

    public GameObject[] playerItemsArray;
    public ItemHolder.Items CurrentItem = ItemHolder.Items.None;

    void setItem(ItemHolder.Items item)
    {
        switch (item)
        {
            case ItemHolder.Items.None:
                for (int i = 0; i < ItemHolder.totalItems; i++)
                {
                    playerItemsArray[i] = itemHolder.transform.GetChild(i).gameObject;
                    playerItemsArray[i].SetActive(false);
                }
                goto case ItemHolder.Items.Pistol;
            case ItemHolder.Items.Pistol:
                playerItemsArray[0].SetActive(true);
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeManager = FindObjectOfType<TimeManager>();
        itemHolder = FindObjectOfType<ItemHolder>();
        resepawnPosition = new Vector2(transform.position.x, transform.position.y);
    }
    void Start()
    {
        //totalItems = Enum.GetNames(typeof(Items)).Length;
        //playerItemsArray = new GameObject[totalItems];
        //for(int i = 0; i < totalItems; i++)
        //{
        //    playerItemsArray[i] = itemHolder.transform.GetChild(i).gameObject;
        //    playerItemsArray[i].SetActive(false);
        //}
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
        rb.velocity = Vector2.zero;
        if (!dead && timeManager.currentRound >= playerIndex)
        {
            if (timeManager.frameIndex < recording.Count)
            {
                rb.velocity = new Vector2(recording[timeManager.frameIndex].move.x * moveSpeed, recording[timeManager.frameIndex].move.y * moveSpeed);
                if (recording[timeManager.frameIndex].interact.magnitude != 0)
                {
                    interact(recording[timeManager.frameIndex].interact);
                }
            }
        }
    }

    public void Respawn()
    {
        transform.position = new Vector3(resepawnPosition.x, resepawnPosition.y, 0);
        CurrentItem = ItemHolder.Items.None;
        if (timeManager.currentRound < playerIndex)
        {
            dead = true;
            recording = new List<ControlRecord>();
            spriteRenderer.sprite = deadSpirtes[playerIndex - 1];
            gameObject.layer = 7;
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;

        }
        else if (timeManager.currentRound == playerIndex)
        {
            dead = false;
            recording = new List<ControlRecord>();
            spriteRenderer.sprite = sprites[playerIndex - 1];
            itemHolder.setSprite(0);
            gameObject.layer = 7 + playerIndex;
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
        }
        else
        {
            dead = false;
            spriteRenderer.sprite = sprites[playerIndex - 1];
            gameObject.layer = 7 + playerIndex;
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
        }
    }

    public void Die()
    {
        dead = true;
        spriteRenderer.sprite = deadSpirtes[playerIndex - 1];
        gameObject.layer = 7;
    }

    public void Disappear()
    {
        if (!dead)
        {
            Die();
        }
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    public void interact(Vector2 direction)
    {
        if (CurrentItem == ItemHolder.Items.Pistol)
        {
            RaycastHit2D ray;
            switch (playerIndex)
            {
                case 1:
                    ray = Physics2D.Raycast(transform.position, direction, 100f, LayerMask.GetMask("Wall", "2", "3", "4", "5", "6"));
                    break;
                case 2:
                    ray = Physics2D.Raycast(transform.position, direction, 100f, LayerMask.GetMask("Wall", "1", "3", "4", "5", "6"));
                    break;
                case 3:
                    ray = Physics2D.Raycast(transform.position, direction, 100f, LayerMask.GetMask("Wall", "1", "2", "4", "5", "6"));
                    break;
                case 4:
                    ray = Physics2D.Raycast(transform.position, direction, 100f, LayerMask.GetMask("Wall", "1", "2", "3", "5", "6"));
                    break;
                case 5:
                    ray = Physics2D.Raycast(transform.position, direction, 100f, LayerMask.GetMask("Wall", "1", "2", "3", "4", "6"));
                    break;
                case 6:
                    ray = Physics2D.Raycast(transform.position, direction, 100f, LayerMask.GetMask("Wall", "1", "2", "3", "4", "5"));
                    break;
                default:
                    ray = Physics2D.Raycast(transform.position, direction, 100f, LayerMask.GetMask("Wall", "1", "2", "3", "4", "5", "6"));
                    break;
            }    
            
            if (ray.collider != null)
            {
                if (ray.transform.gameObject.tag == "Player")
                {
                    PlayerController target = ray.transform.gameObject.GetComponent<PlayerController>();
                    Debug.Log(ray.transform.gameObject);
                    if (target.dead == false)
                    {
                        target.Die();
                    }
                    else
                    {
                        target.Disappear();
                    }
                }
                else if (ray.transform.gameObject.tag == "Box")
                {
                    Debug.Log(ray.transform.gameObject);

                    BoxController target = ray.transform.gameObject.GetComponent<BoxController>();
                    target.Disappear();
                }
            }

            CurrentItem = ItemHolder.Items.None;
            itemHolder.setSprite(0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item" && !timeManager.loose && !dead && CurrentItem == ItemHolder.Items.None)
        {
            ItemController targetItem = collision.GetComponent<ItemController>();
            switch (targetItem.item)
            {
                case ItemHolder.Items.None:
                    break;
                case ItemHolder.Items.Coin:
                    if (timeManager.currentRound == playerIndex)
                    {
                        itemHolder.setSprite(1);
                        timeManager.initNext();
                    }
                    else
                    {
                        timeManager.playSound(1);
                        timeManager.loose = true;
                    }
                    CurrentItem = ItemHolder.Items.Coin;
                    targetItem.pickUp();
                    break;
                case ItemHolder.Items.Pistol:
                    if (timeManager.currentRound == playerIndex)
                    {
                        itemHolder.setSprite(2);
                    }
                    CurrentItem = ItemHolder.Items.Pistol;
                    targetItem.pickUp();
                    break;
                default:
                    break;
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
