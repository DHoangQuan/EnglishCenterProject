namespace EnglishCenter.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelContext1 : DbContext
    {
        public ModelContext1()
            : base("name=ModelContext1")
        {
        }

        public virtual DbSet<Authentication> Authentications { get; set; }
        public virtual DbSet<Catalogue> Catalogues { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<LecturerTime> LecturerTimes { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<QuantityAndAttendance> QuantityAndAttendances { get; set; }
        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<StudentTime> StudentTimes { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<TopicPost> TopicPosts { get; set; }
        public virtual DbSet<UsingRoom> UsingRooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authentication>()
                .Property(e => e.PeopleID)
                .IsUnicode(false);

            modelBuilder.Entity<Catalogue>()
                .Property(e => e.Catalogue1)
                .IsUnicode(false);

            modelBuilder.Entity<Catalogue>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Catalogue>()
                .HasMany(e => e.TopicPosts)
                .WithOptional(e => e.Catalogue)
                .HasForeignKey(e => e.CatalogueID);

            modelBuilder.Entity<Class>()
                .Property(e => e.ClassID)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.LessonID)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.PeopleID)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.PeopleID)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.PeopleID)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.TopicPostID)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Content)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LecturerTime>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<LecturerTime>()
                .Property(e => e.LecturerID)
                .IsUnicode(false);

            modelBuilder.Entity<LecturerTime>()
                .Property(e => e.Year)
                .IsUnicode(false);

            modelBuilder.Entity<LecturerTime>()
                .Property(e => e.Month)
                .IsUnicode(false);

            modelBuilder.Entity<Lesson>()
                .Property(e => e.LessonID)
                .IsUnicode(false);

            modelBuilder.Entity<Lesson>()
                .Property(e => e.TopicID)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.PeopleID)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .HasOptional(e => e.Authentication)
                .WithRequired(e => e.Person);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.LecturerTimes)
                .WithRequired(e => e.Person)
                .HasForeignKey(e => e.LecturerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.StudentTimes)
                .WithOptional(e => e.Person)
                .HasForeignKey(e => e.StudentID);

            modelBuilder.Entity<QuantityAndAttendance>()
                .Property(e => e.QnAID)
                .IsUnicode(false);

            modelBuilder.Entity<QuantityAndAttendance>()
                .Property(e => e.PeopleID)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.QuoteID)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.RommID)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.UsingRooms)
                .WithOptional(e => e.Room)
                .HasForeignKey(e => e.RoomID);

            modelBuilder.Entity<Skill>()
                .Property(e => e.SkilID)
                .IsUnicode(false);

            modelBuilder.Entity<Skill>()
                .HasMany(e => e.Topics)
                .WithOptional(e => e.Skill)
                .HasForeignKey(e => e.SkillID);

            modelBuilder.Entity<Slot>()
                .Property(e => e.SlotID)
                .IsUnicode(false);

            modelBuilder.Entity<StudentTime>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<StudentTime>()
                .Property(e => e.StudentID)
                .IsUnicode(false);

            modelBuilder.Entity<Topic>()
                .Property(e => e.TopicID)
                .IsUnicode(false);

            modelBuilder.Entity<Topic>()
                .Property(e => e.SkillID)
                .IsUnicode(false);

            modelBuilder.Entity<TopicPost>()
                .Property(e => e.TopicPostID)
                .IsUnicode(false);

            modelBuilder.Entity<TopicPost>()
                .Property(e => e.CatalogueID)
                .IsUnicode(false);

            modelBuilder.Entity<TopicPost>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<UsingRoom>()
                .Property(e => e.RoomID)
                .IsUnicode(false);

            modelBuilder.Entity<UsingRoom>()
                .Property(e => e.SlotID)
                .IsUnicode(false);

            modelBuilder.Entity<UsingRoom>()
                .Property(e => e.ClassID)
                .IsUnicode(false);
        }
    }
}
