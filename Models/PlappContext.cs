using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PL_APP_MVC.Models;

public partial class PlappContext : DbContext
{
    public PlappContext()
    {
    }

    public PlappContext(DbContextOptions<PlappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aadharmaster> Aadharmasters { get; set; }

    public virtual DbSet<Bankdetail> Bankdetails { get; set; }

    public virtual DbSet<Basicdetail> Basicdetails { get; set; }

    public virtual DbSet<Companydetail> Companydetails { get; set; }

    public virtual DbSet<Companymaster> Companymasters { get; set; }

    public virtual DbSet<Doctypemaster> Doctypemasters { get; set; }

    public virtual DbSet<Docuploaddetail> Docuploaddetails { get; set; }

    public virtual DbSet<Loandetail> Loandetails { get; set; }

    public virtual DbSet<Panmaster> Panmasters { get; set; }

    public virtual DbSet<Personaldetail> Personaldetails { get; set; }

    public virtual DbSet<Pincodemaster> Pincodemasters { get; set; }

    public virtual DbSet<Rulesmaster> Rulesmasters { get; set; }

    public virtual DbSet<Usermaster> Usermasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PLAPP;Integrated Security=True;Encrypt=False;Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aadharmaster>(entity =>
        {
            entity.ToTable("aadharmaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aadharnumber)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("aadharnumber");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Fathername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fathername");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Middlename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("middlename");
        });

        modelBuilder.Entity<Bankdetail>(entity =>
        {
            entity.ToTable("bankdetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accholdername)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("accholdername");
            entity.Property(e => e.Accnumber).HasColumnName("accnumber");
            entity.Property(e => e.Acctype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("acctype");
            entity.Property(e => e.Appid).HasColumnName("appid");
            entity.Property(e => e.Bankbranch)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("bankbranch");
            entity.Property(e => e.Bankname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("bankname");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ifsccode");

            entity.HasOne(d => d.App).WithMany(p => p.Bankdetails)
                .HasForeignKey(d => d.Appid)
                .HasConstraintName("FK_bankdetails_basicdetails");
        });

        modelBuilder.Entity<Basicdetail>(entity =>
        {
            entity.ToTable("basicdetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aadharnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("aadharnumber");
            entity.Property(e => e.Appstage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("appstage");
            entity.Property(e => e.Appstatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("appstatus");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Emailid)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("emailid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Leadid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("leadid");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mobile");
            entity.Property(e => e.Pannumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pannumber");
            entity.Property(e => e.Pincode).HasColumnName("pincode");
            entity.Property(e => e.Statusremarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("statusremarks");
        });

        modelBuilder.Entity<Companydetail>(entity =>
        {
            entity.ToTable("companydetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppId).HasColumnName("appId");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.Companyaddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("companyaddress");
            entity.Property(e => e.Companymailid)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("companymailid");
            entity.Property(e => e.Companyname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyname");
            entity.Property(e => e.Grossincome).HasColumnName("grossincome");
            entity.Property(e => e.Obligations).HasColumnName("obligations");

            entity.HasOne(d => d.App).WithMany(p => p.Companydetails)
                .HasForeignKey(d => d.AppId)
                .HasConstraintName("FK_companydetails_basicdetails");
        });

        modelBuilder.Entity<Companymaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_categorymaster");

            entity.ToTable("companymaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.Companyname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("companyname");
        });

        modelBuilder.Entity<Doctypemaster>(entity =>
        {
            entity.ToTable("doctypemaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Doctype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("doctype");
        });

        modelBuilder.Entity<Docuploaddetail>(entity =>
        {
            entity.ToTable("docuploaddetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Appid).HasColumnName("appid");
            entity.Property(e => e.Docpath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("docpath");
            entity.Property(e => e.Doctype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("doctype");

            entity.HasOne(d => d.App).WithMany(p => p.Docuploaddetails)
                .HasForeignKey(d => d.Appid)
                .HasConstraintName("FK_docuploaddetails_basicdetails");
        });

        modelBuilder.Entity<Loandetail>(entity =>
        {
            entity.ToTable("loandetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Appid).HasColumnName("appid");
            entity.Property(e => e.Appliedamount).HasColumnName("appliedamount");
            entity.Property(e => e.Appliedtenure).HasColumnName("appliedtenure");
            entity.Property(e => e.Approvaldate).HasColumnName("approvaldate");
            entity.Property(e => e.Approvedamount).HasColumnName("approvedamount");
            entity.Property(e => e.Approvedemi)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("approvedemi");
            entity.Property(e => e.Approvedroi)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("approvedroi");
            entity.Property(e => e.Approvedtenure).HasColumnName("approvedtenure");
            entity.Property(e => e.Emi)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("emi");
            entity.Property(e => e.Roi)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("roi");

            entity.HasOne(d => d.App).WithMany(p => p.Loandetails)
                .HasForeignKey(d => d.Appid)
                .HasConstraintName("FK_loandetails_basicdetails");
        });

        modelBuilder.Entity<Panmaster>(entity =>
        {
            entity.ToTable("panmaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Fathername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fathername");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Middlename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("middlename");
            entity.Property(e => e.Pancardnumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("pancardnumber");
        });

        modelBuilder.Entity<Personaldetail>(entity =>
        {
            entity.ToTable("personaldetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Appid).HasColumnName("appid");
            entity.Property(e => e.Currentaddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("currentaddress");
            entity.Property(e => e.Fathername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fathername");
            entity.Property(e => e.Mothername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mothername");
            entity.Property(e => e.Permanentaddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("permanentaddress");
            entity.Property(e => e.Reference1email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("reference1email");
            entity.Property(e => e.Reference1mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("reference1mobile");
            entity.Property(e => e.Reference1name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reference1name");
            entity.Property(e => e.Reference1relation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reference1relation");
            entity.Property(e => e.Reference2email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("reference2email");
            entity.Property(e => e.Reference2mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("reference2mobile");
            entity.Property(e => e.Reference2name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reference2name");
            entity.Property(e => e.Reference2relation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reference2relation");

            entity.HasOne(d => d.App).WithMany(p => p.Personaldetails)
                .HasForeignKey(d => d.Appid)
                .HasConstraintName("FK_personaldetails_basicdetails");
        });

        modelBuilder.Entity<Pincodemaster>(entity =>
        {
            entity.ToTable("pincodemaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Circle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("circle");
            entity.Property(e => e.District)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("district");
            entity.Property(e => e.Pincode).HasColumnName("pincode");
            entity.Property(e => e.Servicable).HasColumnName("servicable");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Village)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("village");
        });

        modelBuilder.Entity<Rulesmaster>(entity =>
        {
            entity.ToTable("rulesmaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Maxvalue).HasColumnName("maxvalue");
            entity.Property(e => e.Minvalue).HasColumnName("minvalue");
            entity.Property(e => e.Rulename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rulename");
        });

        modelBuilder.Entity<Usermaster>(entity =>
        {
            entity.ToTable("usermaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
