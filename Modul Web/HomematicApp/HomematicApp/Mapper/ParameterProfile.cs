﻿using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.ViewModels;

namespace HomematicApp.Mapper
{
	public class ParameterProfile:Profile
	{
		public ParameterProfile()
		{
			CreateMap<ParameterModel, Parameter>();
			CreateMap<Parameter, ParameterModel>();
		}
	}
}
