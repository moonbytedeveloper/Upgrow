using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace VerifyIndia.Domain.Common;

public class PaginationParams
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 500;
    private int _pageNumber = 1;
    private int _pageSize = DefaultPageSize;

    [JsonIgnore]
    [BindNever]
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    [JsonIgnore]
    [BindNever]
    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value <= 0)
            {
                _pageSize = DefaultPageSize;
                return;
            }

            _pageSize = Math.Min(value, MaxPageSize);
        }
    }

    [JsonIgnore]
    [BindNever]
    public int Skip => (PageNumber - 1) * PageSize;
}
