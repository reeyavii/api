using API.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Text.RegularExpressions;

namespace API.ValueGenerators
{
    public class MessageIdValueGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            var context = (DatabaseContext)entry.Context;
            var id = context.Messages.LastOrDefault()?.MessageId == null ?
                "A001"
                : Regex.Replace(context.Messages.LastOrDefault()?.MessageId, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));
            return id;
        }
    }
}
