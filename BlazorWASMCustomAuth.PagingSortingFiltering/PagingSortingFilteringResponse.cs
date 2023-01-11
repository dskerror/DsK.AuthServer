namespace BlazorWASMCustomAuth.PagingSortingFiltering
{
	public class PagingSortingFilteringResponse
    {
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int TotalRows { get; set; }
        public string? OrderBy { get; set; }
        public string? WhereFilter { get; set; }
        public int TotalPages => (int)Math.Ceiling((float)this.TotalRows / (float)this.PageSize);
        public int OffSet()
        {
            return (this.CurrentPage - 1) * this.PageSize;
        }
    }
}
