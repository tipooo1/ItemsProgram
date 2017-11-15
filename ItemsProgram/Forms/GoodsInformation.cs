﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GoodsEditor
{
    public partial class GoodsInfoMation : UserControl
    {
        public GoodsInfoMation()
        {
            InitializeComponent();
            Column2.ReadOnly = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.MouseMove += DataGridView1_MouseMove;
            dataGridView1.CellMouseDoubleClick += DataGridView1_CellMouseDoubleClick;
            Init();
        }

        /// <summary>
        /// 详细编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0 && e.RowIndex <dataGridView1.RowCount)
            {
                if (dataGridView1[0, e.RowIndex].Value == null) return;
                int id = int.Parse(dataGridView1[0, e.RowIndex].Value.ToString());
                GoodsEditor itemEditor = new GoodsEditor(id);
                itemEditor.GoodsAbilitysChanged += ItemEditor_GoodsAbilitysChanged;
                itemEditor.ShowDialog();
            }
        }

        private void ItemEditor_GoodsAbilitysChanged(int id, List<GoodsAbility> abilitys)
        {
            if (GoodsControl.allGoodsAbilitys.ContainsKey(id))
            {
                GoodsControl.allGoodsAbilitys[id] = abilitys;

            }
            else
            {
                GoodsControl.allGoodsAbilitys.Add(id, abilitys);
            }
        }


        /// <summary>
        /// 强制combox列直接聚焦,实现单次点击combox列下拉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            var hitInfo = dataGridView1.HitTest(e.X, e.Y);
            if (hitInfo != DataGridView.HitTestInfo.Nowhere)
            {
                if (hitInfo.ColumnIndex >= 0 && hitInfo.ColumnIndex < dataGridView1.ColumnCount)
                {
                    var column = dataGridView1.Columns[hitInfo.ColumnIndex];
                    if (column.CellType == typeof(DataGridViewComboBoxCell))
                    {
                        if (hitInfo.RowIndex != dataGridView1.NewRowIndex && hitInfo.RowIndex >= 0 && hitInfo.RowIndex < dataGridView1.RowCount)
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[hitInfo.RowIndex].Cells[hitInfo.ColumnIndex];
                            dataGridView1.BeginEdit(true);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 强制触发combox  内容改变后valueChanged事件，否则得等待失焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 4 && e.ColumnIndex <= 6)
            {
                DataGridViewComboBoxCell cell4x = dataGridView1[4, e.RowIndex] as DataGridViewComboBoxCell;
                DataGridViewComboBoxCell cell5x = dataGridView1[5, e.RowIndex] as DataGridViewComboBoxCell;
                DataGridViewComboBoxCell cell6x = dataGridView1[6, e.RowIndex] as DataGridViewComboBoxCell;
                if (e.ColumnIndex == 4 && cell4x.Value != null)
                {
                    RefreshFuncTypeComboxItems(e.RowIndex);

                    RefreshSpecialTypeComboxItems(e.RowIndex);
                }
                if (e.ColumnIndex == 5 && cell5x.Value != null)
                {
                    RefreshSpecialTypeComboxItems(e.RowIndex);
                }

                if (cell4x.Value != null && cell5x.Value != null && cell6x.Value != null)
                {
                    int baseType = GoodsDictionarys.Instance().GoodsBaseType[cell4x.Value.ToString()];
                    var funcTypes = GoodsDictionarys.Instance().GoodsFuncType(baseType);
                    int funcNumber = funcTypes.ToDictionary(kv => kv.Value, kv => kv.Key)[cell5x.Value.ToString()];
                    var specialComboxItems = GoodsDictionarys.Instance().GoodSpecialType(funcNumber);
                    int specialNumer = specialComboxItems.ToDictionary(kv => kv.Value, kv => kv.Key)[cell6x.Value.ToString()];


                    string idHeader = specialNumer.ToString();
                    //产生id
                    List<int> ids = new List<int>();
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (i != dataGridView1.CurrentCell.RowIndex)
                        {
                            if (dataGridView1[0, i].Value == null)
                                continue;
                            int id = int.Parse(dataGridView1[0, i].Value.ToString());
                            ids.Add(id);
                        }
                    }

                    var SameKindId = ids.Where(id => id.ToString().Substring(0, 4).Equals(idHeader)).Select(id => Convert.ToInt32(id.ToString().Substring(4))).ToArray();
                    Array.Sort(SameKindId);
                    string generateID = string.Empty;
                    if (SameKindId.Length == 0)
                    {
                        generateID = idHeader + "001";
                    }
                    else
                    {
                        generateID = idHeader + (SameKindId.Last() + 1).ToString().PadLeft(3, '0');
                    }
                    dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value = generateID;
                }

            }
        }

        /// <summary>
        /// 刷新具体类型选择框
        /// </summary>
        /// <param name="rowIndex"></param>
        private void RefreshSpecialTypeComboxItems(int rowIndex)
        {
            DataGridViewComboBoxCell cell4x = dataGridView1[4, rowIndex] as DataGridViewComboBoxCell;
            DataGridViewComboBoxCell cell5x = dataGridView1[5, rowIndex] as DataGridViewComboBoxCell;
            DataGridViewComboBoxCell cell6x = dataGridView1[6, rowIndex] as DataGridViewComboBoxCell;
            int baseType = GoodsDictionarys.Instance().GoodsBaseType[cell4x.Value.ToString()];
            var funcTypes = GoodsDictionarys.Instance().GoodsFuncType(baseType);
            int funcNumber = funcTypes.ToDictionary(kv => kv.Value, kv => kv.Key)[cell5x.Value.ToString()];
            var specialComboxItems = GoodsDictionarys.Instance().GoodSpecialType(funcNumber);
            cell6x.DataSource = specialComboxItems.Values.ToArray();
            cell6x.Value = specialComboxItems.Values.ToArray()[0];
        }

        /// <summary>
        /// 刷新功能类型选择框
        /// </summary>
        /// <param name="row"></param>
        private void RefreshFuncTypeComboxItems(int row)
        {
            DataGridViewComboBoxCell cell4x = dataGridView1[4, row] as DataGridViewComboBoxCell;
            DataGridViewComboBoxCell cell5x = dataGridView1[5, row] as DataGridViewComboBoxCell;
            int number = GoodsDictionarys.Instance().GoodsBaseType[cell4x.Value.ToString()];
            var funcType = GoodsDictionarys.Instance().GoodsFuncType(number);
            cell5x.DataSource = funcType.Values.ToArray();
            cell5x.Value = funcType.Values.ToArray()[0];
        }

        /// <summary>
        /// 获取功能类型
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private string[] GetFuncType(string baseType)
        {
            int number = GoodsDictionarys.Instance().GoodsBaseType[baseType];
            var funcType = GoodsDictionarys.Instance().GoodsFuncType(number);
            return funcType.Values.ToArray();
        }


        /// <summary>
        /// 获取特殊类型列表
        /// </summary>
        /// <param name="funcType"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private string[] GetSpecialType(string funcType, string baseType)
        {
            int baseTypeNumber = GoodsDictionarys.Instance().GoodsBaseType[baseType];
            var funcTypes = GoodsDictionarys.Instance().GoodsFuncType(baseTypeNumber);
            int funcNumber = funcTypes.ToDictionary(kv => kv.Value, kv => kv.Key)[funcType];
            var specialComboxItems = GoodsDictionarys.Instance().GoodSpecialType(funcNumber);
            return specialComboxItems.Values.ToArray();

        }
        /// <summary>
        /// 配置文件存储位置
        /// </summary>
        private string ItemListDirectory = "";
        /// <summary>
        /// 所有物品列表存储数组（每次程序启动时读取）
        /// </summary>
        string[] itemlist = null;

        /// <summary>
        /// 控件初始化
        /// </summary>
        private void Init()
        {
            var baseType = GoodsDictionarys.Instance().GoodsBaseType.Keys.ToList();
            this.GoodsBaseTypeColumn.Items.Clear();
            this.baseType = baseType.ToArray();
            this.GoodsBaseTypeColumn.Items.AddRange(baseType.ToArray());
            ReadListFile();
        }

        private string[] baseType;

        /// <summary>
        /// 按物品基础属性向表格中添加一行
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        private void AddUnits(string ID, string Name, float weight, int price, string complain)
        {
            string itemType = GoodsDictionarys.Instance().GetGoodsType(int.Parse(ID));
            string[] ItemType = itemType.Split(',');
            if (ItemType.Length == 3)
            {
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewCell[] cell = new DataGridViewCell[8];
                for (int i = 0; i < cell.Length; i++)
                {
                    if (i == 4 || i == 5 || i == 6)
                    {
                        cell[i] = new DataGridViewComboBoxCell() { FlatStyle = FlatStyle.Flat, Style = new DataGridViewCellStyle() { BackColor = Color.White } };

                    }
                    else
                    {
                        cell[i] = new DataGridViewTextBoxCell();
                    }
                }
                cell[0].ToolTipText = "双击设置详细信息";
                cell[0].Value = ID;
                cell[1].Value = Name;
                cell[2].Value = weight;
                cell[3].Value = price;
                var cell4 = cell[4] as DataGridViewComboBoxCell;
                cell4.Items.AddRange(baseType);
                cell4.Value = ItemType[0];
                var cell5 = cell[5] as DataGridViewComboBoxCell;
                cell5.Items.AddRange(GetFuncType(ItemType[0]));
                cell[5].Value = ItemType[1];
                var cell6 = cell[6] as DataGridViewComboBoxCell;
                string[] items = GetSpecialType(ItemType[1], ItemType[0]);
                cell6.Items.AddRange(items);
                cell6.Value = ItemType[2];
                cell[7].Value = complain;
                r.Cells.AddRange(cell);
                dataGridView1.Rows.Add(r);
            }
        }

        /// <summary>
        /// 双击一个单元格时显示该物品的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView1[0, e.RowIndex].Value.ToString());
                GoodsEditor itemEditor = new GoodsEditor(id);
                itemEditor.ShowDialog();
            }
            catch
            {
                MessageBox.Show("未能正确获取到指定物品ID，请检查后重试");
            }
        }

        #region 功能方法
        /// <summary>
        /// 从指定文件中获取所有物品列表信息,并写入表格中
        /// </summary>
        public void ReadListFile()
        {
            dataGridView1.Rows.Clear();
            //指定位置读取所有物品列表
            itemlist = File.ReadAllLines(System.Environment.CurrentDirectory + "\\itemlist.txt");
            //遍历文件所有行，读取物品列表信息
            for (int i = 0; i < itemlist.Length; i++)
            {
                string[] item = itemlist[i].Split('~');
                AddUnits(item[0], item[1], float.Parse(item[2]), int.Parse(item[3]), item[4]);
            }
        }

        public void ReadListFile(string filePath)
        {
            if (!File.Exists(filePath)) return;
            dataGridView1.Rows.Clear();
            //指定位置读取所有物品列表
            itemlist = File.ReadAllLines(filePath);
            //遍历文件所有行，读取物品列表信息
            for (int i = 0; i < itemlist.Length; i++)
            {
                string[] item = itemlist[i].Split('~');
                AddUnits(item[0], item[1], float.Parse(item[2]), int.Parse(item[3]), item[4]);
            }
        }


        /// <summary>
        /// 向指定文件中写入所有物品列表信息
        /// </summary>
        public bool WriteListFile()
        {
            //文件流
            StreamWriter sr = File.CreateText("itemlist.txt");
            bool WrongOccured = false;
            //循环写入文件流
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                try
                {
                    sr.WriteLine(dataGridView1[0, i].Value.ToString() + "~" + dataGridView1[1, i].Value.ToString() + "~" +
                        dataGridView1[2, i].Value.ToString() + "~" + dataGridView1[3, i].Value.ToString() + "~" + dataGridView1[7, i].Value.ToString());
                }
                catch (Exception ex)
                {

                    WrongOccured = true;
                }
            }
            sr.Close();

            return !WrongOccured;
        }
        #endregion
    }
}
