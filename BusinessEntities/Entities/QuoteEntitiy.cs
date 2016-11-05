using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities
{
    public class QuoteEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
    }
}
