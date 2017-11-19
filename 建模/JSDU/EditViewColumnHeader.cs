namespace JSDU
{
    using System;
    using System.Windows.Forms;

    public class EditViewColumnHeader : ColumnHeader
    {
        private System.Type _Type;
        private ListEditViewColumnStyle cStyle;

        public EditViewColumnHeader()
        {
            this.cStyle = ListEditViewColumnStyle.Text;
        }

        public EditViewColumnHeader(ListEditViewColumnStyle cStyle)
        {
            this.cStyle = cStyle;
            this.SetType();
        }

        private void SetType()
        {
            if (this.cStyle == ListEditViewColumnStyle.Text)
            {
                this._Type = typeof(string);
            }
            else if (this.cStyle == ListEditViewColumnStyle.Control)
            {
                this._Type = typeof(Control);
            }
        }

        public ListEditViewColumnStyle ColumnStyle
        {
            get
            {
                return this.cStyle;
            }
            set
            {
                this.cStyle = value;
                this.SetType();
            }
        }
    }
}

