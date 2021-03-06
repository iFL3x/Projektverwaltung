USE [master]
GO
/****** Object:  Database [Projektverwaltung]    Script Date: 07.03.2018 16:26:07 ******/
CREATE DATABASE [Projektverwaltung]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Projektverwaltung', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Projektverwaltung.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Projektverwaltung_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Projektverwaltung_log.ldf' , SIZE = 2368KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Projektverwaltung] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Projektverwaltung].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Projektverwaltung] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Projektverwaltung] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Projektverwaltung] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Projektverwaltung] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Projektverwaltung] SET ARITHABORT OFF 
GO
ALTER DATABASE [Projektverwaltung] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Projektverwaltung] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Projektverwaltung] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Projektverwaltung] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Projektverwaltung] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Projektverwaltung] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Projektverwaltung] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Projektverwaltung] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Projektverwaltung] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Projektverwaltung] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Projektverwaltung] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Projektverwaltung] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Projektverwaltung] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Projektverwaltung] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Projektverwaltung] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Projektverwaltung] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Projektverwaltung] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Projektverwaltung] SET RECOVERY FULL 
GO
ALTER DATABASE [Projektverwaltung] SET  MULTI_USER 
GO
ALTER DATABASE [Projektverwaltung] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Projektverwaltung] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Projektverwaltung] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Projektverwaltung] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Projektverwaltung] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Projektverwaltung] SET QUERY_STORE = OFF
GO
USE [Projektverwaltung]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Projektverwaltung]
GO
/****** Object:  Table [dbo].[Aktivitaet]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aktivitaet](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[status] [int] NULL,
	[projektphase_id] [int] NOT NULL,
	[startdatum_geplant] [datetime] NULL,
	[enddatum_geplant] [datetime] NULL,
	[startdatum_effektiv] [datetime] NULL,
	[enddatum_effektiv] [datetime] NULL,
	[erwartete_kosten] [float] NULL,
	[effektive_kosten] [float] NULL,
	[fortschritt] [float] NULL,
	[dokumente_link] [varchar](250) NULL,
	[verantwortlicher_id] [int] NULL,
	[abweichungen] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AktivitaetMitarbeiter]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AktivitaetMitarbeiter](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[aktivitaet_id] [int] NOT NULL,
	[mitarbeiter_id] [int] NOT NULL,
	[budgetierte_zeit] [int] NULL,
	[effektive_zeit] [int] NULL,
	[abweichungsgrund] [varchar](150) NULL,
	[funktion] [varchar](150) NULL,
	[kostenart_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dokument]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dokument](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[projekt_id] [int] NULL,
	[projektphase_id] [int] NULL,
	[aktivitaet_id] [int] NULL,
	[pfad] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Funktion]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Funktion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kostenart]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kostenart](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meilenstein]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meilenstein](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](60) NULL,
	[projektphase_id] [int] NOT NULL,
	[beschreibung] [varchar](500) NULL,
	[status] [int] NULL,
	[nicht_loeschbar] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mitarbeiter]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mitarbeiter](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[vorname] [varchar](50) NOT NULL,
	[abteilung] [varchar](50) NOT NULL,
	[pensum] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MitarbeiterFunktion]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MitarbeiterFunktion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[mitarbeiter_id] [int] NOT NULL,
	[funktion_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projekt]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projekt](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](60) NOT NULL,
	[beschreibung] [varchar](500) NULL,
	[status] [int] NOT NULL,
	[prioritaet] [int] NOT NULL,
	[startdatum_geplant] [datetime] NOT NULL,
	[enddatum_geplant] [datetime] NOT NULL,
	[startdatum_effektiv] [datetime] NULL,
	[enddatum_effektiv] [datetime] NULL,
	[fortschritt] [float] NULL,
	[dokument_link] [varchar](250) NULL,
	[projektleiter_id] [int] NOT NULL,
	[vorgehensmodell_id] [int] NOT NULL,
	[bewilligungsdatum] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjektPhase]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjektPhase](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[beschreibung] [varchar](250) NULL,
	[status] [int] NULL,
	[startdatum_geplant] [datetime] NULL,
	[enddatum_geplant] [datetime] NULL,
	[startdatum_effektiv] [datetime] NULL,
	[enddatum_effektiv] [datetime] NULL,
	[fortschritt] [float] NULL,
	[freigabe_datum] [datetime] NULL,
	[freigabe_visum] [varchar](40) NULL,
	[dokumente_link] [varchar](250) NULL,
	[reviewdatum_geplant] [datetime] NULL,
	[reviewdatum_effektiv] [datetime] NULL,
	[projekt_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vorgehensmodell]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vorgehensmodell](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](60) NOT NULL,
	[status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VorgehensmodellPhase]    Script Date: 07.03.2018 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VorgehensmodellPhase](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[beschreibung] [varchar](250) NULL,
	[vorgehensmodell_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Aktivitaet]  WITH CHECK ADD  CONSTRAINT [FK_Aktivitaet_Mitarbeiter] FOREIGN KEY([verantwortlicher_id])
REFERENCES [dbo].[Mitarbeiter] ([id])
GO
ALTER TABLE [dbo].[Aktivitaet] CHECK CONSTRAINT [FK_Aktivitaet_Mitarbeiter]
GO
ALTER TABLE [dbo].[Aktivitaet]  WITH CHECK ADD  CONSTRAINT [FK_Aktivitaet_ProjektPhase] FOREIGN KEY([projektphase_id])
REFERENCES [dbo].[ProjektPhase] ([id])
GO
ALTER TABLE [dbo].[Aktivitaet] CHECK CONSTRAINT [FK_Aktivitaet_ProjektPhase]
GO
ALTER TABLE [dbo].[AktivitaetMitarbeiter]  WITH CHECK ADD  CONSTRAINT [FK_AktivitaetMitarbeiter_Aktivitaet] FOREIGN KEY([aktivitaet_id])
REFERENCES [dbo].[Aktivitaet] ([id])
GO
ALTER TABLE [dbo].[AktivitaetMitarbeiter] CHECK CONSTRAINT [FK_AktivitaetMitarbeiter_Aktivitaet]
GO
ALTER TABLE [dbo].[AktivitaetMitarbeiter]  WITH CHECK ADD  CONSTRAINT [FK_AktivitaetMitarbeiter_Kostenart] FOREIGN KEY([kostenart_id])
REFERENCES [dbo].[Kostenart] ([id])
GO
ALTER TABLE [dbo].[AktivitaetMitarbeiter] CHECK CONSTRAINT [FK_AktivitaetMitarbeiter_Kostenart]
GO
ALTER TABLE [dbo].[AktivitaetMitarbeiter]  WITH CHECK ADD  CONSTRAINT [FK_AktivitaetMitarbeiter_Mitarbeiter] FOREIGN KEY([mitarbeiter_id])
REFERENCES [dbo].[Mitarbeiter] ([id])
GO
ALTER TABLE [dbo].[AktivitaetMitarbeiter] CHECK CONSTRAINT [FK_AktivitaetMitarbeiter_Mitarbeiter]
GO
ALTER TABLE [dbo].[Dokument]  WITH CHECK ADD  CONSTRAINT [FK_Dokument_Aktivitaet] FOREIGN KEY([aktivitaet_id])
REFERENCES [dbo].[Aktivitaet] ([id])
GO
ALTER TABLE [dbo].[Dokument] CHECK CONSTRAINT [FK_Dokument_Aktivitaet]
GO
ALTER TABLE [dbo].[Dokument]  WITH CHECK ADD  CONSTRAINT [FK_Dokument_Projekt] FOREIGN KEY([projekt_id])
REFERENCES [dbo].[Projekt] ([id])
GO
ALTER TABLE [dbo].[Dokument] CHECK CONSTRAINT [FK_Dokument_Projekt]
GO
ALTER TABLE [dbo].[Dokument]  WITH CHECK ADD  CONSTRAINT [FK_Dokument_ProjektPhase] FOREIGN KEY([projektphase_id])
REFERENCES [dbo].[ProjektPhase] ([id])
GO
ALTER TABLE [dbo].[Dokument] CHECK CONSTRAINT [FK_Dokument_ProjektPhase]
GO
ALTER TABLE [dbo].[Meilenstein]  WITH CHECK ADD  CONSTRAINT [FK_Meilenstein_Phase] FOREIGN KEY([projektphase_id])
REFERENCES [dbo].[ProjektPhase] ([id])
GO
ALTER TABLE [dbo].[Meilenstein] CHECK CONSTRAINT [FK_Meilenstein_Phase]
GO
ALTER TABLE [dbo].[MitarbeiterFunktion]  WITH CHECK ADD  CONSTRAINT [FK_MitarbeiterFunktion_Funktion] FOREIGN KEY([funktion_id])
REFERENCES [dbo].[Funktion] ([id])
GO
ALTER TABLE [dbo].[MitarbeiterFunktion] CHECK CONSTRAINT [FK_MitarbeiterFunktion_Funktion]
GO
ALTER TABLE [dbo].[MitarbeiterFunktion]  WITH CHECK ADD  CONSTRAINT [FK_MitarbeiterFunktion_Mitarbeiter] FOREIGN KEY([mitarbeiter_id])
REFERENCES [dbo].[Mitarbeiter] ([id])
GO
ALTER TABLE [dbo].[MitarbeiterFunktion] CHECK CONSTRAINT [FK_MitarbeiterFunktion_Mitarbeiter]
GO
ALTER TABLE [dbo].[Projekt]  WITH CHECK ADD  CONSTRAINT [FK_Projekt_Mitarbeiter] FOREIGN KEY([projektleiter_id])
REFERENCES [dbo].[Mitarbeiter] ([id])
GO
ALTER TABLE [dbo].[Projekt] CHECK CONSTRAINT [FK_Projekt_Mitarbeiter]
GO
ALTER TABLE [dbo].[Projekt]  WITH CHECK ADD  CONSTRAINT [FK_Projekt_Vorgehensmodell] FOREIGN KEY([vorgehensmodell_id])
REFERENCES [dbo].[Vorgehensmodell] ([id])
GO
ALTER TABLE [dbo].[Projekt] CHECK CONSTRAINT [FK_Projekt_Vorgehensmodell]
GO
ALTER TABLE [dbo].[ProjektPhase]  WITH CHECK ADD  CONSTRAINT [FK_Phase_Projekt] FOREIGN KEY([projekt_id])
REFERENCES [dbo].[Projekt] ([id])
GO
ALTER TABLE [dbo].[ProjektPhase] CHECK CONSTRAINT [FK_Phase_Projekt]
GO
ALTER TABLE [dbo].[VorgehensmodellPhase]  WITH CHECK ADD  CONSTRAINT [FK_VorgehensmodellPhase_Vorgehensmodell] FOREIGN KEY([vorgehensmodell_id])
REFERENCES [dbo].[Vorgehensmodell] ([id])
GO
ALTER TABLE [dbo].[VorgehensmodellPhase] CHECK CONSTRAINT [FK_VorgehensmodellPhase_Vorgehensmodell]
GO
USE [master]
GO
ALTER DATABASE [Projektverwaltung] SET  READ_WRITE 
GO
