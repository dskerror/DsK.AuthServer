namespace BlazorWASMCustomAuth.Database
{
	public class DbResult
	{
		public DbResult(object result)
		{
			HasError = false;
			Exception = null;
			this.Result = result;
		}

		public DbResult(object result, Exception ex)
		{
			HasError = true;
			Exception = ex;
			this.Result = result;
		}
		public object Result { get; set; }

		public bool HasError { get; set; }

		public Exception? Exception { get; set; }
	}
}
