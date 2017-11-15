using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsEditor
{
    /// <summary>
    /// 所有游戏内物品基类
    /// </summary>
    class Goods
    {
        #region 物品基础通用属性
        //物品ID
        EnumGoodsType ID = EnumGoodsType.None;
        //重量
        float Weight = 0;
        //基础价格
        int Price = 0;
        //文字说明
        string Explain = "道具说明";
        //物品具有的属性列表
        List<GoodsNature> itemnatures = new List<GoodsNature>();
        #endregion

        /// <summary>
        /// 向属性列表中添加一项的方法
        /// </summary>
        /// <param name="natureType"></param>
        /// <param name="natureNumber"></param>
        /// <param name="natureExplain"></param>
        public void AddItemNature(EnumGoodsAbility natureType, float natureNumber, string natureExplain)
        {
            itemnatures.Add(new GoodsNature(natureType, natureNumber, natureExplain));
        }
        /// <summary>
        /// 清空物品属性列表
        /// </summary>
        public void DeleteItemNature()
        {
            itemnatures.Clear();
        }

        public Goods(int ID)
        {
            //待修改ZXW
            //根据传入的ID寻找并反序列化对应的物品信息
        }
    }
    /// <summary>
    /// 物品属性
    /// </summary>
    class GoodsNature
    {
        #region 物品属性的基本元素
        /// <summary>
        /// 物品属性类型
        /// </summary>
        private EnumGoodsAbility NatureType = EnumGoodsAbility.None;
        /// <summary>
        /// 物品属性数值
        /// </summary>
        private float NatureNumber = 0;
        /// <summary>
        /// 物品属性说明（字典存储）
        /// </summary>
        private string NatureExplain = "物品属性说明";
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="natureType"></param>
        /// <param name="natureNumber"></param>
        /// <param name="natureExplain"></param>
        public GoodsNature(EnumGoodsAbility natureType, float natureNumber, string natureExplain)
        {
            NatureType = natureType;
            NatureNumber = natureNumber;
            NatureExplain = natureExplain;
        }
    }
}
