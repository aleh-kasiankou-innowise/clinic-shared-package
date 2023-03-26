using System.Linq.Expressions;
using System.Text;
using Innowise.Clinic.Shared.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;

[FilterKey("base")]
public abstract class EntityFilter<T> : IEntityFilter<T>
{
    protected EntityFilter(string value)
    {
        Value = value;
    }

    public string Value { get; }
    public abstract Expression<Func<T, bool>> ToExpression();
    public abstract (StringBuilder, object) ToSql();
}