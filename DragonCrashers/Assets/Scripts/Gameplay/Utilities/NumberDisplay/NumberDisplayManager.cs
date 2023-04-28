using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDisplayManager : MonoBehaviour
{   
    [Header("Object Pool of Numbers")]
    public ObjectPoolBehaviour objectPool;
    
    [Header("Position Random Offset")]
    public Vector3 positionRandomOffsetRange;

    public void ShowNumber(int numberAmount, Transform numberTransform, Color numberColor)
    {
        GameObject numberObject = objectPool.GetPooledObject();

        Vector3 newPosition = numberTransform.position + RandomOffsetRange(positionRandomOffsetRange);

        numberObject.GetComponent<NumberDisplayBehaviour>().SetupDisplay(numberAmount, newPosition, numberColor);
        numberObject.SetActive(true);
    }

    Vector3 RandomOffsetRange(Vector3 rangeVectors)
    {
        
        return new Vector3( RandomInRange(rangeVectors.x),
                            RandomInRange(rangeVectors.y),
                            RandomInRange(rangeVectors.x)
        );
    }

    float RandomInRange(float rangeValue)
    {
        return Random.Range(-rangeValue, rangeValue);
    }

}
