using System.Linq.Expressions;
using Innowise.Clinic.Shared.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;

[FilterKey("base")]
public abstract class EntityFilter<T> : IEntityFilter<T>
{
    public abstract Expression<Func<T, bool>> ToExpression(string value);
    
}