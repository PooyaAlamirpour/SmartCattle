using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Helper
{
    class DevHelper
    {
        public class WhereClause
        {
            public String TABLE_NAME { get; set; }
            public String _where { get; set; }
            public String _update { get; set; }
            public String _insert { get; set; }
            public String _include = "";
            public String _limit { get; set; }
            public String _offset { get; set; }
            public String _orderby { get; set; }
            public String _Action = "Select ";
        }
        public static object _CurrentValueExp { get; set; }
        public static Type _CurrentTypeExp { get; set; }

        public static String Where<T>(Expression<Func<T, bool>> expr)
        {
            String _where = "";
            try
            {
                var sanitized = (Expression<Func<T, bool>>)new Literalizer().Visit(expr);
                var paramName = expr.Parameters[0].Name;            //x
                var paramTypeName = expr.Parameters[0].Type.Name;   //X_Model

                String sanitizedStr = sanitized.ToString()
                    .Replace(paramName + "=>", "")
                    .Replace(paramName + " =>", "")
                    .Replace(paramName + ".", "")
                    .Replace(paramTypeName + ".", "")
                    .Replace("==", "=")
                    .Replace("AndAlso", " && ")
                    .Replace("TrueAnd", "")
                    .Replace("\"", "'")
                    .Replace("(", "")
                    .Replace(")", "");

                var CurrentValueExp = _CurrentValueExp;
                var CurrentTypeExp = _CurrentTypeExp;
                var NodeType = sanitized.Body.NodeType;
                if (CurrentTypeExp != null)
                {
                    switch (CurrentTypeExp.Name)
                    {
                        case "DateTime":
                            char delimeter = getDelimeter(NodeType.ToString());
                            sanitizedStr = sanitizedStr.Split(delimeter)[0] + delimeter.ToString() + "'" + CurrentValueExp + "'";
                            break;
                    }
                }

                if (sanitizedStr.Equals("True"))
                {
                    _where = " ";
                }
                else
                {
                    _where = " " + sanitizedStr + " And ";
                }
            }
            catch (Exception ex)
            {
                String exp = ex.Message;
            }
            return _where;
        }

        private static char getDelimeter(String nodeType)
        {
            char retValue = '=';
            switch (nodeType)
            {
                case "GreaterThan":
                    retValue = '>';
                    break;

                case "Equal":
                    retValue = '=';
                    break;
            }

            return retValue;
        }

        public static void setExpression(object CurrentValueExp, Type CurrentTypeExp)
        {
            _CurrentValueExp = CurrentValueExp;
            _CurrentTypeExp = CurrentTypeExp;
        }

        public static string Update<T>(Expression<Func<T, bool>> expr)
        {
            String _update = "";
            var sanitized = (Expression<Func<T, bool>>)new Literalizer().Visit(expr);
            var paramName = expr.Parameters[0].Name;            //x
            var paramTypeName = expr.Parameters[0].Type.Name;   //X_Model
            String sanitizedStr = sanitized.ToString().Replace(" ", "").Replace(paramName + "=>", "").Replace(paramName + ".", "").Replace(paramTypeName + ".", "").Replace("==", "=").Replace("AndAlso", " && ").Replace("TrueAnd", "").Replace("\"", "'");
            _update += sanitizedStr + ", ";
            _update = _update.Replace("(", "").Replace(")", "");
            return _update;
        }

        public static string Insert<T>(Expression<Func<T, bool>> expr)
        {
            String _insert = "";
            var sanitized = (Expression<Func<T, bool>>)new Literalizer().Visit(expr);
            var paramName = expr.Parameters[0].Name;            //x
            var paramTypeName = expr.Parameters[0].Type.Name;   //X_Model
            String sanitizedStr = sanitized.ToString()
                .Replace(paramName + "=>", "")
                .Replace(paramName + " =>", "")
                .Replace(paramName + ".", "")
                .Replace(paramTypeName + ".", "")
                .Replace("==", "=")
                .Replace("AndAlso", " && ")
                .Replace("TrueAnd", "")
                .Replace("\"", "'");
            _insert += sanitizedStr + ", ";
            _insert = _insert.Replace("(", "").Replace(")", "").Replace("Convert", "");
            switch(_CurrentTypeExp.Name)
            {
                case "String":
                    _insert = _insert.Replace("= '", "= N'");
                    break;

                case "string":
                    _insert = _insert.Replace("= '", "= N'");
                    break;
            }
            return _insert;
        }

        public static string Include<T>(Expression<Func<T, bool>> expr)
        {
            String _include = "";
            //////////////////////////////////////////////////
            Expression<Func<T, bool>> exp = expr;
            string expBody = ((LambdaExpression)exp).Body.ToString();
            var paramName = exp.Parameters[0].Name;
            var paramTypeName = exp.Parameters[0].Type.Name;
            expBody = expBody.Replace(paramName + ".", paramTypeName + ".").Replace("AndAlso", "&&");
            //PropertyName<Country>(x => x.CountryName)
            //////////////////////////////////////////////////
            String strExpr = expr.Body.ToString().Split('.')[1];
            _include += "[" + strExpr.Substring(0, strExpr.Length - 1) + "],";
            return _include;
        }

        public static String Ascending<T>(Expression<Func<T, object>> expr)
        {
            String _orderby = "";
            Expression<Func<T, object>> sanitized = (Expression<Func<T, object>>)new Literalizer().Visit(expr);
            String paramName = expr.Parameters[0].Name;            //x
            String paramTypeName = expr.Parameters[0].Type.Name;   //X_Model
            String sanitizedStr = sanitized.ToString().Replace(" ", "").Replace(paramName + "=>", "").Replace(paramName + ".", "").Replace(paramTypeName + ".", "").Replace("==", "=").Replace("AndAlso", " && ").Replace("TrueAnd", "").Replace("\"", "'");
            _orderby = sanitizedStr.Replace("Convert", "");
            _orderby = " " + _orderby.Replace("(", "").Replace(")", "") + " Asc ";
            return _orderby;
        }

        public static string Descending<T>(Expression<Func<T, object>> expr)
        {
            String _orderby = "";
            var sanitized = (Expression<Func<T, object>>)new Literalizer().Visit(expr);
            var paramName = expr.Parameters[0].Name;            //x
            var paramTypeName = expr.Parameters[0].Type.Name;   //X_Model
            String sanitizedStr = sanitized.ToString().Replace(" ", "").Replace(paramName + "=>", "").Replace(paramName + ".", "").Replace(paramTypeName + ".", "").Replace("==", "=").Replace("AndAlso", " && ").Replace("TrueAnd", "").Replace("\"", "'");
            _orderby = sanitizedStr.Replace("Convert", "");
            _orderby = " order by " + _orderby.Replace("(", "").Replace(")", "") + " Desc ";
            return _orderby;
        }

        public static string ExpressionBuilder(WhereClause _whereClause)
        {
            String expression = "";
            if (_whereClause._include == "" || _whereClause._include.Replace(" ", "") == "*")
            {
                _whereClause._include = " * ";
            }
            else
            {
                _whereClause._include = _whereClause._include.Substring(0, _whereClause._include.Length - 1);
            }
            if (_whereClause._Action.Equals("Delete "))
            {
                expression = _whereClause._Action + " FROM " + _whereClause.TABLE_NAME + _whereClause._where;
            }
            else if (_whereClause._Action.Equals("Select "))
            {
                if (_whereClause._where != null)
                {
                    expression = _whereClause._Action + _whereClause._include + " FROM " + _whereClause.TABLE_NAME + " Where " + _whereClause._where + _whereClause._offset + _whereClause._limit + _whereClause._orderby;
                }
                else
                {
                    expression = _whereClause._Action + _whereClause._include + " FROM " + _whereClause.TABLE_NAME + _whereClause._offset + _whereClause._limit + _whereClause._orderby;
                }
            }
            else if (_whereClause._Action.Equals("Update "))
            {
                expression = _whereClause._Action + _whereClause.TABLE_NAME + " SET " + _whereClause._update.Substring(0, _whereClause._update.Length - 2) + " where " + _whereClause._where;
            }
            else if (_whereClause._Action.Equals("Insert "))
            {
                String[] splitedPart = _whereClause._insert.Split(',');
                String tmpColumn = " (";
                String tmpValue = " (";
                for (int i = 0; i < splitedPart.Length - 1; i++)
                {
                    String name = splitedPart[i].Split('=')[0];
                    String Value = splitedPart[i].Split('=')[1];
                    tmpColumn += name + ", ";
                    tmpValue += Value + ", ";
                }
                tmpColumn = tmpColumn.Substring(0, tmpColumn.Length - 2) + ")";
                tmpValue = tmpValue.Substring(0, tmpValue.Length - 2) + ")";
                expression = _whereClause._Action + " INTO " + _whereClause.TABLE_NAME + tmpColumn + " VALUES " + tmpValue;
            }
            String tmp = "";
            if (expression.Length >= 4)
            {
                tmp = expression.Substring(expression.Length - 4).ToLower();
            }
            if (tmp.Contains("and "))
            {
                expression = expression.Remove(expression.Length - 4);
            }

            expression = expression.Replace("&&", "And");
            return expression;
        }

        public static String Where<T>(List<XModels.RelationTbl> Rels, WhereClause _whereClause, T Parent)
        {
            String NewWhere = "";
            for (int j = 0; j < Rels.Count; j++)
            {
                String tmp_where = "";
                object _value;
                _value = Parent.GetType().GetProperty(Rels[j].Ref_KeyCol).GetGetMethod().Invoke(Parent, null);
                if (_value != null)
                {
                    if (_value.GetType() == typeof(int))
                    {
                        tmp_where = " " + Rels[j].FKey_Col + "=" + _value + " And ";
                    }
                    else
                    {
                        tmp_where = " " + Rels[j].FKey_Col + "='" + _value + "' And ";
                    }

                    NewWhere = _whereClause._where + " " + tmp_where;
                    String tmp = "";
                    if (NewWhere.Length >= 4)
                    {
                        tmp = NewWhere.Substring(NewWhere.Length - 4).ToLower();
                    }
                    if (tmp.Contains("and "))
                    {
                        NewWhere = NewWhere.Remove(NewWhere.Length - 4);
                    }
                }
                else
                {
                    NewWhere = "NOT_REL";
                }



            }
            return NewWhere;
        }

        internal static string SetListener<T>(Expression<Func<T, object>> expr)
        {
            String retValue = "";
            try
            {
                var sanitized = (Expression<Func<T, object>>)new Literalizer().Visit(expr);
                var paramName = expr.Parameters[0].Name;            //x
                var paramTypeName = expr.Parameters[0].Type.Name;   //X_Model
                String sanitizedStr = sanitized.ToString()
                    .Replace(" ", "")
                    .Replace(paramName + "=>", "")
                    .Replace(paramName + ".", "")
                    .Replace(paramTypeName + ".", "")
                    .Replace("==", "=")
                    .Replace("AndAlso", " && ")
                    .Replace("TrueAnd", "")
                    .Replace("\"", "'")
                    .Replace("Convert", "")
                    .Replace("(", "")
                    .Replace(")", "");
                retValue = sanitizedStr;
            }
            catch (Exception ex)
            {

            }
            return retValue;
        }
    }
}
