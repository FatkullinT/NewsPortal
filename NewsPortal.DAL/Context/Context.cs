using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Context
{
    public class Context : DbContext
    {
        public Context(string connectionStringName) : base(connectionStringName)
        {
            Configuration.AutoDetectChangesEnabled = false;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }

        public DbSet<User> Users
        {
            get;
            set;
        }

        public DbSet<Role> Roles
        {
            get;
            set;
        }

        public DbSet<Membership> Memberships
        {
            get;
            set;
        }

        public DbSet<OAuthMembership> OAuthMemberships
        {
            get;
            set;
        }

        public DbSet<Account> Accounts
        {
            get;
            set;
        }

        public DbSet<News> News
        {
            get;
            set;
        }

        public DbSet<NewsText> NewsTexts
        {
            get;
            set;
        }

        /// <summary>
        /// Настройка модели
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigurateUser(modelBuilder.Entity<User>());
            ConfigurateRole(modelBuilder.Entity<Role>());
            ConfigurateAccount(modelBuilder.Entity<Account>());
            ConfigurateMembership(modelBuilder.Entity<Membership>());
            ConfigurateOAuthMembership(modelBuilder.Entity<OAuthMembership>());
            ConfigurateNews(modelBuilder.Entity<News>());
            ConfigurateNewsText(modelBuilder.Entity<NewsText>());
        }

        /// <summary>
        /// Настройка таблицы ролей
        /// </summary>
        /// <param name="configuration"></param>
        private void ConfigurateRole(EntityTypeConfiguration<Role> configuration)
        {
            configuration.HasKey(role => role.Id);
            configuration.Property(role => role.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            configuration.Property(role => role.Name).HasMaxLength(50);
            configuration.Property(role => role.Name).HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute()));
            configuration.HasMany(role => role.Users).WithMany(user => user.Roles).Map(cs =>
            {
                cs.MapLeftKey("RoleId");
                cs.MapRightKey("UserId");
                cs.ToTable("UserRole");
            });
        }

        /// <summary>
        /// Настройка теблицы с текстами новостей
        /// </summary>
        /// <param name="configuration"></param>
        private static void ConfigurateNewsText(EntityTypeConfiguration<NewsText> configuration)
        {
            configuration.HasKey(newsText => newsText.Id);
            configuration.Property(newsText => newsText.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        /// <summary>
        /// Настройка таблицы с новостями
        /// </summary>
        /// <param name="configuration"></param>
        private static void ConfigurateNews(EntityTypeConfiguration<News> configuration)
        {
            configuration.HasKey(news => news.Id);
            configuration.Property(news => news.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        /// <summary>
        /// Настройка таблицы с данными авторизации через OAuth
        /// </summary>
        /// <param name="configuration"></param>
        private static void ConfigurateOAuthMembership(EntityTypeConfiguration<OAuthMembership> configuration)
        {
            configuration.HasKey(membership => membership.Id);
            configuration.Property(membership => membership.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            configuration.Property(membership => membership.OAuthUserId).HasMaxLength(20);
            configuration.Property(membership => membership.OAuthUserId)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute()));
        }

        /// <summary>
        /// Настройка таблицы с данными авторизации через логин/пароль
        /// </summary>
        /// <param name="configuration"></param>
        private static void ConfigurateMembership(EntityTypeConfiguration<Membership> configuration)
        {
            configuration.HasKey(membership => membership.Id);
            configuration.Property(membership => membership.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        /// <summary>
        /// Настройка таблицы с дополнительными данными пользователя
        /// </summary>
        /// <param name="configuration"></param>
        private static void ConfigurateAccount(EntityTypeConfiguration<Account> configuration)
        {
            configuration.HasKey(account => account.Id);
            configuration.Property(account => account.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            configuration.Property(account => account.Email).HasMaxLength(50);
        }

        /// <summary>
        /// Настройка таблицы пользователей
        /// </summary>
        /// <param name="configuration"></param>
        private static void ConfigurateUser(EntityTypeConfiguration<User> configuration)
        {
            configuration.HasKey(user => user.Id);
            configuration.Property(user => user.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            configuration.Property(user => user.Name).HasMaxLength(50);
            configuration.Property(user => user.Name).HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute()));
        }
    }
}
