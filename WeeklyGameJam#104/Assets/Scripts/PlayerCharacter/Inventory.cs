
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private string[] tools;
    private string[] parts;

    public KeyCode interact = KeyCode.E;

    void Start()
    {
        tools = new string[0];
        parts = new string[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(interact))
            return;
    }

    // Store item in inverntory in the appropriate
    // "bins" (tools or parts). Delete the game
    // object afterwards.
    public void PickUpItem(ref GameObject item)
    {
        if (item.tag.Equals("Tool"))
        {
            tools = ExpandStrArray(tools);
            tools[tools.Length] = item.name;
        }
        else if (item.tag.Equals("Part"))
        {
            parts = ExpandStrArray(parts);
            parts[parts.Length] = item.name;
        }

        Destroy(item);
    }

    // Destroy/unlist item from inventory.
    public void ConsumeItem(string itemName, string tag)
    {
        if (tag.Equals("Tool"))
        {
            RemoveItemFromArray(tools, itemName);
        }
        else
            RemoveItemFromArray(parts, itemName);
    }

    // Remove item from array.
    string[] RemoveItemFromArray(string[] array, string itemName)
    {
        int index = FindItemInArray(array, itemName);

        int len = array.Length;

        string[] updateArray = new string[len-1];

        if (index > -1)
        {
            array[index] = "#";
        }

        for (int i = 0; i < len; i++)
        {
            if (!array[i].Equals("#"))
                updateArray[i] = array[i];
        }

        return updateArray;
    }

    // Finds and returns an item's index in
    // an array. Otherwise it returns a -1
    // for "nothing found."
    int FindItemInArray(string[] array, string itemName)
    {
        int len = array.Length;

        for (int i = 0; i < len; i++)
        {
            if (array[i].Equals(itemName))
                return i;
        }

        return -1;
    }

    // Expand the array by 1.
    string[] ExpandStrArray(string[] array)
    {
        int len = array.Length;

        string[] newArray = new string[len+1];

        for (int i=0; i<len; i++)
        {
            newArray[i] = array[i];
        }

        return newArray;
    }

    // Decrease the array by 1.
    string[] DecreaseStrArray(string[] array)
    {
        int len = array.Length;

        if (len > 0)
        {
            string[] newArray = new string[len-1];

            for (int i = 0; i < len-1; i++)
            {
                newArray[i] = array[i];
            }

            return newArray;
        }

        return array;
    }

}
