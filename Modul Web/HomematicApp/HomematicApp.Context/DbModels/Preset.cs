using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomematicApp.Context.DbModels
{
    public class Preset
    {
        [Key]
        public int Preset_Id { get; set; }
        public string Preset_Name { get; set; }
        [ForeignKey("Device_Id")]
        public string? Device_Id { get; set; }
        public string Option_Code { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }
}
