/*
    用于记录常用变量和通用计算
 */
public static class Common
{
    public enum GameMode
    {
        step_1 = 1,
        step_2 = 2,
        step_3 = 3,
    }

    public static float[] timeList = new float[3] { 0, 300, 200 };

    public static string ScontToTime(float scont)
    {
        string str = "";
        int _hour = (int)(scont / 3600);
        scont = scont % 3600;
        int _minue = (int)(scont / 60);
        scont = scont % 60;
        str = _hour != 0 ? _hour + ":" : "";
        str = str + (_minue != 0 ? _minue + ":" : "");
        str += (int)scont;
        return str;
    }
}
