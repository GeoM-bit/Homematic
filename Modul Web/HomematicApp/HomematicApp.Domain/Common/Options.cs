namespace HomematicApp.Domain.Common
{
	public class Options
	{
		public TimeOnly Time { get; set; }
		public string Value { get; set; }

        public Options(TimeOnly time, string value)
        {
            Time = time;
            Value = value;  
        }
    }
}
