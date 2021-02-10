﻿using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Exadel.CrazyPrice.Data.Models
{
    /// <summary>
    /// Represents the DbTag.
    /// </summary>
    public class DbTag
    {
        [BsonId]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [BsonIgnoreIfNull]
        public string Language { get; set; }

        [BsonIgnoreIfNull]
        public List<DbTag> Translations { get; set; }
    }
}
