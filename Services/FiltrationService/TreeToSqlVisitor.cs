using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FastExpressionCompiler;
using Innowise.Clinic.Shared.Exceptions;
using Innowise.Clinic.Shared.Services.SqlMappingService;
using Microsoft.Extensions.Logging;

namespace Innowise.Clinic.Shared.Services.FiltrationService;

public class TreeToSqlVisitor
{
    private readonly StringBuilder _textRepresentation = new();
    private readonly ISqlMapper _sqlMapper;
    private readonly ILogger<TreeToSqlVisitor> _logger;
    private ParameterWrapping _nextParameterWrapping = ParameterWrapping.Default;

    private static readonly MethodInfo ContainsMethodInfo =
        typeof(string).GetMethods().First(x => x.Name == "Contains" && x.GetParameters().Length == 1);

    public TreeToSqlVisitor(ISqlMapper sqlMapper, ILogger<TreeToSqlVisitor> logger)
    {
        _sqlMapper = sqlMapper;
        _logger = logger;
    }

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
            return VisitMemberExpression(memberExpression, entityType, parameters);
        }

        if (node is MethodCallExpression methodCallExpression)
        {
            return VisitMethodCall(methodCallExpression, entityType, parameters);
        }

        throw new NotSupportedException(node.NodeType.ToString());
    }

    private StringBuilder VisitBinaryNode(BinaryExpression expression, Type entityType,
        Dictionary<string, object> parameters)
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

        else if (expression.NodeType == ExpressionType.Equal)
        {
            conjunction = " = ";
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


        return new StringBuilder().Append('(').Append(Visit(expression.Left, entityType, parameters))
            .Append(conjunction)
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
                    throw new NotSupportedException("Such parametrization scheme is not yet supported: " +
                                                    _nextParameterWrapping);
            }

            var paramName = $"@p{parameters.Count}";
            sql.Append(paramName);
            parameters.Add(paramName, SqlParamValue);
        }

        _nextParameterWrapping = ParameterWrapping.Default;
        return sql;
    }

    private StringBuilder VisitMethodCall(MethodCallExpression expression, Type entityType,
        Dictionary<string, object> parameters)
    {
        var sql = new StringBuilder();
        if (expression.Method == ContainsMethodInfo)
        {
            _nextParameterWrapping = ParameterWrapping.Contains;
            var property = VisitMemberExpression((MemberExpression)expression.Object, entityType, parameters);
            var sqlParam = Visit(expression.Arguments[0], entityType, parameters);
            sql.Append($"{property} LIKE ").Append(sqlParam);
            return sql;
        }

        throw new NotSupportedException(expression.Method.ToString());
    }

    private StringBuilder VisitMemberExpression(MemberExpression expression, Type entityType,
        Dictionary<string, object> parameters, bool isNested = false)
    {
        var sql = new StringBuilder();

        // TODO FIND A WAY TO AVOID COMPILATION
        if (expression.Member.MemberType == MemberTypes.Field)
        {
            var fieldAccessValue = Expression.Convert(expression, typeof(object));
            var valueGetterLambda = Expression.Lambda<Func<object>>(fieldAccessValue);
            var valueGetterDelegate = valueGetterLambda.CompileFast();
            var paramName = $"@p{parameters.Count}";
            var paramValue = valueGetterDelegate();
            sql.Append(paramName);
            parameters.Add(paramName, paramValue);
        }

        else
        {
            try
            {
                if (expression.Expression is MemberExpression convertedMemberExpression && !isNested)
                {
                    var childEntityType = ((PropertyInfo)convertedMemberExpression.Member).PropertyType;
                    var nestedAccessor = Expression.Property(convertedMemberExpression,
                        childEntityType?.GetProperty(expression.Member.Name));
                    return VisitMemberExpression(nestedAccessor, childEntityType, parameters, true);
                }

                var propertyInfo = entityType.GetProperty(expression.Member.Name) ??
                                   throw new SqlMappingException(
                                       $"Type {entityType.FullName} doesn't have property {expression.Member.Name}");
                sql.Append($"\"{_sqlMapper.GetSqlTableName(entityType)}\".")
                    .Append($"\"{_sqlMapper.GetSqlPropertyName(entityType, propertyInfo)}\"");
            }
            catch
            {
                _logger.LogWarning(
                    "Cannot visit member expression. Main type: {MainType}, Property {PropertyName}",
                    entityType, expression.Member.Name);
                throw;
            }
        }

        return sql;
    }
}