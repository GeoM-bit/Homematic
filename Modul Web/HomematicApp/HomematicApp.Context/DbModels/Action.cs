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
        public ActionType Action_Type { get; set; }
        public float Value_Action { get; set; }
        public DateTime Date_Time { get; set; }

        public Action(string Device_Id, ActionType Action_Type, float Value_Action, DateTime dateTime)
        {
            this.Device_Id = Device_Id;
            this.Action_Type = Action_Type;
            this.Value_Action = Value_Action;
            Date_Time = dateTime;
        }

        public Action()
        {
            
        }
    }
}
