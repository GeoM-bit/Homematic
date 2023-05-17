using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomematicApp.Context.DbModels
{
    public class Action
    {
        [Key]
        public int Action_Id { get; set; }
        [ForeignKey("Device_Id")]
        public string Device_Id { get; set; }
        public string Action_Type { get; set; }
        public float Value_Action { get; set; }
        public DateTime Date_Time { get; set; }

    }
}
