// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure
{
    public class PageViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (string.IsNullOrEmpty(context.PageName))
            {
                // Not a page - just act natural.
                return viewLocations;
            }

            return ExpandPageHierarchy();

            IEnumerable<string> ExpandPageHierarchy()
            {
                foreach (var location in viewLocations)
                {
                    if (!location.Contains("{1}/"))
                    {
                        // If the location doesn't have the 'page' replacement token just return it as-is.
                        yield return location;
                    }

                    // For locations with the 'page' token - expand them into an ascending directory search,
                    // but only up to the pages root.
                    //
                    // This is easy because the 'page' token already trims the root directory.
                    var end = context.PageName.Length;

                    while (end > 0 && (end = context.PageName.LastIndexOf('/', end - 1)) != -1)
                    {
                        // PageName always starts with `/`
                        yield return location.Replace("{1}/", context.PageName.Substring(1, end));
                    }
                }
            }
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // The value we care about - 'page' is already part of the system. We don't need to add it manually.
        }
    }
}
