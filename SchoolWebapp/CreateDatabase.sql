CREATE TABLE [dbo].[Activity] (
    [activityId]  INT            IDENTITY (1, 1) NOT NULL,
    [classId]     INT            NOT NULL,
    [Title]       NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [deadline]    DATETIME2 (7)  NOT NULL,
    [ispublic]    BIT            NOT NULL
);


CREATE TABLE [dbo].[ActivityStudent] (
    [activityStudentId]  INT            IDENTITY (1, 1) NOT NULL,
    [studentId]          INT            NOT NULL,
    [activityId]         INT            NOT NULL,
    [calification]       REAL           NOT NULL,
    [isSubmitted]        BIT            NOT NULL,
    [isRated]            BIT            NOT NULL,
    [canBeSubmittedLate] BIT            NOT NULL,
    [isLate]             BIT            NOT NULL,
    [commentary]         NVARCHAR (MAX) NULL,
    [submitDate]         DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ActivityStudent] PRIMARY KEY CLUSTERED ([activityStudentId] ASC),
    FOREIGN KEY ([studentId]) REFERENCES [dbo].[Student] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([activityId]) REFERENCES [dbo].[Activity] ([activityId]) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]     NVARCHAR (450) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
    ON [dbo].[AspNetRoleClaims]([RoleId] ASC);

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]               NVARCHAR (450) NOT NULL,
    [Name]             NVARCHAR (256) NULL,
    [NormalizedName]   NVARCHAR (256) NULL,
    [ConcurrencyStamp] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL);


CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (450) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider]       NVARCHAR (450) NOT NULL,
    [ProviderKey]         NVARCHAR (450) NOT NULL,
    [ProviderDisplayName] NVARCHAR (MAX) NULL,
    [UserId]              NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (450) NOT NULL,
    [RoleId] NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);

CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (450)     NOT NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);

CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId]        NVARCHAR (450) NOT NULL,
    [LoginProvider] NVARCHAR (450) NOT NULL,
    [Name]          NVARCHAR (450) NOT NULL,
    [Value]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Class] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [teacherId] INT            NOT NULL,
    [SubjectId] INT            NOT NULL,
    CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([teacherId]) REFERENCES [dbo].[Teacher] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subject] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [dbo].[Event] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UserId]    NVARCHAR (450) NOT NULL,
    [Title]     NVARCHAR (MAX) NOT NULL,
    [Start]     DATETIME       NOT NULL,
    [End]       DATETIME       NULL,
    [AllDay]    BIT            NULL,
    [Color]     NVARCHAR (50)  NULL,
    [TextColor] NVARCHAR (50)  NULL,
    [ClassName] NVARCHAR (50)  NULL,
    [URL]       NVARCHAR (450) NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [dbo].[Student] (
    [id]         INT            IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (MAX) NOT NULL,
    [lastName]   NVARCHAR (MAX) NOT NULL,
    [address]    NVARCHAR (MAX) NOT NULL,
    [age]        INT            NOT NULL,
    [schoolYear] INT            NOT NULL,
    [section]    NVARCHAR (1)   NOT NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[StudentClass] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [studentId] INT NOT NULL,
    [classId]   INT NOT NULL,
    CONSTRAINT [PK_StudentClass] PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([classId]) REFERENCES [dbo].[Class] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([studentId]) REFERENCES [dbo].[Student] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [dbo].[Subject] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (MAX) NOT NULL,
    [schoolYear] INT            NOT NULL,
    CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Teacher] (
    [id]       INT            IDENTITY (1, 1) NOT NULL,
    [name]     NVARCHAR (MAX) NOT NULL,
    [lastName] NVARCHAR (MAX) NOT NULL,
    [age]      INT            NOT NULL,
    [email]    NVARCHAR (MAX) NOT NULL,
    [phone]    NVARCHAR (MAX) NOT NULL,
    [address]  NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED ([id] ASC)
);

