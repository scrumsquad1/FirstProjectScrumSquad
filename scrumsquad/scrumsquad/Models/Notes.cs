using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrumsquad.Models
{
	public class Notes
	{
        [BsonId]
        public string Id { get; set; }
       public string Subject { get; set; }
       public string Details { get; set; }
       public int Priority { get; set; }
    }
}