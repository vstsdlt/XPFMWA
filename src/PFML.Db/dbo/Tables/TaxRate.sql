CREATE TABLE [dbo].[TaxRate] (
    [TaxRateId]          INT            IDENTITY (1, 1) NOT NULL,
    [EffectiveBeginDate] DATETIME       NOT NULL,
    [EffectiveEndDate]   DATETIME       NULL,
    [TaxRate]            DECIMAL (6, 3) NOT NULL,
    [CreateUserId]       VARCHAR (50)   NOT NULL,
    [CreateDateTime]     DATETIME       NOT NULL,
    [UpdateUserId]       VARCHAR (50)   NOT NULL,
    [UpdateDateTime]     DATETIME       NOT NULL,
    [UpdateNumber]       INT            NULL,
    [UpdateProcess]      VARCHAR (100)  NULL,
    CONSTRAINT [PK_TaxRate] PRIMARY KEY CLUSTERED ([TaxRateId] ASC)
);









