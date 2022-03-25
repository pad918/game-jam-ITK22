using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutWithTimer : MonoBehaviour
{
    SpriteRenderer sr;
    public float time_to_death = 10;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(1 + Random.Range(2, 3), Random.Range(-1, 1), -1);
        transform.eulerAngles = new Vector3(0, Random.Range(-15, 15), 0);
    }

    // Update is called once per frame
    void Update()
    {
        time_to_death -= Time.deltaTime;

        var tmp = transform.localScale;

        tmp.x = time_to_death / 10.0f;
        tmp.y = time_to_death / 10.0f;

        transform.localScale = tmp;

        if (time_to_death < 0)
            Destroy(gameObject);
    }
}
