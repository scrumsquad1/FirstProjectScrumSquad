using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrumsquad.Models
{
    public class Note : IComparable<Note>
    {
        [BsonId]
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public int Priority { get; set; }

        public int CompareTo(Note other)
        {
            // If they are equal, sort alphabetically by subject
            if (this.Priority == other.Priority)
            {
                return this.Subject.CompareTo(other.Subject);
            }
            // Otherwise sort by priority from high to low
            return other.Priority.CompareTo(this.Priority);
        }

    }
}