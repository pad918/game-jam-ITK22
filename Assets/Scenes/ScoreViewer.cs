using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreViewer : MonoBehaviour
{

    public TMPro.TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = ScoreHandler.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
