using System;
using System.Collections.Generic;
using System.Text;

namespace alcCommon
{
    public class MenuModel
    {
        /// <summary>
		/// 菜单名称
		/// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Href { set; get; }

        /// <summary>
		/// 显示图标
		/// </summary>
        public string Icon { set; get; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Spread { get; set; }

        /// <summary>
        /// 标记
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public MenuModel[] Children { get; set; }
    }
}
