-- Create the OrleansStorage table if it does not exist
IF OBJECT_ID(N'[OrleansStorage]', 'U') IS NULL
BEGIN
    CREATE TABLE OrleansStorage
    (
        -- Hash values for identifying grain and type
        GrainIdHash                INT NOT NULL,
        GrainIdN0                  BIGINT NOT NULL,
        GrainIdN1                  BIGINT NOT NULL,
        GrainTypeHash              INT NOT NULL,
        GrainTypeString            NVARCHAR(512) NOT NULL,
        GrainIdExtensionString    NVARCHAR(512) NULL,
        ServiceId                  NVARCHAR(150) NOT NULL,

        -- Payload data
        PayloadBinary              VARBINARY(MAX) NULL,

        -- Informational fields
        ModifiedOn                 DATETIME2(3) NOT NULL,

        -- Version number for concurrency control
        Version                    INT NULL,

        -- Primary key is conceptual; handled by hashed fields for performance
        -- NOTE: The primary key isn't physically defined due to index constraints
        -- but the combination of hash fields should ensure uniqueness.
    );

    -- Create a non-clustered index on hash fields for efficient lookups
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrleansStorage' AND object_id = OBJECT_ID('OrleansStorage'))
    BEGIN
        CREATE NONCLUSTERED INDEX IX_OrleansStorage ON OrleansStorage (GrainIdHash, GrainTypeHash);
    END

    -- Disable lock escalation to avoid locking the entire table during updates
    ALTER TABLE OrleansStorage SET (LOCK_ESCALATION = DISABLE);

    -- Apply data compression if supported (Enterprise feature)
    IF EXISTS (SELECT 1 FROM sys.dm_db_persisted_sku_features WHERE feature_id = 100)
    BEGIN
        ALTER TABLE OrleansStorage REBUILD PARTITION = ALL WITH (DATA_COMPRESSION = PAGE);
    END
END

-- Create the OrleansQuery table if it does not exist
IF OBJECT_ID(N'[OrleansQuery]', 'U') IS NULL
BEGIN
    CREATE TABLE OrleansQuery
    (
        QueryKey NVARCHAR(100) PRIMARY KEY,
        QueryText NVARCHAR(MAX) NOT NULL
    );
END

-- Insert a query for writing to storage, if not already present
IF NOT EXISTS (SELECT 1 FROM OrleansQuery WHERE QueryKey = 'WriteToStorageKey')
BEGIN
    INSERT INTO OrleansQuery (QueryKey, QueryText)
    SELECT
        'WriteToStorageKey',
        '-- When Orleans is running normally, there should be only one grain with the given ID and type combination.
        -- Updates are made serially to ensure correctness, even in split-brain scenarios.
        -- This query handles both updates and inserts, resolving conflicts with locks where necessary.
        BEGIN TRANSACTION;
        SET XACT_ABORT, NOCOUNT ON;

        DECLARE @NewGrainStateVersion AS INT;

        -- Update existing state if version matches
        IF @GrainStateVersion IS NOT NULL
        BEGIN
            UPDATE OrleansStorage
            SET
                PayloadBinary = @PayloadBinary,
                ModifiedOn = GETUTCDATE(),
                Version = Version + 1
            WHERE
                GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
                AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
                AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
                AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
                AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
                AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
                AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
                AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL;

            SET @NewGrainStateVersion = Version + 1;
        END
        -- Insert new state if not exists
        IF @GrainStateVersion IS NULL
        BEGIN
            INSERT INTO OrleansStorage
            (
                GrainIdHash,
                GrainIdN0,
                GrainIdN1,
                GrainTypeHash,
                GrainTypeString,
                GrainIdExtensionString,
                ServiceId,
                PayloadBinary,
                ModifiedOn,
                Version
            )
            SELECT
                @GrainIdHash,
                @GrainIdN0,
                @GrainIdN1,
                @GrainTypeHash,
                @GrainTypeString,
                @GrainIdExtensionString,
                @ServiceId,
                @PayloadBinary,
                GETUTCDATE(),
                1
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM OrleansStorage
                WHERE
                    GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
                    AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
                    AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
                    AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
                    AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
                    AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
                    AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
            );

            SET @NewGrainStateVersion = 1;
        END

        SELECT @NewGrainStateVersion AS NewGrainStateVersion;
        COMMIT TRANSACTION;'
END

-- Insert a query for clearing storage, if not already present
IF NOT EXISTS (SELECT 1 FROM OrleansQuery WHERE QueryKey = 'ClearStorageKey')
BEGIN
    INSERT INTO OrleansQuery (QueryKey, QueryText)
    SELECT
        'ClearStorageKey',
        'BEGIN TRANSACTION;
        SET XACT_ABORT, NOCOUNT ON;
        DECLARE @NewGrainStateVersion AS INT;

        UPDATE OrleansStorage
        SET
            PayloadBinary = NULL,
            ModifiedOn = GETUTCDATE(),
            Version = Version + 1
        WHERE
            GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
            AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
            AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
            AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
            AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
            AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
            AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
            AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL;

        SELECT @NewGrainStateVersion;
        COMMIT TRANSACTION;'
END

-- Insert a query for reading from storage, if not already present
IF NOT EXISTS (SELECT 1 FROM OrleansQuery WHERE QueryKey = 'ReadFromStorageKey')
BEGIN
    INSERT INTO OrleansQuery (QueryKey, QueryText)
    SELECT
        'ReadFromStorageKey',
        '-- This query retrieves the payload binary and version for a given grain ID and type.
        -- It is optimized to ensure efficient access to a single result row.
        SELECT
            PayloadBinary,
            Version
        FROM
            OrleansStorage
        WHERE
            GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
            AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
            AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
            AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
            AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
            AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
            AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
            OPTION (FAST 1, OPTIMIZE FOR (@GrainIdHash UNKNOWN, @GrainTypeHash UNKNOWN));'
END
