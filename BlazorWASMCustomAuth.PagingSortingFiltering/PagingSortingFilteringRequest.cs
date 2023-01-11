namespace BlazorWASMCustomAuth.PagingSortingFiltering
{ 
	public class PagingSortingFilteringRequest
	{
		public int PageSize { get; set; } = 10;
		public int CurrentPage { get; set; } = 1;
	}
}
