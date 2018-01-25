CREATE TABLE [dbo].[Employer] (
    [EmployerId]                INT           IDENTITY (1, 1) NOT NULL,
    [FEIN]                      INT           NULL,
    [EntityName]                VARCHAR (80)  NOT NULL,
    [EntityTypeCode]            VARCHAR (20)  NOT NULL,
    [BusinessTypeCode]          VARCHAR (20)  NOT NULL,
    [UserName]                  VARCHAR (10)  NOT NULL,
    [IsIndividualContractor]    BIT           NOT NULL,
    [IsAcquired]                BIT           NOT NULL,
    [IsPresentInMultipleLoc]    BIT           NOT NULL,
    [IsProfessionalEmployerOrg] BIT           NOT NULL,
    [ReportMethodCode]          VARCHAR (20)  NOT NULL,
    [RegistrationDate]          DATETIME      NOT NULL,
    [StatusCode]                VARCHAR (20)  NOT NULL,
    [StatusDate]                DATETIME      NOT NULL,
    [IsServiceBegin]            BIT           NULL,
    [IsExemptUnderIRS501C3]     BIT           NOT NULL,
    [IsClientOfPEO]             BIT           NOT NULL,
    [SubjectivityCode]          VARCHAR (20)  NOT NULL,
    [IsApplyingForREIM]         BIT           NULL,
    [NoOfLocations]             INT           NULL,
    [LiabilityIncurredDate]     DATETIME      NULL,
    [LiabilityDate]             DATETIME      NULL,
    [CreateUserId]              VARCHAR (50)  NOT NULL,
    [CreateDateTime]            DATETIME      NOT NULL,
    [UpdateUserId]              VARCHAR (50)  NOT NULL,
    [UpdateDateTime]            DATETIME      NOT NULL,
    [NoOfEmployeesPaid]         INT           NULL,
    [ServiceBeginDate]          DATETIME      NULL,
    [UpdateNumber]              INT           NULL,
    [UpdateProcess]             VARCHAR (100) NULL,
    [HasTelecommuters]          BIT           NULL,
    [HasPhysicalLocation]       BIT           NULL,
    CONSTRAINT [PK_Employer] PRIMARY KEY CLUSTERED ([EmployerId] ASC)
);















