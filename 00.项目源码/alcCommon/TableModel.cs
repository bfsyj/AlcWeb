using System;
using System.Collections.Generic;

namespace alcCommon
{
    /// <summary>
    /// 数据表模型(前端模型，需要小写)
    /// </summary>
    /// <typeparam name="T">数据实体</typeparam>
    public class TableModel<T> where T : new()
    {
        /// <summary>
        /// 初始化列表返回数据模型
        /// </summary>
        /// <param name="msg">返回信息</param>
        public TableModel(string msgStr)
        {
            code = -1;
            msg = msgStr;
            data = null;
            count = 0;
        }

        /// <summary>
        /// 初始化列表返回数据模型
        /// </summary>
        /// <param name="list">数据列表集合</param>
        /// <param name="total">总条数</param>
        public TableModel(List<T> list, int total)
        {
            code = 0;
            msg = "";
            data = list;
            count = total;
        }

        /// <summary>
        /// 请求是否成功,(0：成功)
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> data { get; set; }
    }
}
