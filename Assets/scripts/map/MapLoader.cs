using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader
{
	Dictionary<string, List<string>> file_entrys;

	public MapLoader()
	{
		file_entrys = new Dictionary<string, List<string>>();
	}

	public List<Note> load_map(string file_path)
	{
		List<Note> notes;
		file_entrys.Clear();
		string[] lines = System.IO.File.ReadAllLines(file_path);
		load_all_entrys(lines);
		notes = load_all_notes();

		//Set map properties

		//Set offset
		List<string> entries;
		bool found = file_entrys.TryGetValue("[General]", out entries);
		if (found) {

			var tmp = parse_sub_properties(entries);
			string prop;
			found = tmp.TryGetValue("Offset", out prop);
			if (found)
			{
				try
				{
					MapHandler.offset = int.Parse(prop);
					Debug.Log("Setting offset: " + MapHandler.offset + "ms");
				}
				catch (System.Exception e)
				{
					Debug.Log("Parsing failed: " + e.Message);
				}
			}
		}
		return notes;
	}

	private void load_all_entrys(string[] lines)
	{
		
		string current_heading="[HEADING_LESS]";
		file_entrys[current_heading] = new List<string>();

		foreach (string line in lines)
		{
			if (line.StartsWith("["))
			{
				current_heading = line;
				file_entrys[current_heading] = new List<string>();
				continue;
			}

			//Test if line is empty

			if (line.Length < 1)
			{
				continue;
			}

			List<string> entries_in_current_key;
			bool found = file_entrys.TryGetValue(current_heading, out entries_in_current_key);
			if (found)
			{
				entries_in_current_key.Add(line);
			}
			else
			{
				Debug.Log("Failed loading line: " + line + " under heading: " + current_heading);
			}
			
		}
	}

	private List<Note> load_all_notes()
	{
		List<Note> notes = new List<Note>();
		List<string> entries;
		bool found = file_entrys.TryGetValue("[HitObjects]", out entries);
		if (found)
		{
			//Parse every note from file:
			foreach (string line in entries) {
				Note n = parse_single_note(line);
				if (n != null)
					notes.Add(n);
			}
		}
		return notes;
	}

	private Dictionary<string, string> parse_sub_properties(List<string> strs)
    {
		Dictionary<string, string> dic = new Dictionary<string, string>();
		foreach(string s in strs)
        {
			string[] parts = s.Split(':');
			if(parts.Length != 2)
            {
				continue;
            }
            else
            {
				dic[parts[0]] = parts[1].Trim();
            }
        }
		return dic;
    }

	//Parse one single note from the file.
	private Note parse_single_note(string line)
	{
		Note n = new Note(0, 0, Color.white);
		//Split string
		string[] args = line.Split(',');
        if (args.Length < 3)
        {
			throw new System.Exception("Failed parsing =( ");
        }
        else
        {
			//Get click time:
			int col = Mathf.FloorToInt(int.Parse(args[0]) * Note.TOTAL_COLUMNS / 512);
			n = new Note(long.Parse(args[2]), col, Color.white);
        }
		return n;
	}
}
