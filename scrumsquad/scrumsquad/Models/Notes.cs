using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrumsquad.Models
{
	public class Notes
	{
       public int Id { get; set; }
       public string Subject { get; set; }
       public string Details { get; set; }
       public int Priority { get; set; }
    }
}