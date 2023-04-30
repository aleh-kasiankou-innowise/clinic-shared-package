using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Shared.Services.FiltrationService.Attributes;
using Innowise.Clinic.Shared.Services.PredicateBuilder;

namespace Innowise.Clinic.Shared.Services.FiltrationService;

public class FilterResolver<T>
{
    private ConcurrentDictionary<string, Func<string, Expression<Func<T, bool>>>> FilterRegistry { get; } = new();

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

            var filterInstance = (EntityFilter<T>) Activator.CreateInstance(filter) ??
                                 throw new ApplicationException(
                                     $"Filter of type {filter.FullName} cannot be instantiated.");
            if (!FilterRegistry.TryAdd(filterKey, filterInstance.ToExpression))
            {
                throw new ApplicationException(
                    $"The filter keys must be unique. " +
                    $"The {filterKey} is already reserved by class {FilterRegistry[filterKey].Target.GetType().FullName}");
            }
        }
    }

    public Expression<Func<T, bool>> ConvertCompoundFilterToExpression(ICompoundFilter<T> compoundFilter)
    {
        var filtrationExpressions = new List<Expression<Func<T, bool>>>();
        foreach (var filter in compoundFilter.Filters)
        {
            if (FilterRegistry.TryGetValue(filter.Key, out var filterExpressionDelegate))
            {
                filtrationExpressions.Add(filterExpressionDelegate(filter.Value));
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
}