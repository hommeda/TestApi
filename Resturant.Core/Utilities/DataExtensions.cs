using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Resturant.Core.Utilities
{
    public static class DataExtensions
    {
        #region EntityFramework Extensions

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "source is null.");

            if (string.IsNullOrEmpty(sortExpression))
                throw new ArgumentException("sortExpression is null or empty.", nameof(sortExpression));

            var parts = sortExpression.Split(' ');
            var isDescending = false;
            var tType = typeof(T);

            if (parts.Length <= 0 || parts[0] == "") return source;
            var propertyName = parts[0];

            if (parts.Length > 1)
                isDescending = parts[1].ToLower().Contains("esc");

            var prop = tType.GetProperty(propertyName);

            if (prop == null)
                throw new ArgumentException($"No property '{propertyName}' on type '{tType.Name}'");

            var funcType = typeof(Func<,>).MakeGenericType(tType, prop.PropertyType);

            var lambdaBuilder = typeof(Expression)
                .GetMethods()
                .First(x => x.Name == "Lambda" && x.ContainsGenericParameters && x.GetParameters().Length == 2)
                .MakeGenericMethod(funcType);

            var parameter = Expression.Parameter(tType);
            var propExpress = Expression.Property(parameter, prop);

            var sortLambda = lambdaBuilder.Invoke(null, new object[] { propExpress, new[] { parameter } });

            var sorter = typeof(Queryable)
                .GetMethods()
                .FirstOrDefault(x => x.Name == (isDescending ? "OrderByDescending" : "OrderBy") && x.GetParameters().Length == 2)
                ?.MakeGenericMethod(new[] { tType, prop.PropertyType });

            if (sorter != null) return (IQueryable<T>) sorter.Invoke(null, new[] {source, sortLambda});

            return source;
        }

        #endregion

        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
        }

        public static DataTable ConvertToDataTable<T>(this IList<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
        }

        public static DataRow ConvertToDataRow<T>(this T item, DataTable table)
        {
            var properties =
                TypeDescriptor.GetProperties(typeof(T));
            var row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            return row;
        }

        private static T ConvertToEntity<T>(this DataRow tableRow) where T : new()
        {
            // Create a new type of the entity I want
            var t = typeof(T);
            var returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                var colName = col.ColumnName;

                // Look for the object's property with the columns name, ignore case
                var pInfo = t.GetProperty(colName.ToLower(),
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // did we find the property ?
                if (pInfo != null)
                {
                    var val = tableRow[colName];

                    // is this a Nullable<> type
                    var isNullable = Nullable.GetUnderlyingType(pInfo.PropertyType) != null;
                    if (isNullable)
                    {
                        val = val is DBNull ? null : Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType)!);
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity
                        SetDefaultValue(ref val, pInfo.PropertyType);
                        if (pInfo.PropertyType.IsEnum && !pInfo.PropertyType.IsGenericType)
                        {
                            val = Enum.ToObject(pInfo.PropertyType, val);
                        }
                        else
                            val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    // Set the value of the property with the value from the db
                    if (pInfo.CanWrite)
                        pInfo.SetValue(returnObject, val, null);
                }
            }

            // return the entity object with values
            return returnObject;
        }

        private static void SetDefaultValue(ref object val, Type propertyType)
        {
            if (val is DBNull)
            {
                val = GetDefault(propertyType);
            }
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static List<T> ConvertToList<T>(this DataTable table) where T : new()
        {
            // Create a list of the entities we want to return

            // Iterate through the DataTable's rows

            // Return the finished list
            return (from DataRow dr in table.Rows select dr.ConvertToEntity<T>()).ToList();
        }

        public static List<Dictionary<string, object>> ConvertToList(this DataTable table)
        {
            // Create a list of the entities we want to return
            var returnObject = new List<Dictionary<string, object>>();

            // Iterate through the DataTable's rows
            foreach (DataRow dr in table.Rows)
            {
                var row = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                returnObject.Add(row);
            }

            // Return the finished list
            return returnObject;
        }
    }
}
