namespace Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;

public interface ICompoundFilter<T>
{
    IEnumerable<KeyValuePair<string, string>> Filters { get; set; }
}