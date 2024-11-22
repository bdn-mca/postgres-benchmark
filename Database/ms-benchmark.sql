CREATE TABLE [dbo].[benchmark] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [CreatedOn]  SMALLDATETIME    NOT NULL,
    [DeletedOn]  SMALLDATETIME    NULL,
    [Name]       NVARCHAR (50)    NOT NULL,
    [Type]       INT              NOT NULL,
    [Properties] NVARCHAR (250)   NULL,
    [Uid]        UNIQUEIDENTIFIER NOT NULL,
    [Amount]     DECIMAL(28, 8)   NOT NULL,
    CONSTRAINT [PK_benchmark] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [benchmark_type_idx]
    ON [dbo].[benchmark]([Type] DESC);