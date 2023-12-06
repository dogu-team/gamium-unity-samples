using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HardHierarchy : MonoBehaviour
{
    public GameObject dummy;
    public Text countText;
    public uint maxDepth = 2;
    public uint maxWidth = 2;
    void Awake()
    {
        uint count = 0;
        CreateDummiesRecursive(maxDepth, maxWidth, gameObject, ref count);
        Debug.Log("Created " + count + " dummies");
        countText.text = "Created " + count + " dummies";
    }

    private void CreateDummiesRecursive(uint depth, uint width, GameObject parent, ref uint currentCount)
    {
        if (depth == 0)
        {
            return;
        }
        GameObject[] dummies = new GameObject[width];
        for (int i = 0; i < width; i++)
        {
            dummies[i] = Instantiate(dummy, parent.transform);
            currentCount++;
            CreateDummiesRecursive(depth - 1, width, dummies[i], ref currentCount);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
