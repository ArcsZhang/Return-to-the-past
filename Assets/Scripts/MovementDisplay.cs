using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDisplay : MonoBehaviour
{
    public KeyCode key;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            spriteRenderer.enabled = true;
        }
        if (Input.GetKeyUp(key))
        {
            spriteRenderer.enabled = false;
        }
    }
}
