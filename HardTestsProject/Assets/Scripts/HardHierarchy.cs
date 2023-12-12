using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HardHierarchy : MonoBehaviour
{
    public GameObject dummy;
    public Text countText;
    public InputField depthInput;
    public InputField widthInput;

    readonly uint maxDepth = 6;
    readonly uint maxWidth = 7;



    void Awake()
    {
        depthInput.text = 6.ToString();
        widthInput.text = 6.ToString();
    }

    public void OnRecreate()
    {
        // Destroy all children recursively
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        uint count = 0;
        uint depth = uint.TryParse(depthInput.text, out depth) ? depth : 0;
        uint width = uint.TryParse(widthInput.text, out width) ? width : 0;
        if (depth > maxDepth)
        {
            depth = maxDepth;
            depthInput.text = maxDepth.ToString();
        }
        if (width > maxWidth)
        {
            width = maxWidth;
            widthInput.text = maxWidth.ToString();
        }

        CreateDummiesRecursive(depth, width, gameObject, ref count);
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

}
