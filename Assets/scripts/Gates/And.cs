using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class And : Gate
{
    public override bool get_output_click()
    {
        bool output = get_output_down();
        bool rising_edge = !last_output && (output);
        last_output = output;
        return rising_edge;
    }

    public override bool get_output_down()
    {
        return inputs[0] && inputs[1];
    }
}
