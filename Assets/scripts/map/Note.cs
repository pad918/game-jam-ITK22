using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Borde kanske vara en abstrakt klass? Flera olika typer av noter?
public class Note
{
    public static int TOTAL_COLUMNS = 4;
    public long click_time { get; private set; }
    public int column { get; private set; }

    public int effects { get; private set; }

    Color color;
    public Note(long click_time, int column, int effects, Color color)
    {
        this.click_time = click_time;
        this.column = column;
        this.color = color;
        this.effects = effects;
    }
}
