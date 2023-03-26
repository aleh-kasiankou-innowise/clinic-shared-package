namespace Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;

public class CompoundFilter<T> : ICompoundFilter<T>
{
    public IEnumerable<KeyValuePair<string, string>> Filters { get; set; }
}