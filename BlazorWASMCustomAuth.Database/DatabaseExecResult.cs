namespace BlazorWASMCustomAuth.Database
{
	public class DatabaseExecResult
	{
		public DatabaseExecResult(object result)
		{
			HasError = false;
			Message = "";
			Exception = null;
			this.Result = result;
		}

		public DatabaseExecResult(object result, Exception ex)
		{
			HasError = true;
			Message = "";
			Exception = ex;
			this.Result = result;
		}
		public object Result { get; set; }

		public string Message { get; set; }

		public bool HasError { get; set; }

		public Exception Exception { get; set; }
	}
}
