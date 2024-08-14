using AutoMapper;
using Ems.Application.ViewModels.Factories;
using Ems.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Expense, GetExpenseRequestViewModel>()

                .ReverseMap();
            
        }
    }
}
