using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/* Uppdatera även UI från den här klassen, vill inte förstöra MapHandler mer ... */
public class ScoreHandler : MonoBehaviour
{
    public static long score = 0;
    public static int combo = 0;

    public GameObject score_text_obj;
    public GameObject combo_text_obj;

    private TMPro.TMP_Text score_text;
    private TMPro.TMP_Text combo_text;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        combo = 1;
        score_text = score_text_obj.GetComponent<TMPro.TMP_Text>();
        combo_text = combo_text_obj.GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        score_text.text = score.ToString();
        combo_text.text = "x" + combo.ToString();
    }

    public static void add_score(int base_score)
    {
        score += (long)base_score * (long)combo;
    }
    public static void add_combo()
    {
        combo++;
    }

    public static void clear_combo()
    {
        //Play combo break sound effect if combo was larger than five when broken
        if(combo > 5)
        {
            InputHandler.play_miss_sound();
        }
        combo = 1;
    }
}
