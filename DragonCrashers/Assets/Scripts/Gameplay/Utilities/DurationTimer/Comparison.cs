/*
Script Source: https://coffeebraingames.wordpress.com/2013/12/18/a-generic-floating-point-comparison-class/
*/

using UnityEngine;
 
/**
 * Class for comparing floating point values 
 */
public static class Comparison {
 
    /**
     * Returns whether or not a == b
     */
    public static bool TolerantEquals(float a, float b) {
        return Mathf.Approximately(a, b);
    }
 
    /**
     * Returns whether or not a >= b
     */
    public static bool TolerantGreaterThanOrEquals(float a, float b) {
        return a > b || TolerantEquals(a, b);
    }
 
    /**
     * Returns whether or not a <= b
     */
    public static bool TolerantLesserThanOrEquals(float a, float b) {
        return a < b || TolerantEquals(a, b);
    }
 
}