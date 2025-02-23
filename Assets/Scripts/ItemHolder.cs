using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public enum Items
    {
        None,
        Coin,
        Pistol
    }
    public static int totalItems = Enum.GetNames(typeof(Items)).Length;
    public Sprite[] itemSprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        setSprite(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSprite(int index)
    {
        spriteRenderer.sprite = itemSprites[index];
    }
}
