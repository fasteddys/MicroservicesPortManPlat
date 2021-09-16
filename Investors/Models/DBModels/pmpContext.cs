using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Investors.Models.DBModels
{
    public partial class pmpContext : DbContext
    {
        public pmpContext()
        {
        }

        public pmpContext(DbContextOptions<pmpContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collateral> Collaterals { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<Insolvency> Insolvencies { get; set; }
        public virtual DbSet<Investor> Investors { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<PortfolioContract> PortfolioContracts { get; set; }
        public virtual DbSet<PortfolioInvestor> PortfolioInvestors { get; set; }
        public virtual DbSet<PortfolioParticipant> PortfolioParticipants { get; set; }
        public virtual DbSet<PortfolioProcedure> PortfolioProcedures { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Procedure> Procedures { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=pmpDb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_bin");

            modelBuilder.Entity<Collateral>(entity =>
            {
                entity.ToTable("collaterals");

                entity.HasIndex(e => e.ContractId, "collaterals_contract_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "collaterals_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CadastralReference)
                    .HasMaxLength(45)
                    .HasColumnName("cadastral_reference");

                entity.Property(e => e.City)
                    .HasMaxLength(45)
                    .HasColumnName("city");

                entity.Property(e => e.CollateralId)
                    .HasMaxLength(45)
                    .HasColumnName("collateral_id");

                entity.Property(e => e.CollateralSubtype)
                    .HasMaxLength(45)
                    .HasColumnName("collateral_subtype");

                entity.Property(e => e.CollateralType)
                    .HasMaxLength(45)
                    .HasColumnName("collateral_type");

                entity.Property(e => e.CollateralUsage)
                    .HasMaxLength(45)
                    .HasColumnName("collateral_usage");

                entity.Property(e => e.ConstuctionYear)
                    .HasMaxLength(45)
                    .HasColumnName("constuction_year");

                entity.Property(e => e.Contract)
                    .HasMaxLength(45)
                    .HasColumnName("contract");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Country)
                    .HasMaxLength(45)
                    .HasColumnName("country");

                entity.Property(e => e.Idufir)
                    .HasMaxLength(45)
                    .HasColumnName("idufir");

                entity.Property(e => e.InitialAppraisalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("initial_appraisal_date");

                entity.Property(e => e.InitialAppraisalValue)
                    .HasPrecision(15, 1)
                    .HasColumnName("initial_appraisal_value");

                entity.Property(e => e.LandRegistryNumber)
                    .HasMaxLength(45)
                    .HasColumnName("land_registry_number");

                entity.Property(e => e.LandRegistryTown)
                    .HasMaxLength(45)
                    .HasColumnName("land_registry_town");

                entity.Property(e => e.LastAppraisalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_appraisal_date");

                entity.Property(e => e.LastAppraisalValue)
                    .HasPrecision(15, 2)
                    .HasColumnName("last_appraisal_value");

                entity.Property(e => e.Liens)
                    .HasPrecision(15, 2)
                    .HasColumnName("liens");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("process_date");

                entity.Property(e => e.Province)
                    .HasMaxLength(45)
                    .HasColumnName("province");

                entity.Property(e => e.Region)
                    .HasMaxLength(45)
                    .HasColumnName("region");

                entity.Property(e => e.RegistryAssetId).HasColumnName("registry_asset_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(45)
                    .HasColumnName("status");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.Property(e => e.SurfaceM2).HasColumnName("surface_m2");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(45)
                    .HasColumnName("zip_code");

                entity.HasOne(d => d.ContractNavigation)
                    .WithMany(p => p.Collaterals)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("collaterals_contract_id_foreign");

                entity.HasOne(d => d.PortfolioNavigation)
                    .WithMany(p => p.Collaterals)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("collaterals_portfolio_id_foreign");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("contracts");

                entity.HasIndex(e => e.PortfolioId, "contracts_portfolio_id_foreign");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountingSituation)
                    .HasMaxLength(45)
                    .HasColumnName("accounting_situation");

                entity.Property(e => e.ContractId)
                    .HasMaxLength(45)
                    .HasColumnName("contract_id");

                entity.Property(e => e.ContractType)
                    .HasMaxLength(45)
                    .HasColumnName("contract_type");

                entity.Property(e => e.Currency)
                    .HasMaxLength(45)
                    .HasColumnName("currency");

                entity.Property(e => e.DebtType)
                    .HasMaxLength(45)
                    .HasColumnName("debt_type");

                entity.Property(e => e.DefaultDate)
                    .HasColumnType("datetime")
                    .HasColumnName("default_date");

                entity.Property(e => e.DefaultInterests)
                    .HasPrecision(15, 2)
                    .HasColumnName("default_interests");

                entity.Property(e => e.EarlyMaturity).HasColumnName("early_maturity");

                entity.Property(e => e.EarlyMaturityDate)
                    .HasColumnType("datetime")
                    .HasColumnName("early_maturity_date");

                entity.Property(e => e.FirstUnpaidInstalmentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("first_unpaid_instalment_date");

                entity.Property(e => e.Insolvency)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency");

                entity.Property(e => e.JudicialProcess).HasColumnName("judicial_process");

                entity.Property(e => e.LegalProcess).HasColumnName("legal_process");

                entity.Property(e => e.LtvOriginal).HasColumnName("ltv_original");

                entity.Property(e => e.MainParticipantId)
                    .HasMaxLength(45)
                    .HasColumnName("main_participant_id");

                entity.Property(e => e.MaturityDate)
                    .HasColumnType("datetime")
                    .HasColumnName("maturity_date");

                entity.Property(e => e.MaturityPrincipalBalance)
                    .HasPrecision(15, 2)
                    .HasColumnName("maturity_principal_balance");

                entity.Property(e => e.Novation).HasColumnName("novation");

                entity.Property(e => e.NovationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("novation_date");

                entity.Property(e => e.NumGuarantors).HasColumnName("num_guarantors");

                entity.Property(e => e.NumParticipants).HasColumnName("num_participants");

                entity.Property(e => e.OrdinaryInterests)
                    .HasPrecision(15, 2)
                    .HasColumnName("ordinary_interests");

                entity.Property(e => e.OriginalEntity)
                    .HasMaxLength(45)
                    .HasColumnName("original_entity");

                entity.Property(e => e.OriginationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("origination_date");

                entity.Property(e => e.OutstandingInstalments).HasColumnName("outstanding_instalments");

                entity.Property(e => e.OutstandingPrincipalBalance)
                    .HasPrecision(15, 2)
                    .HasColumnName("outstanding_principal_balance");

                entity.Property(e => e.PerformingStatus)
                    .HasMaxLength(45)
                    .HasColumnName("performing_status");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ProceedingType)
                    .HasMaxLength(45)
                    .HasColumnName("proceeding_type");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("process_date");

                entity.Property(e => e.Reo).HasColumnName("reo");

                entity.Property(e => e.ReoAmount)
                    .HasPrecision(15, 2)
                    .HasColumnName("reo_amount");

                entity.Property(e => e.Securitized).HasColumnName("securitized");

                entity.Property(e => e.SecurityNumber)
                    .HasMaxLength(45)
                    .HasColumnName("security_number");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.Property(e => e.Syndicated).HasColumnName("syndicated");

                entity.Property(e => e.SyndicationPercent)
                    .HasPrecision(15, 2)
                    .HasColumnName("syndication_percent");

                entity.Property(e => e.TotalAmountOb)
                    .HasPrecision(15, 2)
                    .HasColumnName("total_amount_ob");

                entity.Property(e => e.UnpaidInstalments).HasColumnName("unpaid_instalments");

                entity.HasOne(d => d.PortfolioNavigation)
                    .WithMany(p => p.ContractsNavigation)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("contracts_portfolio_id_foreign");
            });

            modelBuilder.Entity<Home>(entity =>
            {
                entity.HasKey(e => e.IdHome)
                    .HasName("PRIMARY");

                entity.ToTable("home");

                entity.HasIndex(e => e.PortfolioId, "home_portfolio_id_foreign_idx");

                entity.Property(e => e.IdHome).HasColumnName("id_home");

                entity.Property(e => e.AcquisitionYear).HasColumnName("acquisition_year");

                entity.Property(e => e.AssignmentEntity)
                    .HasMaxLength(45)
                    .HasColumnName("assignment_entity");

                entity.Property(e => e.DebtType)
                    .HasMaxLength(45)
                    .HasColumnName("debt_type");

                entity.Property(e => e.NominalValue)
                    .HasPrecision(15, 2)
                    .HasColumnName("nominal_value");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.PriceSecured)
                    .HasPrecision(15, 2)
                    .HasColumnName("price_secured");

                entity.Property(e => e.Secured)
                    .HasPrecision(15, 2)
                    .HasColumnName("secured");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.Property(e => e.Unsecured)
                    .HasPrecision(15, 2)
                    .HasColumnName("unsecured");

                entity.Property(e => e.UnsecuredPrice)
                    .HasPrecision(15, 2)
                    .HasColumnName("unsecured_price");

                entity.HasOne(d => d.PortfolioNavigation)
                    .WithMany(p => p.Homes)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("home_portfolio_id_foreign");
            });

            modelBuilder.Entity<Insolvency>(entity =>
            {
                entity.ToTable("insolvency");

                entity.HasIndex(e => e.ParticipantId, "insolvency_participant_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "insolvency_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuctionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("auction_date");

                entity.Property(e => e.CourtInsolvencyAddress)
                    .HasMaxLength(45)
                    .HasColumnName("court_insolvency_address");

                entity.Property(e => e.CourtInsolvencyCity)
                    .HasMaxLength(45)
                    .HasColumnName("court_insolvency_city");

                entity.Property(e => e.CourtInsolvencyName)
                    .HasMaxLength(45)
                    .HasColumnName("court_insolvency_name");

                entity.Property(e => e.CourtInsolvencyNumber)
                    .HasMaxLength(45)
                    .HasColumnName("court_insolvency_number");

                entity.Property(e => e.CourtInsolvencyProvince)
                    .HasMaxLength(45)
                    .HasColumnName("court_insolvency_province");

                entity.Property(e => e.CourtInsolvencyZipCode)
                    .HasMaxLength(45)
                    .HasColumnName("court_insolvency_zip_code");

                entity.Property(e => e.CourtProcedureNumber)
                    .HasMaxLength(45)
                    .HasColumnName("court_procedure_number");

                entity.Property(e => e.InsolvencyAutoAdjudicationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("insolvency_auto_adjudication_date");

                entity.Property(e => e.InsolvencyDate)
                    .HasColumnType("datetime")
                    .HasColumnName("insolvency_date");

                entity.Property(e => e.InsolvencyId)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_id");

                entity.Property(e => e.InsolvencyLaywerMail)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_laywer_mail");

                entity.Property(e => e.InsolvencyLaywerName)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_laywer_name");

                entity.Property(e => e.InsolvencyLaywerPhoneNumber)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_laywer_phone_number");

                entity.Property(e => e.InsolvencyManagerEmail)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_manager_email");

                entity.Property(e => e.InsolvencyManagerName)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_manager_name");

                entity.Property(e => e.InsolvencyManagerPhoneNumber)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_manager_phone_number");

                entity.Property(e => e.InsolvencyPhase)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_phase");

                entity.Property(e => e.InsolvencySolicitorMail)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_solicitor_mail");

                entity.Property(e => e.InsolvencySolicitorName)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_solicitor_name");

                entity.Property(e => e.InsolvencySolicitorPhoneNumber)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_solicitor_phone_number");

                entity.Property(e => e.InsolvencyType)
                    .HasMaxLength(45)
                    .HasColumnName("insolvency_type");

                entity.Property(e => e.LastInsolvencyActionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_insolvency_action_date");

                entity.Property(e => e.LiquidationPlanApprovalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("liquidation_plan_approval_date");

                entity.Property(e => e.LiquidationPlanDate)
                    .HasColumnType("datetime")
                    .HasColumnName("liquidation_plan_date");

                entity.Property(e => e.Participant)
                    .HasMaxLength(45)
                    .HasColumnName("participant");

                entity.Property(e => e.ParticipantId).HasColumnName("participant_id");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("process_date");

                entity.Property(e => e.RecognizedInclusionAmount)
                    .HasPrecision(15, 2)
                    .HasColumnName("recognized_inclusion_amount");

                entity.Property(e => e.RecognizedInclusionUnderAmount)
                    .HasPrecision(15, 2)
                    .HasColumnName("recognized_inclusion_under_amount");

                entity.Property(e => e.StartLiquidationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_liquidation_date");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.HasOne(d => d.ParticipantNavigation)
                    .WithMany(p => p.Insolvencies)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("insolvency_participant_id_foreign");

                entity.HasOne(d => d.PortfolioNavigation)
                    .WithMany(p => p.Insolvencies)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("insolvency_portfolio_id_foreign");
            });

            modelBuilder.Entity<Investor>(entity =>
            {
                entity.ToTable("investors");

                entity.HasIndex(e => e.ContractId, "investors_contract_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "investors_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bank)
                    .HasMaxLength(45)
                    .HasColumnName("bank");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Iban)
                    .HasMaxLength(45)
                    .HasColumnName("iban");

                entity.Property(e => e.InvestorName)
                    .HasMaxLength(45)
                    .HasColumnName("investor_name");

                entity.Property(e => e.Mail)
                    .HasMaxLength(45)
                    .HasColumnName("mail");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.SocialAddress)
                    .HasMaxLength(45)
                    .HasColumnName("social_address");

                entity.Property(e => e.TaxIdentification)
                    .HasMaxLength(45)
                    .HasColumnName("tax_identification");

                entity.Property(e => e.Telephone1)
                    .HasMaxLength(45)
                    .HasColumnName("telephone1");

                entity.Property(e => e.Telephone2)
                    .HasMaxLength(45)
                    .HasColumnName("telephone2");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Investors)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("investors_contract_id_foreign");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.Investors)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("investors_portfolio_id_foreign");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("participants");

                entity.HasIndex(e => e.ContractId, "participants_contact_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "participants_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(45)
                    .HasColumnName("address");

                entity.Property(e => e.BirthdayDate)
                    .HasColumnType("datetime")
                    .HasColumnName("birthday_date");

                entity.Property(e => e.City)
                    .HasMaxLength(45)
                    .HasColumnName("city");

                entity.Property(e => e.Cnae)
                    .HasMaxLength(45)
                    .HasColumnName("cnae");

                entity.Property(e => e.CnaeDescription)
                    .HasMaxLength(45)
                    .HasColumnName("cnae_description");

                entity.Property(e => e.Cno)
                    .HasMaxLength(45)
                    .HasColumnName("cno");

                entity.Property(e => e.Contract)
                    .HasMaxLength(45)
                    .HasColumnName("contract");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Country)
                    .HasMaxLength(45)
                    .HasColumnName("country");

                entity.Property(e => e.DebtorType)
                    .HasMaxLength(45)
                    .HasColumnName("debtor_type");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .HasColumnName("email");

                entity.Property(e => e.Identification)
                    .HasMaxLength(45)
                    .HasColumnName("identification");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(45)
                    .HasColumnName("nationality");

                entity.Property(e => e.ParticipantId)
                    .HasMaxLength(45)
                    .HasColumnName("participant_id");

                entity.Property(e => e.PersonType)
                    .HasMaxLength(45)
                    .HasColumnName("person_type");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("process_date");

                entity.Property(e => e.Province)
                    .HasMaxLength(45)
                    .HasColumnName("province");

                entity.Property(e => e.Region)
                    .HasMaxLength(45)
                    .HasColumnName("region");

                entity.Property(e => e.Resident)
                    .HasMaxLength(45)
                    .HasColumnName("resident");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.Property(e => e.Telephone1)
                    .HasMaxLength(45)
                    .HasColumnName("telephone1");

                entity.Property(e => e.Telephone2)
                    .HasMaxLength(45)
                    .HasColumnName("telephone2");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(45)
                    .HasColumnName("zip_code");

                entity.HasOne(d => d.ContractNavigation)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("participants_contact_id_foreign");

                entity.HasOne(d => d.PortfolioNavigation)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("participants_portfolio_id_foreign");
            });

            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.ToTable("portfolios");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClosingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("closing_date");

                entity.Property(e => e.ClosingOb)
                    .HasPrecision(15, 2)
                    .HasColumnName("closing_ob");

                entity.Property(e => e.Contracts).HasColumnName("contracts");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.CutOffDate)
                    .HasColumnType("datetime")
                    .HasColumnName("cut_off_date");

                entity.Property(e => e.CutOffOb)
                    .HasPrecision(15, 2)
                    .HasColumnName("cut_off_ob");

                entity.Property(e => e.HolderEntity)
                    .HasMaxLength(45)
                    .HasColumnName("holder_entity");

                entity.Property(e => e.Investor)
                    .HasMaxLength(100)
                    .HasColumnName("investor");

                entity.Property(e => e.OperationType)
                    .HasMaxLength(45)
                    .HasColumnName("operation_type");

                entity.Property(e => e.Portfolio1)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.SigningDate)
                    .HasColumnType("datetime")
                    .HasColumnName("signing_date");

                entity.Property(e => e.SigningOb)
                    .HasPrecision(15, 2)
                    .HasColumnName("signing_ob");

                entity.Property(e => e.Status)
                    .HasMaxLength(45)
                    .HasColumnName("status");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.Property(e => e.Tipology)
                    .HasMaxLength(100)
                    .HasColumnName("tipology");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<PortfolioContract>(entity =>
            {
                entity.ToTable("portfolio_contract");

                entity.HasIndex(e => e.ContractId, "portfolio_contract_contract_id_idx");

                entity.HasIndex(e => e.PortfolioId, "portfolio_contract_portfolio_id_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.PortfolioContracts)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("portfolio_contract_contract_id");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.PortfolioContracts)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("portfolio_contract_portfolio_id");
            });

            modelBuilder.Entity<PortfolioInvestor>(entity =>
            {
                entity.ToTable("portfolio_investor");

                entity.HasIndex(e => e.InvestorId, "portfolio_investor_investor_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "portfolio_investor_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InvestorId).HasColumnName("investor_id");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.HasOne(d => d.Investor)
                    .WithMany(p => p.PortfolioInvestors)
                    .HasForeignKey(d => d.InvestorId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("portfolio_investor_investor_id_foreign");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.PortfolioInvestors)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("portfolio_investor_portfolio_id_foreign");
            });

            modelBuilder.Entity<PortfolioParticipant>(entity =>
            {
                entity.ToTable("portfolio_participant");

                entity.HasIndex(e => e.ParticipantId, "portfolio_participant_participant_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "portfolio_participant_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ParticipantId).HasColumnName("participant_id");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.PortfolioParticipants)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("portfolio_participant_participant_id_foreign");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.PortfolioParticipants)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("portfolio_participant_portfolio_id_foreign");
            });

            modelBuilder.Entity<PortfolioProcedure>(entity =>
            {
                entity.ToTable("portfolio_procedure");

                entity.HasIndex(e => e.PortfolioId, "portfolio_procedure_portfolio_id_foreign_idx");

                entity.HasIndex(e => e.ProcedureId, "portfolio_procedure_procedure_id_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ProcedureId).HasColumnName("procedure_id");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.PortfolioProcedures)
                    .HasForeignKey(d => d.PortfolioId)
                    .HasConstraintName("portfolio_procedure_portfolio_id_foreign");

                entity.HasOne(d => d.Procedure)
                    .WithMany(p => p.PortfolioProcedures)
                    .HasForeignKey(d => d.ProcedureId)
                    .HasConstraintName("portfolio_procedure_procedure_id_foreign");
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.ToTable("prices");

                entity.HasIndex(e => e.ContractId, "prices_contract_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "prices_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Contract)
                    .HasMaxLength(45)
                    .HasColumnName("contract");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("process_date");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.HasOne(d => d.ContractNavigation)
                    .WithMany(p => p.Prices)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("prices_contract_id_foreign");

                entity.HasOne(d => d.PortfolioNavigation)
                    .WithMany(p => p.Prices)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("prices_portfolio_id_foreign");
            });

            modelBuilder.Entity<Procedure>(entity =>
            {
                entity.ToTable("procedures");

                entity.HasIndex(e => e.ContractId, "procedures_contract_id_foreign_idx");

                entity.HasIndex(e => e.PortfolioId, "procedures_portfolio_id_foreign_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AmountClaimed)
                    .HasPrecision(15, 2)
                    .HasColumnName("amount_claimed");

                entity.Property(e => e.Contract)
                    .HasMaxLength(45)
                    .HasColumnName("contract");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Court)
                    .HasMaxLength(100)
                    .HasColumnName("court");

                entity.Property(e => e.CourtCity)
                    .HasMaxLength(45)
                    .HasColumnName("court_city");

                entity.Property(e => e.CourtNumber)
                    .HasMaxLength(45)
                    .HasColumnName("court_number");

                entity.Property(e => e.CourtProcedureNumber)
                    .HasMaxLength(45)
                    .HasColumnName("court_procedure_number");

                entity.Property(e => e.CourtProvince)
                    .HasMaxLength(45)
                    .HasColumnName("court_province");

                entity.Property(e => e.JudicialCostAmount)
                    .HasPrecision(15, 2)
                    .HasColumnName("judicial_cost_amount");

                entity.Property(e => e.LastJudicialPhaseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_judicial_phase_date");

                entity.Property(e => e.LastJudicialPhaseDescription)
                    .HasMaxLength(45)
                    .HasColumnName("last_judicial_phase_description");

                entity.Property(e => e.LawFirm)
                    .HasMaxLength(45)
                    .HasColumnName("law_firm");

                entity.Property(e => e.LawsuitDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lawsuit_date");

                entity.Property(e => e.LaywerMail)
                    .HasMaxLength(45)
                    .HasColumnName("laywer_mail");

                entity.Property(e => e.LaywerName)
                    .HasMaxLength(45)
                    .HasColumnName("laywer_name");

                entity.Property(e => e.LaywerPhoneNumber)
                    .HasMaxLength(45)
                    .HasColumnName("laywer_phone_number");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(45)
                    .HasColumnName("portfolio");

                entity.Property(e => e.PortfolioId).HasColumnName("portfolio_id");

                entity.Property(e => e.ProcedureId)
                    .HasMaxLength(45)
                    .HasColumnName("procedure_id");

                entity.Property(e => e.ProcedureType)
                    .HasMaxLength(45)
                    .HasColumnName("procedure_type");

                entity.Property(e => e.ProceedingId)
                    .HasMaxLength(45)
                    .HasColumnName("proceeding_id");

                entity.Property(e => e.ProceedingType)
                    .HasMaxLength(45)
                    .HasColumnName("proceeding_type");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("process_date");

                entity.Property(e => e.SolicitorMail)
                    .HasMaxLength(45)
                    .HasColumnName("solicitor_mail");

                entity.Property(e => e.SolicitorName)
                    .HasMaxLength(45)
                    .HasColumnName("solicitor_name");

                entity.Property(e => e.SolicitorPhoneNumber)
                    .HasMaxLength(45)
                    .HasColumnName("solicitor_phone_number");

                entity.Property(e => e.Subportfolio)
                    .HasMaxLength(45)
                    .HasColumnName("subportfolio");

                entity.HasOne(d => d.ContractNavigation)
                    .WithMany(p => p.Procedures)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("procedures_contract_id_foreign");

                entity.HasOne(d => d.PortfolioNavigation)
                    .WithMany(p => p.Procedures)
                    .HasForeignKey(d => d.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("procedures_portfolio_id_foreign");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(45)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(45)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
