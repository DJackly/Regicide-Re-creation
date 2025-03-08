using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureGIF : MonoBehaviour
{
    private readonly int MAX = 4;
    private int index = 0;
    public List<Sprite> PicList = new List<Sprite>();
    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private readonly float GapTime = 0.5f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = PicList[index];
    }
    private void IndexPlus()
    {
        if (index == MAX - 1) //×î´óÖµ
        {
            index = 0;
        }
        else index++;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if( timer >= GapTime )
        {
            timer = 0f;
            IndexPlus();
            spriteRenderer.sprite = PicList[index];
        }
    }
}
