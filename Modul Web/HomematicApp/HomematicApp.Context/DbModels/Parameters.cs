
using System.ComponentModel.DataAnnotations;

namespace HomematicApp.Context.DbModels
{
	public class Parameters
	{
		[Key]
		public int Row_id { get; set; }
		public float Temperature { get; set; }
		public float Light_intensity { get; set; }
		public bool Opened_door { get; set; }

	}
}
