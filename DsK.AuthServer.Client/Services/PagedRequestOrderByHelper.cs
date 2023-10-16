using MudBlazor;
using System.Runtime.CompilerServices;

namespace DsK.AuthServer.Client.Services
{
    public static class PagedRequestOrderByHelper
    {
        public static string ToPagedRequestString(this TableState state)
        {
            string[] orderings = null;
            string orderby = "";
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            if (orderings?.Any() == true)
            {
                foreach (var orderByPart in orderings)
                {
                    orderby += $"{orderByPart},";
                }
                orderby = orderby[..^1]; // loose training ,
            }

            return orderby;
        }
    }
}
