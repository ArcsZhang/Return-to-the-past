using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int roundCount = 1;
    public int currentRound = 1;
    public int frameIndex;
    public PlayerController[] playerList;
    public ItemController[] itemList;
    public BoxController[] boxList;
    public Sprite[] numbers;
    public float nextCountDown = -1;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioPlay;
    public AudioClip[] audios;
    public bool loose;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioPlay = GetComponent<AudioSource>();
        playerList = FindObjectsOfType<PlayerController>();
        itemList = FindObjectsOfType<ItemController>();
        boxList = FindObjectsOfType<BoxController>();
        restart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        frameIndex++;
        countDown();
        if (Input.GetKeyDown(KeyCode.R))
        {
            respawn();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            restart();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            initNext();
        }
    }

    void respawn()
    {
        frameIndex = 0;
        loose = false;
        foreach (PlayerController player in playerList)
        {
            player.Respawn();
        }
        foreach (ItemController item in itemList)
        {
            item.Respawn();
        }
        foreach (BoxController box in boxList)
        {
            box.Respawn();
        }
    }

    void restart()
    {
        currentRound = 1;
        spriteRenderer.sprite = numbers[currentRound - 1];
        respawn();
    }

    void next()
    {
        if (currentRound < roundCount)
        {
            currentRound++;
            spriteRenderer.sprite = numbers[currentRound - 1];
            respawn();
        }
        else if (currentRound == 7)
        {

        }
        else
        {
            currentRound = 7;
            spriteRenderer.sprite = numbers[6];
            respawn();
        }
    }

    public void initNext()
    {
        nextCountDown = 1;
        playSound(0);
    }

    public void playSound(int x)
    {
        audioPlay.clip = audios[x];
        audioPlay.Play();
    }

    void countDown()
    {
        if (nextCountDown > 0)
        {
            nextCountDown -= Time.deltaTime;
        }
        else if (nextCountDown != -1)
        {
            next();
            nextCountDown = -1;
        }
    }
}
