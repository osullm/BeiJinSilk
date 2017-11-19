namespace JSDU
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class ListEditViewItem : ListViewItem
    {
        private List<object> _Items = new List<object>(0);
        private bool First = true;

        public void AddSubItem(object subItem)
        {
            this.Items.Add(subItem);
            if (this.First)
            {
                this.First = false;
                if (subItem is string)
                {
                    base.SubItems[0] = new ListViewItem.ListViewSubItem(this, subItem.ToString());
                }
                else
                {
                    base.SubItems[0] = new ListViewItem.ListViewSubItem(this, "");
                }
            }
        }

        public List<object> Items
        {
            get
            {
                return this._Items;
            }
        }
    }
}

