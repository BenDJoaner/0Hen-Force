/*
    用于记录常用变量和通用计算
 */
public static class Common
{
    //游戏阶段
    public enum GameMode
    {
        step_0 = 0,
        step_1 = 1,
        step_2 = 2,
        step_3 = 3,
    }

    //角色类型
    public enum PosEnum
    {
        迅捷 = 0,
        强攻 = 1,
        支援 = 2
    }

    //(被)攻击效果
    public enum AttackEffect
    {
        无 = 0,
        强位移 = 1,
        魅惑 = 2,
        恐惧 = 3,
        禁锢 = 4,
        减速 = 5,
        凝滞 = 6,
    }

    //作用对象
    public enum EffectTo
    {
        自己 = 0,
        敌人 = 1,
        队友 = 2,
        所有人 = 3
    }

    //第一阶段收集能量的总数
    public static int Step_1_Sum = 6;

    //角色类型显示的文字
    public static string[] PosList = new string[] { "迅捷", "强攻", "支援" };

    //第二阶段时间缩放
    public static float decTime_1 = 1f;

    //各个阶段的限制时间（0为无限）
    public static float[] timeList = new float[3] { 0, 300, 200 };

    //将 xxxxx秒 转化为 xx时xx分xx秒
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
