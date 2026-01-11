using System;
using System.Collections.Generic;

namespace ApplicantTracking.Application.DTOs;

public sealed class PagedList<T>
{
    public PagedList(IReadOnlyList<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
        TotalCount = totalCount < 0 ? throw new ArgumentOutOfRangeException(nameof(totalCount)) : totalCount;
        CurrentPage = currentPage <= 0 ? throw new ArgumentOutOfRangeException(nameof(currentPage)) : currentPage;
        PageSize = pageSize <= 0 ? throw new ArgumentOutOfRangeException(nameof(pageSize)) : pageSize;

        TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
        HasPreviousPage = CurrentPage > 1;
        HasNextPage = CurrentPage < TotalPages;
    }

    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int PageSize { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }
}

