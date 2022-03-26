using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateHandler : MonoBehaviour
{
	public enum gate_type { BUF, NOT, AND, OR }

	public static List<gate_type> future_gates = new List<gate_type>();

	int total_diff = 0;
	public static bool[] gate_outputs_down = new bool[4];
	public static bool[] gate_outputs_clicked = new bool[4];
	public List<Vector3> key_positions;
	public GameObject buf, not, and, or;
	private static GameObject buf_static, not_static, and_static, or_static;
	private static Gate[] gates;
	bool[] inputs = new bool[2];
	void Start()
	{
		buf_static	= buf;
		not_static	= not;
		and_static	= and;
		or_static	= or;

		if (future_gates.Count == 0)
        {
			future_gates.Add(gate_type.BUF);
			future_gates.Add(gate_type.BUF);
			future_gates.Add(gate_type.BUF);
			future_gates.Add(gate_type.BUF);
		}
		gates = new Gate[4];

		//Instantiate 4 gates:
		for(int i = 0; i < 4; i++)
        {
			load_next_gate();
		}
	}

	void LateUpdate() 
	{
		if (gates==null || gates[0] == null)
			return;

		total_diff = 0;
		for(int i = 0; i < 4; i++)
        {
			total_diff += gates[i].difficulty;
        }

		for (int i = 0; i < 4; i++)
        {
			if (key_positions.Count != 4)
				return;

			//GIZMOS DRAW:
			var t = gates[i].output_line.GetComponent<LineRenderer>();
			t.SetPosition(0, gates[i].transform.position + gates[i].output_position + new Vector3(0, 0, 10));
			t.SetPosition(1, gates[i].transform.position + gates[i].output_position + new Vector3(0, 1, 0) + new Vector3(0, 0, 10));
			//Debug.DrawLine(gates[i].transform.position + gates[i].output_position, gates[i].transform.position + gates[i].output_position + new Vector3(0, 1, 0), Color.red);

			for (int j = 0; j < gates[i].input_positions.Count; j++)
			{
				try
				{
					if ((j + i) <= 3)
					{
						//Draw input lines:
						var temp = gates[i].input_lines[j].GetComponent<LineRenderer>();
						temp.SetPosition(0, gates[i].transform.position + gates[i].input_positions[j] + new Vector3(0, 0, 10));
						temp.SetPosition(1, key_positions[i + j] + new Vector3(0, 0, 10));
						//Debug.DrawLine(gates[i].transform.position + gates[i].input_positions[j], key_positions[i + j], Color.red);
					}
					else
					{
						var temp = gates[i].input_lines[j].GetComponent<LineRenderer>();
						temp.SetPosition(0, gates[i].transform.position + gates[i].input_positions[j] + new Vector3(0, 0, 10));
						temp.SetPosition(1, key_positions[3] + new Vector3(0, 0, 10));
						//Debug.DrawLine(gates[i].transform.position + gates[i].input_positions[j], key_positions[3], Color.red);
					}
					//Debug.Log("i = " + i + " j = " + j + " | i+j= " + (i + j) + " | key size = " + key_positions.Count);
				}
				catch(System.Exception e)
                {
					Debug.Log("ERRRRRRROR:::: i = " + i + " j = " + j + " | i+j= " + (i + j) + " | key size = " + key_positions.Count);
				}
				
			}
			

			//Get click:
			inputs[0] = InputHandler.btns_down[i];
			if (i + 1 < 4)
				inputs[1] = InputHandler.btns_down[i + 1];
			else
				inputs[1] = InputHandler.btns_down[3];
			gates[i].set_inputs(inputs);
			gate_outputs_clicked[i] = gates[i].get_output_click();

			//Get held down:
			gate_outputs_down[i] = gates[i].get_output_down();
		
		}
	}

	public static void load_next4_gates()
    {
		for (int i = 0; i < 4; i++)
			load_next_gate();
    }
	private static void load_next_gate()
    {
        if (future_gates.Count == 0)
        {
			throw new System.Exception("NOT MORE GATES IN GATE LIST!!!, ALL GATES CONSUMED");
        }

		//Remove leftmost gate
		if(gates[0] != null)
			Destroy(gates[0].transform.gameObject);

		//Reorder gates
		for(int i = 1; i < 4; i++)
        {
			gates[i - 1] = gates[i];
        }

		//Instantiate new gate
		GameObject go = null;
        switch (future_gates[0])
        {
			case gate_type.BUF:
				go = Instantiate(buf_static);
				break;
			case gate_type.NOT:
				go = Instantiate(not_static);
				break;
			case gate_type.AND:
				go = Instantiate(and_static);
				break;
			case gate_type.OR:
				go = Instantiate(or_static);
				break;
        }

		if(go != null)
        {
			gates[3] = go.GetComponent<Gate>();
        }
        else
        {
			throw new System.Exception("Gate is null exception");
        }

		//Set gates to the new correct position:
		for(int i = 0; i < 4; i++)
        {
			if(gates[i] != null)
            {
				gates[i].transform.position = new Vector3(i -2.0f + 0.5f, -4, -1);
            }
        }

		future_gates.RemoveAt(0);
    }
}
