﻿using Ellie.Modules.Searches.Youtube;

namespace Ellie.Modules.Searches;

public interface ISearchServiceFactory
{
    public ISearchService GetSearchService(string? hint = null);
    public ISearchService GetImageSearchService(string? hint = null);
    public IYoutubeSearchService GetYoutubeSearchService(string? hint = null);
}