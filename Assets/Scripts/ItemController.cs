using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Vector2 resepawnPosition = Vector2.zero;
    public SpriteRenderer spriteRenderer;
    public ItemHolder.Items item = ItemHolder.Items.None;
    public bool active = true;
    private ItemHolder itemHolder;
    private CircleCollider2D circleCollider;
    private void Awake()
    {
        resepawnPosition = new Vector2(transform.position.x, transform.position.y);
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        itemHolder = FindObjectOfType<ItemHolder>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        transform.position = new Vector3(resepawnPosition.x, resepawnPosition.y, 0.5f);
        active = true;
        spriteRenderer.sprite = itemHolder.itemSprites[(int)item];
        circleCollider.enabled = true;
    }

    public void pickUp()
    {
        active = false;
        spriteRenderer.sprite = itemHolder.itemSprites[0];
        circleCollider.enabled = false;
    }
}
