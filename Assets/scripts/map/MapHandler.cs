using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{

    public static long offset = 0;

    public GameObject note_prefab;

    public Vector3 target_pos;

    public float scroll_speed = 20.0f;

    MapLoader mapLoader;

    public AudioSource source;

    //List<Note> notes;
    List<Pair<Note, GameObject>> notes;
    long timer = 0;
    // Start is called before the first frame update
    IEnumerator Start() {
        notes = new List<Pair<Note, GameObject>>();
        mapLoader = new MapLoader();
        List<Note> tmp_notes = mapLoader.load_map(@"C:/Users/Måns/game jam ITK22/Assets/maps/test1/UNDEAD CORPORATION - The Empress scream off ver (TheZiemniax) [HD].osu");
        // Create gameobjects for the all notes:
        for(int i = 0; i < tmp_notes.Count; i++)
        {
            //Instantiate gameObject later
            notes.Add(new Pair<Note, GameObject>(tmp_notes[i], null));
        }

        //Load song from file:
        source = GetComponent<AudioSource>();
        using (var www = new WWW(@"C:/Users/Måns/game jam ITK22/Assets/maps/test1/The Empress.mp3"))
        {
            yield return www;
            source.clip = www.GetAudioClip();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!source.isPlaying && source.clip.isReadyToPlay)
        {
            source.Play();
        }
        timer = get_playback_time();
        // Instantiate gameobjects
        create_new_objects();

        // Update notes
        update_all_notes();

        //timer += (long)(1000 * Time.deltaTime);
        if(notes.Count > 0 && timer >= notes[0].First.click_time)
        {
            Debug.Log(timer);
            //notes.RemoveAt(0);
        }
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

                //Update position
                float x = (notes[i].First.column - Note.TOTAL_COLUMNS / 2);
                notes[i].Second.transform.position = target_pos + new Vector3(x, (float)(scroll_speed * delta), -1);

                //Remove if too old
                if(delta < 0)
                {
                    //Debug.Log("REMOVING!");
                    Destroy(notes[i].Second);
                    notes.RemoveAt(i);
                    i--;
                }
            }
            else
            {
                break;
            }
        }
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
