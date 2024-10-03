using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Resturant.Core.Utilities
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        //public static void AddPagination(this HttpResponse response,
        //    int currentPage, int itemsPerPage, int totalItems, int totalPages)
        //{
        //    var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
        //    var camelCaseFormatter = new JsonSerializerSettings();
        //    camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //    response.Headers.Add("Pagination",
        //        JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
        //    response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        //}

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }

        public static string ExceptionMessage(this Exception ex)
        {
            var exceptionMessage = ex.Message;
            if (ex.InnerException == null) return exceptionMessage;
            exceptionMessage += Environment.NewLine + ex.InnerException.Message;
            if (ex.InnerException.InnerException != null)
                exceptionMessage += Environment.NewLine + ex.InnerException.InnerException.Message;
            return exceptionMessage;
        }

        public static string MessageDuplicate(this string str)
        {
            var result = "";
            if (!str.Contains("Cannot insert duplicate key in object")) return result;
            var first = str.IndexOf("The duplicate key value is", StringComparison.Ordinal) + 28;
            var length = str.IndexOf("The statement has been terminated", StringComparison.Ordinal) -
                         first - 4;
            result = "عفوا تم تسجيل هذه القيمة من قبل " + " : " + str.Substring(first, length);
            return result;
        }

        public static List<long> ToLongList(this string str)
        {
            return str?.Split(',').Select(long.Parse).ToList();
        }

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            using var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            var sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);


        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            var result = source.SelectMany(selector);
            return !result.Any() ? result : result.Concat(result.SelectManyRecursive(selector));
        }

        //public static class IQueryableExtensions
        //{
        //    private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        //    private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        //    private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

        //    private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");

        //    private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        //    private static readonly FieldInfo QueryCompilationContextFactoryField = typeof(Database).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory");

        //    public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        //    {
        //        if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
        //        {
        //            throw new ArgumentException("Invalid query");
        //        }

        //        var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);
        //        var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
        //        var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
        //        var queryModel = parser.GetParsedQuery(query.Expression);
        //        var database = DataBaseField.GetValue(queryCompiler);
        //        var queryCompilationContextFactory = (IQueryCompilationContextFactory)QueryCompilationContextFactoryField.GetValue(database);
        //        var queryCompilationContext = queryCompilationContextFactory.Create(false);
        //        var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
        //        modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
        //        var sql = modelVisitor.Queries.First().ToString();

        //        return sql;
        //    }
        //}


        /// <summary> Generic interface for tree node structure </summary>
        /// <typeparam name="T"></typeparam>
        public interface ITree<T>
        {
            T Data { get; }
            ITree<T> Parent { get; }
            ICollection<ITree<T>> Children { get; }
            bool IsRoot { get; }
            bool IsLeaf { get; }
            int Level { get; }
        }
        /// <summary> Flatten tree to plain list of nodes </summary>
        public static IEnumerable<TNode> Flatten<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> childrenSelector)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            return nodes.SelectMany(c => childrenSelector(c).Flatten(childrenSelector)).Concat(nodes);
        }
        /// <summary> Converts given list to tree. </summary>
        /// <typeparam name="T">Custom data type to associate with tree node.</typeparam>
        /// <param name="items">The collection items.</param>
        /// <param name="parentSelector">Expression to select parent.</param>
        public static ITree<T> ToTree<T>(this IList<T> items, Func<T, T, bool> parentSelector)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var lookup = items.ToLookup(item => items.FirstOrDefault(parent => parentSelector(parent, item)),
                child => child);
            return Tree<T>.FromLookup(lookup);
        }
        /// <summary> Internal implementation of <see cref="ITree{T}" /></summary>
        /// <typeparam name="T">Custom data type to associate with tree node.</typeparam>
        internal class Tree<T> : ITree<T>
        {
            public T Data { get; }
            public ITree<T> Parent { get; private set; }
            public ICollection<ITree<T>> Children { get; }
            public bool IsRoot => Parent == null;
            public bool IsLeaf => Children.Count == 0;
            public int Level => IsRoot ? 0 : Parent.Level + 1;
            private Tree(T data)
            {
                Children = new LinkedList<ITree<T>>();
                Data = data;
            }
            public static Tree<T> FromLookup(ILookup<T, T> lookup)
            {
                var rootData = lookup.Count == 1 ? lookup.First().Key : default(T);
                var root = new Tree<T>(rootData);
                root.LoadChildren(lookup);
                return root;
            }
            private void LoadChildren(ILookup<T, T> lookup)
            {
                foreach (var data in lookup[Data])
                {
                    var child = new Tree<T>(data) { Parent = this };
                    Children.Add(child);
                    child.LoadChildren(lookup);
                }
            }
        }

    }
}