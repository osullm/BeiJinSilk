namespace SIMCA.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    public class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        public static Bitmap play
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("play", resourceCulture);
            }
        }

        public static Bitmap play1
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("play1", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("SIMCA.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        public static Bitmap save
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("save", resourceCulture);
            }
        }

        public static Bitmap stop
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stop", resourceCulture);
            }
        }

        public static Bitmap 黄箭头
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("黄箭头", resourceCulture);
            }
        }

        public static Bitmap 建模
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("建模", resourceCulture);
            }
        }

        public static Bitmap 软件信息
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("软件信息", resourceCulture);
            }
        }

        public static Bitmap 软件信息___副本
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("软件信息 - 副本", resourceCulture);
            }
        }

        public static Bitmap 退出
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("退出", resourceCulture);
            }
        }

        public static Bitmap 验证
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("验证", resourceCulture);
            }
        }

        public static Bitmap 样品
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("样品", resourceCulture);
            }
        }

        public static Bitmap 预测
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("预测", resourceCulture);
            }
        }
    }
}

