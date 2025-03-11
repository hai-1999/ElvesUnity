using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ElvesType
{
    None,
    Õý³£,
    »ð,
    Ë®,
    ²Ý,

}

public class TypeChart
{
    static float[][] chart =
    {
        //att/def           NOR   FIR   WAT   GRA  
        /*NOR*/ new float[] {1f,   1f,   1f,   1f },
        /*FIR*/ new float[] {1f,  0.5f, 0.5f,  2f },
        /*WAT*/ new float[] {1f,   2f,  0.5f, 0.5f},
        /*GRA*/ new float[] {1f,   1f,   2f,  0.5f},
    };

    public static float GetEffectiveness(ElvesType att,ElvesType def)
    {
        if (att == ElvesType.None || def == ElvesType.None)
            return 1;

        int row = (int)att - 1;
        int col = (int)def - 1;

        return chart[row][col];
    }

}