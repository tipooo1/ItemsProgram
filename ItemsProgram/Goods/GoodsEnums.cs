
using System;
using System.Linq;

namespace GoodsEditor
{
    /// <summary>
    /// 物品类型
    /// </summary>
    enum EnumGoodsType
    {
        None = 0,
        #region 素材类（1XXXXXX）
        //矿石类11XXXXX（矿石1110XXX，宝石1111XXX，铸锭1112XXX）
        //生物素材类12XXXXX（兽皮1210XXX，兽爪1211XXX，兽骨1212XXX，兽肉1213XXX）
        //自然素材类13XXXXX（草药1310XXX，木材1311XXX，果实1312XXX）
        //特殊素材类14XXXXX（掉落特殊素材1410XXX，任务特殊素材1411XXX）
        #endregion
        #region 装备类（2XXXXXX）
        //武器类21XXXXX（单手剑2110XXX，双手剑2111XXX，弓2112XXX，弩2113XXX，盾2114XXX，匕首2115XXX，长杖2116XXX，短杖2117XXX，水晶球2118XXX）
        //头盔类22XXXXX（头盔2210XXX，头环2211XXX，兜帽2212XXX）
        //盔甲类23XXXXX（重甲2310XXX，皮甲2311XXX，法袍2312XXX）
        //靴子类24XXXXX（甲靴2410XXX，皮靴2411XXX，布靴2412XXX）
        //饰品类25XXXXX（项链2510XXX，戒指2511XXX，护身符2512XXX，勋章2513XXX）
        #endregion
        #region 道具类（3XXXXXX）
        //投掷道具类31XXXXX（炸弹类3110XXX，飞行道具类（飞镖等）3111XXX）
        //魔偶类32XXXXX（可制作魔偶3210XXX，不可制作魔偶3211XXX）
        //陷阱类33XXXXX（固定点陷阱3310XXX，特殊用法陷阱3311XXX）
        #endregion
        #region 炼金，药剂类（4XXXXXX）
        //酒类41XXXXX（普通酒4110XXX）
        //恢复药剂类42XXXXX（恢复药剂4210XXX）
        //强化药剂类43XXXXX（强化药剂4310XXX）
        //附魔药剂类44XXXXX（附魔药剂4410XXX）
        //功能药剂类45XXXXX（功能药剂4510XXX）
        #endregion
        #region 特殊物品类（5XXXXXX）
        //书籍类51XXXXX（故事书5110XXX，技能书5111XXX，信笺5112XXX）
        #endregion
    }

    /// <summary>
    /// 所有品质枚举_普通，罕见，稀有，传说，史诗，唯一（白，绿，蓝，紫，橙，红）
    /// </summary>
    enum EnumQualityType
    {
        White,
        Green,
        Blue,
        Purple,
        Yellow,
        Red
    }

    /// <summary>
    /// 物品能力类型
    /// </summary>
   public enum EnumGoodsAbility
    {
        None = 0,
        /// <summary>
        /// 生命值附加
        /// </summary>
        [EnumDisplay("生命值附加")]
        HP = 10,
        /// <summary>
        /// 法力值附加
        /// </summary>
        [EnumDisplay("法力值附加")]
        MP = 20,
        /// <summary>
        /// 灵巧值附加
        /// </summary>
        [EnumDisplay("灵巧值附加")]
        DEX = 30,
        /// <summary>
        /// 专注值附加
        /// </summary>
        [EnumDisplay("专注值附加")]
        HIT = 40,
        /// <summary>
        /// 意志值附加
        /// </summary>
        [EnumDisplay("意志值附加")]
        WIL = 50,
        /// <summary>
        /// 力量值附加
        /// </summary>
        [EnumDisplay("力量值附加")]
        STR = 60,


        /// <summary>
        /// 物品特殊能力类型（例如暗影勋章这种物品的特殊能力）
        /// </summary>
        特殊能力类型开始 = 10000,
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayAttribute : Attribute
    {
        public EnumDisplayAttribute(string display)
        {
            Display = display;
        }
        public string Display
        {
            get;
            private set;
        }
    }

    public static class EnumExtentions
    {
        public static string Display(this Enum t)
        {
            var t_type = t.GetType();
            var fieldName = Enum.GetName(t_type, t);
            var attributes = t_type.GetField(fieldName).GetCustomAttributes(false);
            var enumDisplayAttribute = attributes.FirstOrDefault(p => p.GetType().Equals(typeof(EnumDisplayAttribute))) as EnumDisplayAttribute;
            return enumDisplayAttribute == null ? fieldName : enumDisplayAttribute.Display;
        }
    }
}

