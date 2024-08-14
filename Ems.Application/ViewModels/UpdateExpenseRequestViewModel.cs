using Ems.Application.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.ViewModels
{
    public class UpdateExpenseRequestViewModel: CreateExpenseRequestViewModel
    {
        public Guid Id { get; set; }
    }
}
