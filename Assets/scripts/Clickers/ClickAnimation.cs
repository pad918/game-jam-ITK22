using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAnimation : MonoBehaviour
{
    public bool is_gate_output = false;
    public int keyCode;
    public GameObject child;
    private SpriteRenderer sprite;
    float fade_timer = 0.0f;
    float time_since_fade_started = 0.0f;
    bool is_fading = false;
    
    // Start is called before the first frame update
    void Start()
    {
        is_fading = false;
        sprite = child.GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        //if (Input.GetKeyDown(keyCode))
        //{
        //    click();
        //}
        if (!is_gate_output && InputHandler.btns_down[keyCode])
        {
            click();
        }
        else if (is_gate_output && GateHandler.gate_outputs_down[keyCode])
        {
            click();
        }

        Color col = sprite.color;
        col.a = fade_timer;
        sprite.color = col;
        if (is_fading)
        {
            time_since_fade_started += Time.deltaTime;
            update_fade_timer();
        }

    }

    public void click()
    {
        is_fading = true;
        fade_timer = 1.0f;
        time_since_fade_started = 0;
        //Debug.Log("Fading!!!");
    }
    private void update_fade_timer()
    {
        if (time_since_fade_started < 1/30f)
            fade_timer = 1.0f;
        else
            fade_timer = 1 / (time_since_fade_started*30);
        if (time_since_fade_started > 1.0f)
        {
            is_fading = false;
            fade_timer = 0;
            time_since_fade_started = 0;
        }
    }
}
