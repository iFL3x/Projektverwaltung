USE [Projektverwaltung]
GO
SET IDENTITY_INSERT [dbo].[Mitarbeiter] ON 
INSERT [dbo].[Mitarbeiter] ([id], [name], [vorname], [abteilung], [pensum]) VALUES (1, N'König', N'Fabrice', N'Softwareentwicklung', 80)
INSERT [dbo].[Mitarbeiter] ([id], [name], [vorname], [abteilung], [pensum]) VALUES (2, N'Trump', N'Donald', N'Präsident', 100)
INSERT [dbo].[Mitarbeiter] ([id], [name], [vorname], [abteilung], [pensum]) VALUES (3, N'Smith', N'John', N'Marketing', 80)
INSERT [dbo].[Mitarbeiter] ([id], [name], [vorname], [abteilung], [pensum]) VALUES (4, N'Muster', N'Max', N'Controlling', 100)
SET IDENTITY_INSERT [dbo].[Mitarbeiter] OFF

SET IDENTITY_INSERT [dbo].[Kostenart] ON 
INSERT [dbo].[Kostenart] ([id], [name]) VALUES (1, N'Hardware')
INSERT [dbo].[Kostenart] ([id], [name]) VALUES (2, N'Software')
INSERT [dbo].[Kostenart] ([id], [name]) VALUES (3, N'Dienstleistungen')
INSERT [dbo].[Kostenart] ([id], [name]) VALUES (4, N'Wartung')
SET IDENTITY_INSERT [dbo].[Kostenart] OFF

SET IDENTITY_INSERT [dbo].[Funktion] ON 
INSERT [dbo].[Funktion] ([id], [name]) VALUES (1, N'Development')
INSERT [dbo].[Funktion] ([id], [name]) VALUES (2, N'Projektleitung')
INSERT [dbo].[Funktion] ([id], [name]) VALUES (3, N'Vorstand')
INSERT [dbo].[Funktion] ([id], [name]) VALUES (4, N'Geschäftsleitung')
INSERT [dbo].[Funktion] ([id], [name]) VALUES (5, N'Design')
INSERT [dbo].[Funktion] ([id], [name]) VALUES (6, N'Marketing')
SET IDENTITY_INSERT [dbo].[Funktion] OFF

SET IDENTITY_INSERT [dbo].[MitarbeiterFunktion] ON 
INSERT [dbo].[MitarbeiterFunktion] ([id], [mitarbeiter_id], [funktion_id]) VALUES (3, 1, 1)
INSERT [dbo].[MitarbeiterFunktion] ([id], [mitarbeiter_id], [funktion_id]) VALUES (6, 2, 2)
INSERT [dbo].[MitarbeiterFunktion] ([id], [mitarbeiter_id], [funktion_id]) VALUES (7, 2, 3)
INSERT [dbo].[MitarbeiterFunktion] ([id], [mitarbeiter_id], [funktion_id]) VALUES (8, 2, 4)
INSERT [dbo].[MitarbeiterFunktion] ([id], [mitarbeiter_id], [funktion_id]) VALUES (9, 3, 6)
INSERT [dbo].[MitarbeiterFunktion] ([id], [mitarbeiter_id], [funktion_id]) VALUES (10, 3, 5)
INSERT [dbo].[MitarbeiterFunktion] ([id], [mitarbeiter_id], [funktion_id]) VALUES (11, 4, 2)
SET IDENTITY_INSERT [dbo].[MitarbeiterFunktion] OFF


