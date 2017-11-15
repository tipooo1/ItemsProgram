using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoodsEditor
{
    public partial class GoodsControl : Form
    {
        public GoodsControl()
        {
            InitializeComponent();

            itemInformationform = new GoodsInfoMation();
            InitialGoodsAbilitys();
            itemInformationform.Parent = this.panel1;
            this.panel1.Width = itemInformationform.Width;
            this.Width = this.panel1.Width;
        }

        /// <summary>
        /// 初始化已保存的物品能力
        /// </summary>
        private void InitialGoodsAbilitys()
        {
            if (File.Exists("allGoodsAbility.ability"))
            {
                allGoodsAbilitys=JsonConvert.DeserializeObject<Dictionary<int, List<GoodsAbility>>>(File.ReadAllText("allGoodsAbility.ability"));
            }
        }

        GoodsInfoMation itemInformationform = null;

        /// <summary>
        /// 所有物品的能力
        /// </summary>
      public static  Dictionary<int, List<GoodsAbility>> allGoodsAbilitys = new Dictionary<int, List<GoodsAbility>>();

        private void 加载文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                itemInformationform.ReadListFile(ofd.FileName);
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool sucess = itemInformationform.WriteListFile();
            bool sucess2 = SaveGoodsAbilitys();
            MessageBox.Show(string.Format("写入{0}", sucess && sucess2 ? "成功" : "失败"));
        }

        /// <summary>
        /// 保存物品能力
        /// </summary>
        /// <returns></returns>
        private bool SaveGoodsAbilitys()
        {
            try
            {
                string output = JsonConvert.SerializeObject(allGoodsAbilitys);
                File.WriteAllText("allGoodsAbility.ability", output);
                return true;
            }
            catch
            {
                return false;
            }

        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridView datadridView = this.panel1.Controls[0].Controls[0] as DataGridView;
            if (datadridView[0, datadridView.CurrentCell.RowIndex].Value != null)
            {
                GoodsEditor editor = new GoodsEditor(int.Parse(datadridView[0, datadridView.CurrentCell.RowIndex].Value.ToString()));
                editor.GoodsAbilitysChanged += Editor_GoodsAbilitysChanged;
                editor.ShowDialog();
            }
        }

        private void Editor_GoodsAbilitysChanged(int id, List<GoodsAbility> abilitys)
        {
            if (allGoodsAbilitys.ContainsKey(id))
            {
                allGoodsAbilitys[id] = abilitys;

            }
            else
            {
                allGoodsAbilitys.Add(id, abilitys);
            }
        }
    }
}
