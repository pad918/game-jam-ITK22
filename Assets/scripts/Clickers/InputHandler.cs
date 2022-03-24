using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HANDLE INPUT AND LOGIC GATE THINGS HERE
 * 
 ALSO SOUND!
 */

public class InputHandler : MonoBehaviour
{
    public static bool[] btns_clicked = new bool[4];
    public static bool[] btns_down    = new bool[4];
    public AudioSource audio_player;
    public static AudioSource audio_player_static;
    public AudioClip normal_hit;
    public AudioClip miss_sound;
    public static AudioClip miss_sound_static;
    public static AudioClip normal_hit_static;

    private void Start()
    {
        audio_player.clip = normal_hit;
        audio_player.loop = false;
        audio_player_static = audio_player;
        miss_sound_static = miss_sound;
        normal_hit_static = normal_hit;
    }

    // Update is called once per frame
    void Update()
    {
        //get clicked buttns
        btns_clicked[0] = Input.GetKeyDown  (KeyCode.D);
        btns_clicked[1] = Input.GetKeyDown  (KeyCode.F);
        btns_clicked[2] = Input.GetKeyDown  (KeyCode.J);
        btns_clicked[3] = Input.GetKeyDown  (KeyCode.K);

        //Get btns down:
        btns_down[0]    = Input.GetKey      (KeyCode.D);
        btns_down[1]    = Input.GetKey      (KeyCode.F);
        btns_down[2]    = Input.GetKey      (KeyCode.J);
        btns_down[3]    = Input.GetKey      (KeyCode.K);
    }

    public static void play_hit_sound()
    {
        audio_player_static.PlayOneShot(normal_hit_static, 0.8f);
    }

    public static void play_miss_sound()
    {
        audio_player_static.PlayOneShot(miss_sound_static, 1.0f);
    }

}
