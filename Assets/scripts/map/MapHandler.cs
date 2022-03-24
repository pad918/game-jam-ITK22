using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapHandler : MonoBehaviour
{

    public static long offset = 0;

    public GameObject note_prefab;

    public Vector3 target_pos;

    public float scroll_speed = 20.0f;

    private bool is_playing = false;

    private bool has_loaded = false;

    private bool GOD_MODE = true;

    MapLoader mapLoader;

    public AudioSource source;

    //List<Note> notes;
    List<Pair<Note, GameObject>> notes;
    long timer = 0;

    // Start is called before the first frame update
    IEnumerator Start() {
        notes = new List<Pair<Note, GameObject>>();
        mapLoader = new MapLoader();
        string maps_folder_path = Application.dataPath + "/maps/";
        List<Note> tmp_notes = mapLoader.load_map(maps_folder_path + "test1/svar1.osu");
        
        // Create gameobjects for the all notes:
        for(int i = 0; i < tmp_notes.Count; i++)
        {
            //Instantiate gameObject later
            notes.Add(new Pair<Note, GameObject>(tmp_notes[i], null));
        }

        //Load song from file:
        source = GetComponent<AudioSource>();
        using (var www = new WWW(maps_folder_path + "test1/The Empress.mp3"))
        {
            yield return www;
            source.clip = www.GetAudioClip();
        }
        has_loaded = true;
    }

    //Handler input here:
    private void LateUpdate()
    {
        for(int i = 0; i < Note.TOTAL_COLUMNS; i++)
        {
            

            //Debug.Log("FÖRSÖKER TA KLICKA PÅ RAD " + i);

            //Find next note in coloumn:
            int note_id = get_id_of_next_note_in_column(i);

            //Get deltatime
            long note_time = notes[note_id].First.click_time;
            long delta_time = (note_time - timer);

            //God mode auto click at perfect time:
            if (GOD_MODE && delta_time < 0)
            {
                InputHandler.btns_clicked[i] = true;
            }

            if (!InputHandler.btns_clicked[i])
                continue;

            if (note_id >= 0)
            {
                
                delta_time = delta_time < 0 ? -delta_time : delta_time;
               
                // BORDE VARA NY FUNKTION!

                /* DLETA TIME RULES:
                 * > 300ms ==> Can't click note
                 * <=300ms && > 150ms,  Miss note
                 * <=150 && >100,       Bad
                 * <= 100 %% > 50       Ok
                 * <= 50 && > 25        Good
                 * <=25,                Perfect
                 * 
                 */
                
                //Can't click more than 300 ms off
                if (delta_time > 300)
                {
                    //Can't click
                }
                else if(delta_time > 150)
                {
                    //Miss
                    ScoreHandler.clear_combo();
                    HitText.display(0);
                }
                else if(delta_time > 100)
                {
                    //Bad
                    ScoreHandler.add_score(50);
                    ScoreHandler.add_combo();
                    HitText.display(1);
                }
                else if(delta_time > 50)
                {
                    //Ok
                    ScoreHandler.add_score(100);
                    ScoreHandler.add_combo();
                    HitText.display(2);
                }
                else if(delta_time > 25)
                {
                    //Good
                    ScoreHandler.add_score(200);
                    ScoreHandler.add_combo();
                    HitText.display(3);
                }
                else
                {
                    //Perfect
                    ScoreHandler.add_score(500);
                    ScoreHandler.add_combo();
                    HitText.display(4);
                }

                bool remove_note = (delta_time < 300);
                if (remove_note)
                {
                    remove_note_at(note_id);
                    //Play click sound
                    InputHandler.play_hit_sound();
                }

                Debug.Log("Clicked with delta: " + delta_time + "ms");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!has_loaded)
            return;

        if(!is_playing && !source.isPlaying && source.clip.isReadyToPlay)
        {
            source.loop = false;
            source.Play();
            is_playing = true;
        }
        if(is_playing && !source.isPlaying)
        {
            Debug.Log("FINNISHED SONG!");
        }
        timer = get_playback_time();
        // Instantiate gameobjects
        create_new_objects();

        // Update notes
        update_all_notes();

        //timer += (long)(1000 * Time.deltaTime);
        if(notes.Count > 0 && timer >= notes[0].First.click_time)
        {
            //Debug.Log(timer);
            //notes.RemoveAt(0);
        }
    }

    private int get_id_of_next_note_in_column(int col)
    {
        int i = 0;
        foreach(var n in notes)
        {
            if(n.First.column == col)
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    private long get_playback_time()
    {
        
        return ((10 * source.timeSamples) / 441) + offset;
    }
    private void update_all_notes()
    {
        //Update all notes that have less than 1 second to live
        for (int i = 0; i < notes.Count; i++)
        {
            // Instantiate when the note has less than one second to live
            if (notes[i].First.click_time < timer + 5000 && notes[i].Second != null)
            {
                double delta = 0.0002 * (notes[i].First.click_time - timer);
                long deltaMs = notes[i].First.click_time - timer;
                //Update position
                float x = (notes[i].First.column - Note.TOTAL_COLUMNS / 2);
                notes[i].Second.transform.position = target_pos + new Vector3(x+0.5f, (float)(scroll_speed * delta), -1);

                //Remove if too old notes
                if(deltaMs < -150)
                {
                    //Debug.Log(" KILLING TOO OLD NOTE!!!");
                    remove_note_at(i);
                    ScoreHandler.clear_combo();
                    i--;
                }
            }
            else
            {
                break;
            }
        }
    }

    private void remove_note_at(int i)
    {
        Destroy(notes[i].Second);
        notes.RemoveAt(i);
    }

    private void create_new_objects()
    {
        for(int i = 0; i < notes.Count; i++)
        {
            // Instantiate when the note has less than one second to live
            if(notes[i].First.click_time < timer + 5000 && notes[i].Second == null)
            {
                GameObject obj = Instantiate(note_prefab, transform.position + new Vector3(10000,10000,100), transform.rotation);
                notes[i].Second = obj;
            }
            else if(notes[i].First.click_time >= timer + 5000)
            {
                break;
            }
        }
    }
}
