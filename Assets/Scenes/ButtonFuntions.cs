using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFuntions : MonoBehaviour
{
    public void exit_game()
    {
        Application.Quit();
    }

    public void go_to_scene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void load_main_game(string file_path)
    {
        string[] tmp = file_path.Split(';');
        // Set arguments
        MapHandler.game_file_path = tmp[0];
        MapHandler.music_path = tmp[1];
        
        // Load main game 
        SceneManager.LoadScene(1);
    }
}
