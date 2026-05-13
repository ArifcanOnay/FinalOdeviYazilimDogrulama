using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Repository
{
    public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options)
    {
        public DbSet<Ogretmen> Ogretmenler {  get; set; }
        public DbSet<YoutubeVideo> YoutubeVideoları {  get; set; }
        public DbSet<Podcast> Podcastler {  get; set; }
        public DbSet<OzelDers>OzelDersler { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VideoProgress> VideoProgresses { get; set; }
        public DbSet<ReferansLink> ReferansLinkler { get; set; }
        public DbSet<Hakkimda> Hakkimda { get; set; }
        public DbSet<FlowPuzzleNoun> FlowPuzzleNouns { get; set; }
        public DbSet<FlowPuzzleSentence> FlowPuzzleSentences { get; set; }
        public DbSet<FlowPuzzleRelativeClause> FlowPuzzleRelativeClauses { get; set; }
        public DbSet<FlowPuzzleLongPhrase> FlowPuzzleLongPhrases { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}
