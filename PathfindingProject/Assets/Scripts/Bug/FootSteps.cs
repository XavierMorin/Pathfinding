using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    // Start is called before the first frame update
    static bool hasSprite1 = false;
    public Sprite sprite1;
    public Sprite sprite2;
    private SpriteRenderer sr;
    public float timer = 5;
    private float maxTime;
    void Start()
    {
        maxTime = timer;
        sr = GetComponent<SpriteRenderer>();
        if (hasSprite1)
            sr.sprite = sprite1;
        else
            sr.sprite = sprite2;
        hasSprite1 = !hasSprite1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
            Destroy(this.gameObject);
        Color color = sr.color;
        color.a = timer/maxTime;
        sr.color = color;      
    }
}
