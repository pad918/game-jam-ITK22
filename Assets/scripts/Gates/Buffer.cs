using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Gate
{
    public override bool get_output_click()
    {
        bool rising_edge = !last_output && inputs[0];
        last_output = inputs[0];
        return rising_edge;
    }

    public override bool get_output_down()
    {
        return inputs[0];
    }
}
