using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public int difficulty = 0;

    public Vector3 output_position;

    public List<Vector3> input_positions;

    public bool[] inputs;

    protected bool last_output = false;

    public void set_inputs(bool[] inputs) {
        if(this.inputs.Length > inputs.Length) {
            throw new System.Exception("SIZE NOTE SAME ...---...");
        }

        for(int i = 0; i < this.inputs.Length; i++) {
            this.inputs[i] = inputs[i];
        }
    }
    public virtual bool get_output_click() { return false; }
    public virtual bool get_output_down() { return false; }
}
