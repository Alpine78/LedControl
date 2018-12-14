using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HarjoitustyoLed
{
    public class SequenceContext : DbContext
    {
        public DbSet<LedSequence> LedSequences { get; set; }
        public DbSet<TimeRow> TimeRows { get; set; }
        public DbSet<LedRow> LedRows { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sequence.db");
        }
    }

    public class LedSequence
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TimeRow> TimeRows { get; set; }
    }
    public class TimeRow
    {
        public int Id { get; set; }
        public int Time { get; set; }
        public IList<LedRow> LedRows { get; set; }
        public LedSequence LedSequence { get; set; }
    }

    public class LedRow
    {
        public int Id { get; set; }
        public int PinId { get; set; }
        public int Status { get; set; }
        public TimeRow TimeRow { get; set; }
    }

    class SequenceModel
    {
    }
}
