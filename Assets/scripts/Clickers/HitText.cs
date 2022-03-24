using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitText : MonoBehaviour
{
    public static List<Sprite> sprites_static;
    public  List<Sprite> sprites;
    private static float time_to_live;
    private static SpriteRenderer rend;
    private static bool is_fading = false;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        sprites_static = sprites;
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_fading)
            return;

        time_to_live -= Time.deltaTime;
        Color tmp = rend.color;
        tmp.a = time_to_live / 0.4f;
        if (time_to_live < 0)
        {
            tmp.a = 0;
            is_fading = false;
        }
        rend.color = tmp;
    }

    public static void display(int id)
    {
        if(id >= sprites_static.Count || id < 0)
        {
            throw new System.Exception("SPRITE NOT IN LIST!!!");
        }

        rend.sprite = sprites_static[id];
        time_to_live = 0.4f;
        is_fading = true;
    }

}
