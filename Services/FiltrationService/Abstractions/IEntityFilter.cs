using System.Linq.Expressions;
using System.Text;

namespace Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;

public interface IEntityFilter<T>
{
    public string Value { get; }
    public Expression<Func<T, bool>> ToExpression();

    public (StringBuilder, object) ToSql();
}