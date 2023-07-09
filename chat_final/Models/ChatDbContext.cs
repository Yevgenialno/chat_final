using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity;
using Microsoft.IdentityModel;

namespace chat_final.Models;

public partial class ChatDbContext : IdentityDbContext
{
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Dialog> StartedDialogs { get; set; }
    public ChatDbContext()
    {
    }

    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    {
        var builder = WebApplication.CreateBuilder();
        optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("ChatDbContextConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
