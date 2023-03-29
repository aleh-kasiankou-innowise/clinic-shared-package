using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FastExpressionCompiler;
using Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Shared.Services.FiltrationService.Attributes;
using Innowise.Clinic.Shared.Services.PredicateBuilder;

namespace Innowise.Clinic.Shared.Services.FiltrationService;

public class FilterResolver<T>
{
    private ConcurrentDictionary<string, Type> FilterRegistry { get; } = new();
    
    public FilterResolver()
    {
        var filterTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => x.IsSubclassOf(typeof(EntityFilter<T>)) &&
                        x.GetCustomAttribute<DisabledFilterAttribute>() is null);

        foreach (var filter in filterTypes)
        {
            Console.WriteLine($"Registering new filter: {filter.FullName}");
            var filterKey = filter.GetCustomAttribute<FilterKeyAttribute>()?.FilterKey ??
                            throw new ApplicationException($"The filter must have a filter key: {filter.FullName}");

            if (!FilterRegistry.TryAdd(filterKey, filter))
            {
                throw new ApplicationException(
                    $"The filter keys must be unique. " +
                    $"The {filterKey} is already reserved by class {FilterRegistry[filterKey].FullName}");
            }
        }
    }

    public Expression<Func<T, bool>> ConvertCompoundFilterToExpression(ICompoundFilter<T> compoundFilter)
    {
        var filtrationExpressions = new List<Expression<Func<T, bool>>>();
        foreach (var filter in compoundFilter.Filters)
        {
            if (FilterRegistry.TryGetValue(filter.Key, out var filterType))
            {
                var expression = BuildExpression(filterType)(filter.Value);
                Console.WriteLine(expression);
                filtrationExpressions.Add(expression);
            }
            else
            {
                Console.WriteLine($"The filter {filter.Key} is not available.");
                foreach (var availableFilter in FilterRegistry)
                {
                    Console.WriteLine("Available filters:");
                    Console.WriteLine(availableFilter.Key);
                }
            }
        }

        if (!filtrationExpressions.Any())
        {
            return x => true;
        }

        var filtrationExpression = filtrationExpressions.Count > 1
            ? filtrationExpressions.Aggregate((first, second) => first
                .And(second))
            : filtrationExpressions[0];
        return filtrationExpression;
    }

    private Func<string, Expression<Func<T, bool>>> BuildExpression(Type filterType)
    {
        // TODO CONDUCT PERFORMANCE BENCHMARK

        var filterValueParam = Expression.Parameter(typeof(string), "filterValue");
        var filterInstance = Expression.New(filterType.GetConstructor(new[] { typeof(string) }), filterValueParam);
        var lambda = Expression.Lambda<Func<string, Expression<Func<T, bool>>>>(
            GetMethodInfo(filterInstance, "ToExpression", Type.EmptyTypes), new[] { filterValueParam }
        );
        return lambda.CompileFast();
    }

    private MethodCallExpression GetMethodInfo(NewExpression instance, string methodName, Type[] types)
    {
        return Expression.Call(instance, methodName, types);
    }
}