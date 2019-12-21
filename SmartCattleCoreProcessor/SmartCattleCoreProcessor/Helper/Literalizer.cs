using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Helper
{
    public class Literalizer : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType.IsDefined(typeof(CompilerGeneratedAttribute), false)
                && node.Expression.NodeType == ExpressionType.Constant)
            {
                object target = ((ConstantExpression)node.Expression).Value, value;
                Type nodeType = node.Type;
                switch (node.Member.MemberType)
                {
                    case MemberTypes.Property:
                        value = ((PropertyInfo)node.Member).GetValue(target, null);
                        break;
                    case MemberTypes.Field:
                        value = ((FieldInfo)node.Member).GetValue(target);
                        break;
                    default:
                        value = target = null;
                        break;
                }
                if (target != null)
                {
                    DevHelper.setExpression(value, nodeType);
                    return Expression.Constant(value, nodeType);
                }
            }
            return base.VisitMember(node);
        }
    }
}
