namespace JSDU
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class vListView : ListView
    {
        public event ListViewScroll OnScroll;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x114:
                case 0x115:
                case 0x20a:
                case 0x4e:
                    if (this.OnScroll != null)
                    {
                        this.OnScroll(this, m.Msg == 0x115);
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        public delegate void ListViewScroll(object sender, bool vscroll);
    }
}

