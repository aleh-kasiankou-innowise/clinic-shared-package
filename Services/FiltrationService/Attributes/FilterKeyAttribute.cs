namespace Innowise.Clinic.Shared.Services.FiltrationService.Attributes;

public class FilterKeyAttribute : Attribute
{
    public string FilterKey { get; }

    public FilterKeyAttribute(string filterKey)
    {
        if (string.IsNullOrEmpty(filterKey))
        {
            throw new ApplicationException("The filter key cannot be null or empty");
        }
        
        else if (filterKey == "base")
        {
            throw new ApplicationException($"The filters cannot use keys of a base class : {filterKey}");
        }
        FilterKey = filterKey.ToLower();
    }
}