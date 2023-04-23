using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibManagementAPI.Models;

public partial class LibraryContext : DbContext
{

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBook> TblBooks { get; set; }

    public virtual DbSet<TblBookAuthor> TblBookAuthors { get; set; }

    public virtual DbSet<TblBookCopy> TblBookCopies { get; set; }

    public virtual DbSet<TblBookLoan> TblBookLoans { get; set; }

    public virtual DbSet<TblBorrower> TblBorrowers { get; set; }

    public virtual DbSet<TblLibraryBranch> TblLibraryBranches { get; set; }

    public virtual DbSet<TblPublisher> TblPublishers { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBook>(entity =>
        {
            entity.HasKey(e => e.BookBookId).HasName("PK__tbl_book__6B53AB8BF5B5D235");

            entity.ToTable("tbl_book", tb =>
            {
                tb.HasTrigger("MSmerge_del_FD4D4600FF2B470BA625232E15981DFD");
                tb.HasTrigger("MSmerge_ins_FD4D4600FF2B470BA625232E15981DFD");
                tb.HasTrigger("MSmerge_upd_FD4D4600FF2B470BA625232E15981DFD");
            });

            entity.ToTable("tbl_book");

            entity.Property(e => e.BookBookId).HasColumnName("book_BookID");
            entity.Property(e => e.BookPublisherName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("book_PublisherName");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("book_Title");

            entity.HasOne(d => d.BookPublisherNameNavigation).WithMany(p => p.TblBooks)
                .HasForeignKey(d => d.BookPublisherName)
                .HasConstraintName("fk_publisher_name1");
        });

        modelBuilder.Entity<TblBookAuthor>(entity =>
        {
            entity.HasKey(e => e.BookAuthorsAuthorId).HasName("PK__tbl_book__A3D4D4CD4124FD92");

            entity.ToTable("tbl_book_authors", tb =>
            {
                tb.HasTrigger("MSmerge_del_51C88C089E1C4CACA4122C15899CED2A");
                tb.HasTrigger("MSmerge_ins_51C88C089E1C4CACA4122C15899CED2A");
                tb.HasTrigger("MSmerge_upd_51C88C089E1C4CACA4122C15899CED2A");
            });

            entity.ToTable("tbl_book_authors");

            entity.Property(e => e.BookAuthorsAuthorId).HasColumnName("book_authors_AuthorID");
            entity.Property(e => e.BookAuthorsAuthorName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("book_authors_AuthorName");
            entity.Property(e => e.BookAuthorsBookId).HasColumnName("book_authors_BookID");

            entity.HasOne(d => d.BookAuthorsBook).WithMany(p => p.TblBookAuthors)
                .HasForeignKey(d => d.BookAuthorsBookId)
                .HasConstraintName("fk_book_id3");
        });

        modelBuilder.Entity<TblBookCopy>(entity =>
        {
            entity.HasKey(e => e.BookCopiesCopiesId).HasName("PK__tbl_book__18E86D1F301B6A07");

            entity.ToTable("tbl_book_copies", tb =>
            {
                tb.HasTrigger("MSmerge_del_AFBB2C43155140A388B7E5615CE88BAB");
                tb.HasTrigger("MSmerge_ins_AFBB2C43155140A388B7E5615CE88BAB");
                tb.HasTrigger("MSmerge_upd_AFBB2C43155140A388B7E5615CE88BAB");
            });

            entity.ToTable("tbl_book_copies");

            entity.Property(e => e.BookCopiesCopiesId).HasColumnName("book_copies_CopiesID");
            entity.Property(e => e.BookCopiesBookId).HasColumnName("book_copies_BookID");
            entity.Property(e => e.BookCopiesBranchId).HasColumnName("book_copies_BranchID");
            entity.Property(e => e.BookCopiesNoOfCopies).HasColumnName("book_copies_No_Of_Copies");

            entity.HasOne(d => d.BookCopiesBook).WithMany(p => p.TblBookCopies)
                .HasForeignKey(d => d.BookCopiesBookId)
                .HasConstraintName("fk_book_id2");

            entity.HasOne(d => d.BookCopiesBranch).WithMany(p => p.TblBookCopies)
                .HasForeignKey(d => d.BookCopiesBranchId)
                .HasConstraintName("fk_branch_id2");
        });

        modelBuilder.Entity<TblBookLoan>(entity =>
        {
            entity.HasKey(e => e.BookLoansLoansId).HasName("PK__tbl_book__9A03D6FB73154FCB");

            entity.ToTable("tbl_book_loans", tb =>
            {
                tb.HasTrigger("MSmerge_del_627A8233165E4D24A83327300E73DA2E");
                tb.HasTrigger("MSmerge_ins_627A8233165E4D24A83327300E73DA2E");
                tb.HasTrigger("MSmerge_upd_627A8233165E4D24A83327300E73DA2E");
            }); 

            entity.ToTable("tbl_book_loans");

            entity.Property(e => e.BookLoansLoansId).HasColumnName("book_loans_LoansID");
            entity.Property(e => e.BookLoansBookId).HasColumnName("book_loans_BookID");
            entity.Property(e => e.BookLoansBranchId).HasColumnName("book_loans_BranchID");
            entity.Property(e => e.BookLoansCardNo).HasColumnName("book_loans_CardNo");
            entity.Property(e => e.BookLoansDateOut)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("book_loans_DateOut");
            entity.Property(e => e.BookLoansDueDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("book_loans_DueDate");
            entity.Property(e => e.BookLoansStatus).HasColumnName("book_loans_Status");

            entity.HasOne(d => d.BookLoansBook).WithMany(p => p.TblBookLoans)
                .HasForeignKey(d => d.BookLoansBookId)
                .HasConstraintName("fk_book_id1");

            entity.HasOne(d => d.BookLoansBranch).WithMany(p => p.TblBookLoans)
                .HasForeignKey(d => d.BookLoansBranchId)
                .HasConstraintName("fk_branch_id1");

            entity.HasOne(d => d.BookLoansCardNoNavigation).WithMany(p => p.TblBookLoans)
                .HasForeignKey(d => d.BookLoansCardNo)
                .HasConstraintName("fk_cardno");
        });

        modelBuilder.Entity<TblBorrower>(entity =>
        {
            entity.HasKey(e => e.BorrowerCardNo).HasName("PK__tbl_borr__F3500D9646B2BE64");

            entity.ToTable("tbl_borrower", tb =>
            {
                tb.HasTrigger("MSmerge_del_7813748A1ACF48D1A7D23A90859618FB");
                tb.HasTrigger("MSmerge_ins_7813748A1ACF48D1A7D23A90859618FB");
                tb.HasTrigger("MSmerge_upd_7813748A1ACF48D1A7D23A90859618FB");
            });

            entity.ToTable("tbl_borrower");

            entity.Property(e => e.BorrowerCardNo).HasColumnName("borrower_CardNo");
            entity.Property(e => e.BorrowerBorrowerAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("borrower_BorrowerAddress");
            entity.Property(e => e.BorrowerBorrowerName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("borrower_BorrowerName");
            entity.Property(e => e.BorrowerBorrowerPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("borrower_BorrowerPhone");
        });

        modelBuilder.Entity<TblLibraryBranch>(entity =>
        {
            entity.HasKey(e => e.LibraryBranchBranchId).HasName("PK__tbl_libr__7C4A21D39A01C337");

            entity.ToTable("tbl_library_branch", tb =>
            {
                tb.HasTrigger("MSmerge_del_3463711C072E415EAD853182F262A470");
                tb.HasTrigger("MSmerge_ins_3463711C072E415EAD853182F262A470");
                tb.HasTrigger("MSmerge_upd_3463711C072E415EAD853182F262A470");
            });

            entity.ToTable("tbl_library_branch");

            entity.Property(e => e.LibraryBranchBranchId).HasColumnName("library_branch_BranchID");
            entity.Property(e => e.LibraryBranchBranchAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("library_branch_BranchAddress");
            entity.Property(e => e.LibraryBranchBranchName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("library_branch_BranchName");
        });

        modelBuilder.Entity<TblPublisher>(entity =>
        {
            entity.HasKey(e => e.PublisherPublisherName).HasName("PK__tbl_publ__EEF598AA09DE5AD5");
            entity.ToTable("tbl_publisher", tb =>
            {
                tb.HasTrigger("MSmerge_del_A0E5618699C24AB6AFF164F95FA6C870");
                tb.HasTrigger("MSmerge_ins_A0E5618699C24AB6AFF164F95FA6C870");
                tb.HasTrigger("MSmerge_upd_A0E5618699C24AB6AFF164F95FA6C870");
            });


            entity.ToTable("tbl_publisher");

            entity.Property(e => e.PublisherPublisherName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("publisher_PublisherName");
            entity.Property(e => e.PublisherPublisherAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("publisher_PublisherAddress");
            entity.Property(e => e.PublisherPublisherPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("publisher_PublisherPhone");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_user");

            entity.Property(e => e.UserCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("user_CreatedDate");
            entity.Property(e => e.UserDisplayName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("user_DisplayName");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("user_Email");
            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("user_ID");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("user_Password");
            entity.Property(e => e.UserRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
