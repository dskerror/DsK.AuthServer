namespace BlazorWASMCustomAuth.PagingSortingFiltering
{
    public class PagingSortingFilteringRequest
    {
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public string OrderBy { get; set; } = "";
        public string FilterByKey { get; set; } = "";
        public string FilterByValue { get; set; } = "";

        public object[] SQLParameters()
        {
            List<string> args = new List<string>();

            if (!string.IsNullOrEmpty(FilterByKey) && !string.IsNullOrEmpty(FilterByValue))
            {
                args.Add("@" + FilterByKey);
                args.Add(FilterByValue);
            }

            if (!string.IsNullOrEmpty(OrderBy))
            {
                args.Add("@OrderBy");
                args.Add(OrderBy);
            }

            args.Add("@PageSize");
            args.Add(PageSize.ToString());
            args.Add("@CurrentPage");
            args.Add(CurrentPage.ToString());


            return args.ToArray();
        }
    }
}
