using System.Data;
using System.Dynamic;

namespace server.Helpers
{
    public static class DataTableHelper
    {
        public static List<dynamic> ToDynamicList(this DataTable dt)
        {
            var list = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                var item = new ExpandoObject() as IDictionary<string, object>;
                foreach (DataColumn col in dt.Columns)
                {
                    item[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                list.Add(item);
            }
            return list;
        }
    }
}