using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ems.Domain.Primitives;
using Ems.Domain.ValueObjects;
using Ems.Core.Interfaces;
using Ems.Core.Entities;

namespace Ems.Domain.Entities
{
    public class Expense : AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string TransactionNumber { get; set; }

        [Required]
        public string TransactionDetails { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal Balance { get; set; }
    }
}
