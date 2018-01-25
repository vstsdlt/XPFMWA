CREATE TABLE [dbo].[EmployerUnit] (
    [EmployerId]          INT           NOT NULL,
    [EmployerUnitSeqNo]   INT           NOT NULL,
    [DoingBusinessAsName] VARCHAR (80)  NULL,
    [CountyCode]          VARCHAR (20)  NOT NULL,
    [StatusCode]          VARCHAR (20)  NOT NULL,
    [StatusDate]          DATETIME      NOT NULL,
    [CreateUserId]        VARCHAR (50)  NOT NULL,
    [CreateDateTime]      DATETIME      NOT NULL,
    [UpdateUserId]        VARCHAR (50)  NOT NULL,
    [UpdateDateTime]      DATETIME      NOT NULL,
    [FirstWageDate]       DATETIME      NULL,
    [UpdateNumber]        INT           NULL,
    [UpdateProcess]       VARCHAR (100) NULL,
    CONSTRAINT [PK_EmployerUnit] PRIMARY KEY CLUSTERED ([EmployerId] ASC, [EmployerUnitSeqNo] ASC),
    CONSTRAINT [FK_EmployerUnit_Employer_EmployerId] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employer] ([EmployerId])
);











