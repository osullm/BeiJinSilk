namespace JSDU
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ListEditView : UserControl
    {
        private List<ListEditViewItem> _Items = new List<ListEditViewItem>();
        private int _LineHeight = 20;
        private IContainer components;
        private vListView ListViewCustom;
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;

        public ListEditView()
        {
            this.InitializeComponent();
            this.ListViewCustom.CheckBoxes = false;
            this.ListViewCustom.FullRowSelect = true;
            this.ListViewCustom.OnScroll += new vListView.ListViewScroll(this.ControlScroll);
            this.ListViewCustom.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.DrawColumnHeader);
            this.SetLineHeight();
        }

        public void AddColumns(EditViewColumnHeader Header)
        {
            this.ListViewCustom.Columns.Add(Header);
        }

        public void AddItem(ListEditViewItem items)
        {
            if (items.Items.Count != this.ListViewCustom.Columns.Count)
            {
                throw new Exception("你提供的数据列数与标题列的数目不同");
            }
            for (int i = 0; i < items.Items.Count; i++)
            {
                EditViewColumnHeader header = this.ListViewCustom.Columns[i] as EditViewColumnHeader;
                string text = "";
                if (header.ColumnStyle == ListEditViewColumnStyle.Control)
                {
                    if (!(items.Items[i] is Control))
                    {
                        throw new Exception("列数据类型不正确!");
                    }
                    this.ListViewCustom.Controls.Add(items.Items[i] as Control);
                }
                else
                {
                    text = items.Items[i].ToString();
                }
                if (i > 0)
                {
                    ListViewItem.ListViewSubItem item = new ListViewItem.ListViewSubItem(items, text);
                    items.SubItems.Add(item);
                }
            }
            this.Items.Add(items);
            this.ListViewCustom.Items.Add(items);
            this.MoveControl();
        }

        private void ControlScroll(object sender, bool vscroll)
        {
            this.MoveControl();
        }

        public void DataBind(List<ListEditViewItem> items)
        {
            foreach (ListEditViewItem item in items)
            {
                this.AddItem(item);
            }
        }

        private void DelControl(ListEditViewItem items)
        {
            ListEditViewItem item = items;
            for (int i = 0; i < this.ListViewCustom.Columns.Count; i++)
            {
                EditViewColumnHeader header = this.ListViewCustom.Columns[i] as EditViewColumnHeader;
                if (header.ColumnStyle == ListEditViewColumnStyle.Control)
                {
                    (item.Items[i] as Control).Dispose();
                }
            }
        }

        public void DeleteItem(ListEditViewItem items)
        {
            this.Items.Remove(items);
            this.ListViewCustom.Items.Remove(items);
            this.MoveControl();
        }

        public void DeleteItem(int index)
        {
            this.DelControl(this.Items[index]);
            this.Items.RemoveAt(index);
            this.ListViewCustom.Items.RemoveAt(index);
            this.MoveControl();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawColumnHeader(object Sender, DrawListViewColumnHeaderEventArgs ex)
        {
            this.MoveControl();
        }

        private void InitializeComponent()
        {
            this.ListViewCustom = new vListView();
            base.SuspendLayout();
            this.ListViewCustom.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.ListViewCustom.BorderStyle = BorderStyle.None;
            this.ListViewCustom.Cursor = Cursors.Default;
            this.ListViewCustom.GridLines = true;
            this.ListViewCustom.ImeMode = ImeMode.NoControl;
            this.ListViewCustom.LabelEdit = true;
            this.ListViewCustom.Location = new Point(0, 0);
            this.ListViewCustom.Name = "ListViewCustom";
            this.ListViewCustom.Size = new Size(380, 320);
            this.ListViewCustom.TabIndex = 0;
            this.ListViewCustom.UseCompatibleStateImageBehavior = false;
            this.ListViewCustom.View = View.Details;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.ListViewCustom);
            base.Name = "ListEditView";
            base.Size = new Size(380, 320);
            base.ResumeLayout(false);
        }

        private void MoveControl()
        {
            for (int i = 0; i < this.ListViewCustom.Items.Count; i++)
            {
                ListEditViewItem item = this.ListViewCustom.Items[i] as ListEditViewItem;
                for (int j = 0; j < this.ListViewCustom.Columns.Count; j++)
                {
                    Point point;
                    Size size;
                    EditViewColumnHeader header = this.ListViewCustom.Columns[j] as EditViewColumnHeader;
                    if (item.SubItems[j].Bounds.Y <= 5)
                    {
                        point = new Point(item.SubItems[j].Bounds.X, item.SubItems[j].Bounds.Y - 20);
                    }
                    else
                    {
                        point = new Point(item.SubItems[j].Bounds.X, item.SubItems[j].Bounds.Y);
                    }
                    if ((j == 0) && (this.ListViewCustom.Columns.Count > 1))
                    {
                        size = new Size(item.SubItems[1].Bounds.X - item.SubItems[0].Bounds.X, item.SubItems[j].Bounds.Height);
                    }
                    else
                    {
                        size = new Size(item.SubItems[j].Bounds.Width, item.SubItems[j].Bounds.Height);
                    }
                    if (header.ColumnStyle == ListEditViewColumnStyle.Control)
                    {
                        Control control = item.Items[j] as Control;
                        control.Location = point;
                        control.Size = size;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.MoveControl();
        }

        public void RefreshControl()
        {
            this.MoveControl();
        }

        private void SetLineHeight()
        {
            Image image = new Bitmap(1, this._LineHeight);
            ImageList list = new ImageList {
                ImageSize = new Size(1, this._LineHeight)
            };
            list.Images.Add(image);
            this.ListViewCustom.SmallImageList = list;
        }

        public bool GridLines
        {
            get
            {
                return this.ListViewCustom.GridLines;
            }
            set
            {
                this.ListViewCustom.GridLines = value;
            }
        }

        public List<ListEditViewItem> Items
        {
            get
            {
                return this._Items;
            }
        }

        public int LineHeight
        {
            get
            {
                return this._LineHeight;
            }
            set
            {
                this._LineHeight = value;
                this.SetLineHeight();
            }
        }

        public Control.ControlCollection ListViewControls
        {
            get
            {
                return this.ListViewCustom.Controls;
            }
        }
    }
}

