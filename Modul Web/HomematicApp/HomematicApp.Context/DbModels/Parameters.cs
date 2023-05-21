
using System.ComponentModel.DataAnnotations;

namespace HomematicApp.Context.DbModels
{
	public class Parameters
	{
		[Key]
		public int row_id { get; set; }
		public float temperature { get; set; }
		public float light_intensity { get; set; }
		public bool opened_door { get; set; }

	}
}
