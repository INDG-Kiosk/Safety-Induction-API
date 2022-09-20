using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Model
{
    public class Message<TEntity>
    {
        public string Status { get; set; } = "E";
        public string Text { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public TEntity Result { get; set; }
    }
}
