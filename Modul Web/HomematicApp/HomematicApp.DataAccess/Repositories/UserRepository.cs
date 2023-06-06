using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.Domain.Common;
using HomematicApp.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Action = HomematicApp.Context.DbModels.Action;


namespace HomematicApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HomematicContext _context;
        public UserRepository(HomematicContext context)
        {
            _context = context;
        }

        public async Task<List<Action>> getActions(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser == null) return null;
            return await _context.Actions.Where(a => a.Device_Id == dbUser.Device_Id).ToListAsync();
        }

        public async Task<Parameter> getParameters()
        {
            var result = await _context.Parameters.ToListAsync();
            return result[0];
        }

        public async Task<List<PresetModelDTO>> getPresetList(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser == null) return null;
            var dbList = await _context.Presets.Where(p => p.Device_Id == null || p.Device_Id == dbUser.Device_Id).ToListAsync();
            List<PresetModelDTO> decodedList = new List<PresetModelDTO>();
            if (dbList != null)
            {
                foreach (var preset in dbList)
                {
                    var decodedPreset = decodePreset(preset);
                    decodedList.Add(decodedPreset);
                }
            }
            return decodedList;
        }

        public PresetModelDTO decodePreset(Preset preset)
        {
            var result = new PresetModelDTO();

            result.Preset_Id = preset.Preset_Id;
            result.Preset_Name = preset.Preset_Name;
            result.Device_Id = preset.Device_Id;
            result.Start_Date = preset.Start_Date;
            result.End_Date = preset.End_Date;
            result.Temperature_Options = new List<Options>();
            result.Light_Options = new List<Options>();

            string[] splits = preset.Option_Code.Split('.');

            for (int i = 1; i <= int.Parse(splits[0]); i++)
            {
                TimeOnly time = new TimeOnly(int.Parse(splits[i].Substring(0, 2)), int.Parse(splits[i].Substring(2, 2)));
                string tempValue = splits[i].Substring(4, 2);
                string lightValue = splits[i].Substring(6, 3);
                if (tempValue != "XX")
                {
                    if (tempValue.StartsWith("0"))
                    {
                        tempValue = tempValue.Substring(1, 1);

                    }
                    Options tempOptions = new Options(time, tempValue);
                    result.Temperature_Options.Add(tempOptions);
                }
                if (lightValue != "XXX")
                {
                    if (lightValue.StartsWith("0"))
                    {
                        lightValue = lightValue.Substring(1, 2);
                        if (lightValue.StartsWith("0"))
                        {
                            lightValue = lightValue.Substring(1, 1);
                        }
                    }
                    Options lightOptions = new Options(time, lightValue);
                    result.Light_Options.Add(lightOptions);
                }
            }

            return result;
        }
        public async Task<bool> modifyParameters(Parameter parameters, string email)
		{
			var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser != null)
            {
                var actions = await _context.Actions.ToListAsync();
                Action temp = new Action(dbUser.Device_Id, ActionType.temperature, parameters.Temperature, DateTime.Now);            
                Action light = new Action( dbUser.Device_Id, ActionType.light, parameters.Light_Intensity, DateTime.Now);              
                Action door = new Action( dbUser.Device_Id, ActionType.door, parameters.Opened_Door ? 1 : 0, DateTime.Now);
				_context.Actions.Add(temp);
				_context.Actions.Add(light);
				_context.Actions.Add(door);
            }
            else return false;
		    parameters.Row_Id = 1;
		    _context.Parameters.Update(parameters);

			return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<object>?> getChartData(string email, ActionType actionType)
        {
			List<object> data = new();
			var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser != null && actionType!=0)          
            {

                List<string> labels = new();
                List<float> values = new();
				var result = await _context.Actions.Where(a => a.Device_Id == dbUser.Device_Id && a.Action_Type == actionType && a.Date_Time.Date == DateTime.Parse("2023-05-23").Date).ToListAsync();
                if (result != null)
                {
                    labels = result.OrderBy(a => a.Date_Time).Select(x => x.Date_Time.ToString("HH:mm")).ToList();
                    values = result.OrderBy(a => a.Date_Time).Select(x => x.Value_Action).ToList();

                    data.Add(labels);
                    data.Add(values);

                    return data;
                }
            }
            return null;		
        }
    }
}
