using System.ComponentModel;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.DTOs;

public class DataTableRequest : PaginationParams
{
    private int _start;

    [DefaultValue(0)]
    public int Start
    {
        get => _start;
        set
        {
            _start = value < 0 ? 0 : value;
            RecalculatePageNumber();
        }
    }
    [DefaultValue(10)]
    public int Length
    {
        get => PageSize;
        set
        {
            PageSize = value;
            RecalculatePageNumber();
        }
    }

    public string? Search { get; set; }
    public string? SortColumn { get; set; }
    public string? SortDirection { get; set; }

    private void RecalculatePageNumber()
    {
        PageNumber = Length > 0 ? (_start / Length) + 1 : 1;
    }
}