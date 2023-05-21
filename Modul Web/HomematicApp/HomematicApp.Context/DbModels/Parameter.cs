using System.ComponentModel.DataAnnotations;

namespace HomematicApp.Context.DbModels
{
	public class Parameter
	{
		[Key]
		public int Row_Id { get; set; }
		public float Temperature { get; set; }
		public float Light_Intensity { get; set; }
		public bool Opened_Door { get; set; }
	}
}
