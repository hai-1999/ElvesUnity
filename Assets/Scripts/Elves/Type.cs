public enum Type
{
    None,
    ����,
    ��,
    ˮ,
    ��,
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

    public static float GetEffectiveness(Type att,Type def)
    {
        if (att == Type.None || def == Type.None)
            return 1;

        int row = (int)att - 1;
        int col = (int)def - 1;

        return chart[row][col];
    }

}