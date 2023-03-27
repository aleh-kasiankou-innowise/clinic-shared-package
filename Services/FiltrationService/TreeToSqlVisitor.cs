using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PostgresSQL.ADONET;

public class TreeToSqlVisitor
{
    private readonly StringBuilder _textRepresentation = new();
    private ParameterWrapping _nextParameterWrapping = ParameterWrapping.Default;

    private static MethodInfo _containsMethodInfo =
        typeof(string).GetMethods().First(x => x.Name == "Contains" && x.GetParameters().Length == 1);

    public StringBuilder Visit(Expression? node, Type entityType, Dictionary<string, object> parameters)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (node.NodeType == ExpressionType.Lambda)
        {
            var lambda = (LambdaExpression)node;
            _textRepresentation.Clear();
            _textRepresentation.Append(Visit(lambda.Body, entityType, parameters));
            return _textRepresentation;
        }

        var isLeaf = node is ParameterExpression ||
                     node is ConstantExpression;
        if (isLeaf)
        {
            return VisitLeaf(node, entityType, parameters);
        }

        if (node is BinaryExpression binaryExpression)
        {
            return VisitBinaryNode(binaryExpression, entityType, parameters);
        }

        if (node is MemberExpression memberExpression)
        {
            return VisitMemberExpression(memberExpression, entityType);
        }

        if (node is MethodCallExpression methodCallExpression)
        {
            return VisitMethodCall(methodCallExpression, entityType, parameters);
        }

        throw new NotSupportedException(node.NodeType.ToString());
    }

    private StringBuilder VisitBinaryNode(BinaryExpression expression, Type entityType, Dictionary<string, object> parameters)
    {
        string conjunction = "";
        if (expression.NodeType == ExpressionType.AndAlso)
        {
            conjunction = " AND ";
        }
        else if (expression.NodeType == ExpressionType.OrElse)
        {
            conjunction = " OR ";
        }

        else if (expression.NodeType == ExpressionType.GreaterThan)
        {
            conjunction = " > ";
        }

        else if (expression.NodeType == ExpressionType.GreaterThanOrEqual)
        {
            conjunction = " >= ";
        }

        else if (expression.NodeType == ExpressionType.LessThan)
        {
            conjunction = " < ";
        }

        else if (expression.NodeType == ExpressionType.LessThanOrEqual)
        {
            conjunction = " <= ";
        }

        else
        {
            throw new NotSupportedException(expression.NodeType.ToString());
        }

        
        return new StringBuilder().Append('(').Append(Visit(expression.Left, entityType, parameters)).Append(conjunction)
            .Append(Visit(expression.Right, entityType, parameters).Append(')'));
    }

    private StringBuilder VisitLeaf(Expression expression, Type entityType, Dictionary<string, object> parameters)
    {
        var sql = new StringBuilder();

        if (expression.NodeType == ExpressionType.Parameter)
        {
            sql.Append(entityType);
        }
        else
        {

            object SqlParamValue;

            switch (_nextParameterWrapping)
            {
                case ParameterWrapping.Default:
                    SqlParamValue = ((ConstantExpression)expression).Value;
                    break;
                case ParameterWrapping.Contains:
                    SqlParamValue = $"%{((ConstantExpression)expression).Value}%";
                    break;
                default:
                    throw new NotSupportedException("Such parametrization scheme is not yet supported: " + _nextParameterWrapping.ToString());
            }
            
            var paramName = $"@p{parameters.Count}";
            sql.Append(paramName);
            parameters.Add(paramName, SqlParamValue);
        }

        _nextParameterWrapping = ParameterWrapping.Default;
        return sql;
    }

    private StringBuilder VisitMethodCall(MethodCallExpression expression, Type entityType, Dictionary<string, object> parameters)
    {
        var sql = new StringBuilder();
        if (expression.Method == _containsMethodInfo)
        {
            _nextParameterWrapping = ParameterWrapping.Contains;
            var property = VisitMemberExpression((MemberExpression)expression.Object, entityType);
            var sqlParam = Visit(expression.Arguments[0], entityType, parameters);
            sql.Append($"{property} LIKE ").Append(sqlParam);
            return sql;
        }

        throw new NotSupportedException(expression.Method.ToString());
    }

    private StringBuilder VisitMemberExpression(MemberExpression expression, Type entityType)
    {
        var sql = new StringBuilder();
        var propertyInfo = entityType.GetProperty(expression.Member.Name);
        sql.Append($"{entityType.GetCustomAttribute<TableAttribute>().Name}.").Append(propertyInfo.GetCustomAttribute<ColumnAttribute>().Name);
        return sql;
    }
}