using System.ComponentModel.DataAnnotations;

namespace HomematicApp.Context.DbModels
{
	public class EspData
	{
		[Key]
		public int Id { get; set; }
		public float Temperature_ESP { get; set; }
		public DateTime Timestamp_Value { get; set; }
	}
}
