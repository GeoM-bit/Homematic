using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
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
            var dbList = await _context.Presets.Where(p => p.Device_Id == "default" || p.Device_Id == dbUser.Device_Id).ToListAsync();
            List<PresetModelDTO> decodedList = new List<PresetModelDTO>();
            if (dbList != null)
            {
                foreach (var preset in dbList)
                {
                    if (preset.Option_Code != null)
                    {
                        var decodedPreset = decodePreset(preset);
                        decodedList.Add(decodedPreset);
                    }
                }
            }
            return decodedList;
        }

        private PresetModelDTO decodePreset(Preset preset)
        {
            var result = new PresetModelDTO();

            result.Preset_Name = preset.Preset_Name;
            result.Device_Id = preset.Device_Id;

            string[] splits = preset.Option_Code.Split('.');
            result.Temperature = int.Parse(splits[0]);
            result.Light = int.Parse(splits[1]);          

            return result;
        }
        public async Task<bool> modifyParameters(Parameter parameters, string email)
		{
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser != null)
            {
                var actions = await _context.Actions.ToListAsync();
                Action temp = new Action(dbUser.Device_Id, ActionType.temperature, parameters.Temperature, DateTime.Now);
                Action light = new Action(dbUser.Device_Id, ActionType.light, parameters.Light_Intensity, DateTime.Now);
                Action door = new Action(dbUser.Device_Id, ActionType.door, parameters.Opened_Door ? 1 : 0, DateTime.Now);
                _context.Actions.Add(temp);
                _context.Actions.Add(light);
                _context.Actions.Add(door);
            }
            else return false;
            parameters.Row_Id = 1;
            if (parameters.Current_Preset == null) 
                parameters.Current_Preset = "manual";
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
                if (actionType!=ActionType.esp_temperature)
                {
                    var result = await _context.Actions.Where(a => a.Device_Id == dbUser.Device_Id && a.Action_Type == actionType && a.Date_Time.Date >= DateTime.Now.AddDays(-1) && a.Date_Time.Date <= DateTime.Now).ToListAsync();
                    if (result != null)
                    {
                        labels = result.OrderBy(a => a.Date_Time).Select(x => x.Date_Time.ToString("HH:mm")).ToList();
                        values = result.OrderBy(a => a.Date_Time).Select(x => x.Value_Action).ToList();

                        data.Add(labels);
                        data.Add(values);

                        return data;
                    }
                }
                else{
					var result = await _context.Temperature_ESP.Where(a =>a.Timestamp_Value.Date >= DateTime.Now.AddDays(-1) && a.Timestamp_Value.Date <= DateTime.Now).Take(50).ToListAsync();
					if (result != null)
					{
						labels = result.OrderBy(a => a.Timestamp_Value).Select(x => x.Timestamp_Value.ToString("HH:mm")).ToList();
						values = result.OrderBy(a => a.Timestamp_Value).Select(x => x.Temperature_ESP).ToList();

						data.Add(labels);
						data.Add(values);

						return data;
					}
				}
            }
            return null;		
        }

        public async Task<bool> createPreset(PresetModelDTO presetDTO, string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser != null)
            {
                Preset preset = new Preset
                {
                    Device_Id = dbUser.Device_Id,
                    Preset_Name = presetDTO.Preset_Name,
                    Option_Code = presetDTO.Temperature.ToString() + "." + presetDTO.Light.ToString()
                };
                _context.Presets.Add(preset);             
            }
       		return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> getCurrentPreset()
        {
            var result = _context.Parameters.Select(x=>x.Current_Preset).ToList();

            return result[0];
        }

        public async Task<PresetModelDTO?> setPreset(string presetName, string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser != null)
            {
                var result = await _context.Presets.FirstOrDefaultAsync(p => p.Preset_Name == presetName && p.Device_Id==dbUser.Device_Id);
                if (result != null)
                {
                    return decodePreset(result);
                }
            }
            return null;
        }

		public async Task<List<EspData>> getEspData()
		{
			return await _context.Temperature_ESP.Where(a => a.Timestamp_Value.Date >= DateTime.Now.AddDays(-1) && a.Timestamp_Value.Date <= DateTime.Now).ToListAsync();
		}
	}
}
