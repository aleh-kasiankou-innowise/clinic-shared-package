using System.Linq.Expressions;

namespace Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;

public interface IEntityFilter<T>
{
    public Expression<Func<T, bool>> ToExpression(string value);
}