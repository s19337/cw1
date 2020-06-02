IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Medicaments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    CONSTRAINT [PK_Medicaments] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200602155337_Nazwa', N'3.1.4');

GO

CREATE TABLE [Persons] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NULL,
    [LasttName] nvarchar(max) NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200602155554_AddPerson', N'3.1.4');

GO

DROP TABLE [Persons];

GO

ALTER TABLE [Medicaments] DROP CONSTRAINT [PK_Medicaments];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Medicaments]') AND [c].[name] = N'Id');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Medicaments] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Medicaments] DROP COLUMN [Id];

GO

ALTER TABLE [Medicaments] ADD [IdMedicament] int NOT NULL IDENTITY;

GO

ALTER TABLE [Medicaments] ADD CONSTRAINT [PK_Medicaments] PRIMARY KEY ([IdMedicament]);

GO

CREATE TABLE [Doctors] (
    [IdDoctor] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY ([IdDoctor])
);

GO

CREATE TABLE [Patients] (
    [IdPatient] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY ([IdPatient])
);

GO

CREATE TABLE [Prescriptions] (
    [IdPrescription] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [IdPatient1] int NULL,
    [IdDoctor1] int NULL,
    CONSTRAINT [PK_Prescriptions] PRIMARY KEY ([IdPrescription]),
    CONSTRAINT [FK_Prescriptions_Doctors_IdDoctor1] FOREIGN KEY ([IdDoctor1]) REFERENCES [Doctors] ([IdDoctor]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescriptions_Patients_IdPatient1] FOREIGN KEY ([IdPatient1]) REFERENCES [Patients] ([IdPatient]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Prescriptions_IdDoctor1] ON [Prescriptions] ([IdDoctor1]);

GO

CREATE INDEX [IX_Prescriptions_IdPatient1] ON [Prescriptions] ([IdPatient1]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200602162233_AllTablels', N'3.1.4');

GO

