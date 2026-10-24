IF OBJECT_ID(N'dbo.ERP_RECORD_SEQ', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.ERP_RECORD_SEQ AS BIGINT START WITH 1 INCREMENT BY 1;');
GO

IF OBJECT_ID(N'dbo.ERP_DEPARTMENTAL_RETURN_NO_SEQ', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.ERP_DEPARTMENTAL_RETURN_NO_SEQ AS BIGINT START WITH 1 INCREMENT BY 1;');
GO

IF OBJECT_ID(N'dbo.DepartmentalReturnMaster', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DepartmentalReturnMaster
    (
        RecordId BIGINT NOT NULL CONSTRAINT PK_DepartmentalReturnMaster PRIMARY KEY
            CONSTRAINT DF_DepartmentalReturnMaster_RecordId DEFAULT (NEXT VALUE FOR dbo.ERP_RECORD_SEQ),
        ReturnNo NVARCHAR(50) NOT NULL,
        ReturnDate DATE NOT NULL,
        DeptFrom BIGINT NOT NULL,
        DeptTo BIGINT NOT NULL,
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_DepartmentalReturnMaster_Status DEFAULT ('PENDING'),
        RejectionReason NVARCHAR(500) NULL,
        CreatedBy NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_DepartmentalReturnMaster_CreatedAt DEFAULT (SYSUTCDATETIME()),
        ModifiedBy NVARCHAR(50) NULL,
        ModifiedAt DATETIME2(0) NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_DepartmentalReturnMaster_IsActive DEFAULT (1),
        CONSTRAINT UQ_DepartmentalReturnMaster_ReturnNo UNIQUE (ReturnNo)
    );
END;
GO

IF OBJECT_ID(N'dbo.DepartmentalReturnDetail', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DepartmentalReturnDetail
    (
        RecordId BIGINT NOT NULL CONSTRAINT PK_DepartmentalReturnDetail PRIMARY KEY
            CONSTRAINT DF_DepartmentalReturnDetail_RecordId DEFAULT (NEXT VALUE FOR dbo.ERP_RECORD_SEQ),
        ReturnRecordId BIGINT NOT NULL,
        Item BIGINT NOT NULL,
        Qty DECIMAL(18, 4) NOT NULL,
        CreatedBy NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_DepartmentalReturnDetail_CreatedAt DEFAULT (SYSUTCDATETIME()),
        ModifiedBy NVARCHAR(50) NULL,
        ModifiedAt DATETIME2(0) NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_DepartmentalReturnDetail_IsActive DEFAULT (1),
        CONSTRAINT FK_DepartmentalReturnDetail_Master FOREIGN KEY (ReturnRecordId)
            REFERENCES dbo.DepartmentalReturnMaster (RecordId),
        CONSTRAINT CK_DepartmentalReturnDetail_Qty CHECK (Qty > 0)
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_DEPARTMENTAL_RETURN_SETUP
    @p_user NVARCHAR(50),
    @p_action NVARCHAR(50),
    @p_formid NVARCHAR(50),
    @p_jsondata NVARCHAR(MAX),
    @p_record_id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @action NVARCHAR(50) = UPPER(@p_action);
    DECLARE @returnNo NVARCHAR(50);

    IF @action = 'GRIDDATA'
    BEGIN
        SELECT RecordId, ReturnNo, ReturnDate, DeptFrom, DeptTo, Status,
               CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
        FROM dbo.DepartmentalReturnMaster
        WHERE IsActive = 1
        ORDER BY ReturnDate DESC, RecordId DESC;
        RETURN;
    END;

    IF @action = 'SINGELRECORD'
    BEGIN
        SELECT RecordId, ReturnNo, ReturnDate, DeptFrom, DeptTo, Status, RejectionReason,
               CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, IsActive
        FROM dbo.DepartmentalReturnMaster
        WHERE RecordId = @p_record_id AND IsActive = 1;

        SELECT RecordId, ReturnRecordId, Item, Qty, CreatedBy, CreatedAt,
               ModifiedBy, ModifiedAt, IsActive
        FROM dbo.DepartmentalReturnDetail
        WHERE ReturnRecordId = @p_record_id AND IsActive = 1
        ORDER BY RecordId;
        RETURN;
    END;

    IF @action = 'GET_DEPARTMENTS'
    BEGIN
        SELECT DepartmentId, DepartmentName
        FROM dbo.Department
        WHERE IsActive = 1
        ORDER BY DepartmentName;
        RETURN;
    END;

    IF @action = 'GET_ITEMS'
    BEGIN
        SELECT ItemId, ItemCode, ItemName
        FROM dbo.Item
        WHERE IsActive = 1
        ORDER BY ItemName;
        RETURN;
    END;

    IF @action = 'GET_NEXT_NO'
    BEGIN
        SET @returnNo = CONCAT(
            N'DR-', YEAR(SYSUTCDATETIME()), N'-',
            RIGHT(REPLICATE(N'0', 8) +
                CONVERT(NVARCHAR(20), NEXT VALUE FOR dbo.ERP_DEPARTMENTAL_RETURN_NO_SEQ), 8));
        SELECT @returnNo AS ReturnNo;
        RETURN;
    END;

    IF @action IN ('INSERT', 'UPDATE')
    BEGIN
        DECLARE @recordId BIGINT = @p_record_id;
        DECLARE @returnDate DATE = TRY_CONVERT(DATE, JSON_VALUE(@p_jsondata, '$.ReturnDate'));
        DECLARE @deptFrom BIGINT = TRY_CONVERT(BIGINT, JSON_VALUE(@p_jsondata, '$.DeptFrom'));
        DECLARE @deptTo BIGINT = TRY_CONVERT(BIGINT, JSON_VALUE(@p_jsondata, '$.DeptTo'));
        DECLARE @status NVARCHAR(20) = COALESCE(JSON_VALUE(@p_jsondata, '$.Status'), N'PENDING');
        DECLARE @lines TABLE
        (
            LineNo INT IDENTITY(1,1) PRIMARY KEY,
            Item BIGINT NULL,
            Qty DECIMAL(18,4) NULL
        );
        DECLARE @lineCount INT;
        DECLARE @lineIndex INT = 1;
        DECLARE @item BIGINT;
        DECLARE @qty DECIMAL(18,4);

        INSERT INTO @lines (Item, Qty)
        SELECT TRY_CONVERT(BIGINT, Item), TRY_CONVERT(DECIMAL(18,4), Qty)
        FROM OPENJSON(@p_jsondata, '$.lines')
        WITH
        (
            Item NVARCHAR(50) '$.Item',
            Qty NVARCHAR(50) '$.Qty'
        );

        SELECT @lineCount = COUNT(*) FROM @lines;

        BEGIN TRY
            BEGIN TRANSACTION;

            IF @returnDate IS NULL OR @deptFrom IS NULL OR @deptTo IS NULL OR @lineCount = 0
                THROW 50001, 'ReturnDate, DeptFrom, DeptTo, and at least one detail line are required.', 1;

            IF EXISTS (SELECT 1 FROM @lines WHERE Item IS NULL OR Qty IS NULL OR Qty <= 0)
                THROW 50002, 'Every detail line must have a valid Item and positive Qty.', 1;

            IF @action = 'INSERT'
            BEGIN
                SET @returnNo = JSON_VALUE(@p_jsondata, '$.ReturnNo');
                IF NULLIF(LTRIM(RTRIM(@returnNo)), N'') IS NULL
                    SET @returnNo = CONCAT(
                        N'DR-', YEAR(@returnDate), N'-',
                        RIGHT(REPLICATE(N'0', 8) +
                            CONVERT(NVARCHAR(20), NEXT VALUE FOR dbo.ERP_DEPARTMENTAL_RETURN_NO_SEQ), 8));

                INSERT INTO dbo.DepartmentalReturnMaster
                    (ReturnNo, ReturnDate, DeptFrom, DeptTo, Status, CreatedBy)
                VALUES
                    (@returnNo, @returnDate, @deptFrom, @deptTo, @status, @p_user);

                SET @recordId = CONVERT(BIGINT, SCOPE_IDENTITY());
                IF @recordId IS NULL OR @recordId = 0
                    SELECT @recordId = RecordId
                    FROM dbo.DepartmentalReturnMaster
                    WHERE ReturnNo = @returnNo;
            END
            ELSE
            BEGIN
                IF @recordId IS NULL OR NOT EXISTS
                    (SELECT 1 FROM dbo.DepartmentalReturnMaster WHERE RecordId = @recordId AND IsActive = 1)
                    THROW 50003, 'Departmental return was not found.', 1;

                UPDATE dbo.DepartmentalReturnMaster
                SET ReturnNo = COALESCE(NULLIF(JSON_VALUE(@p_jsondata, '$.ReturnNo'), N''), ReturnNo),
                    ReturnDate = @returnDate,
                    DeptFrom = @deptFrom,
                    DeptTo = @deptTo,
                    ModifiedBy = @p_user,
                    ModifiedAt = SYSUTCDATETIME()
                WHERE RecordId = @recordId AND IsActive = 1;

                UPDATE dbo.DepartmentalReturnDetail
                SET IsActive = 0, ModifiedBy = @p_user, ModifiedAt = SYSUTCDATETIME()
                WHERE ReturnRecordId = @recordId AND IsActive = 1;
            END;

            WHILE @lineIndex <= @lineCount
            BEGIN
                SELECT @item = Item, @qty = Qty
                FROM @lines
                WHERE LineNo = @lineIndex;

                INSERT INTO dbo.DepartmentalReturnDetail
                    (ReturnRecordId, Item, Qty, CreatedBy)
                VALUES
                    (@recordId, @item, @qty, @p_user);

                SET @lineIndex += 1;
            END;

            COMMIT TRANSACTION;
            SELECT N'Saved successfully.' AS Message, 1 AS isSuccess;
        END TRY
        BEGIN CATCH
            IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
            SELECT ERROR_MESSAGE() AS Message, 0 AS isSuccess;
        END CATCH;
        RETURN;
    END;

    IF @action = 'APPROVE'
    BEGIN
        UPDATE dbo.DepartmentalReturnMaster
        SET Status = N'APPROVED', ModifiedBy = @p_user, ModifiedAt = SYSUTCDATETIME()
        WHERE RecordId = @p_record_id AND IsActive = 1 AND Status = N'PENDING';

        IF @@ROWCOUNT = 0
            SELECT N'Return was not found or is not pending approval.' AS Message, 0 AS isSuccess;
        ELSE
            SELECT N'Saved successfully.' AS Message, 1 AS isSuccess;
        RETURN;
    END;

    IF @action = 'REJECT'
    BEGIN
        UPDATE dbo.DepartmentalReturnMaster
        SET Status = N'REJECTED',
            RejectionReason = JSON_VALUE(@p_jsondata, '$.reason'),
            ModifiedBy = @p_user,
            ModifiedAt = SYSUTCDATETIME()
        WHERE RecordId = @p_record_id AND IsActive = 1 AND Status = N'PENDING';

        IF @@ROWCOUNT = 0
            SELECT N'Return was not found or is not pending approval.' AS Message, 0 AS isSuccess;
        ELSE
            SELECT N'Saved successfully.' AS Message, 1 AS isSuccess;
        RETURN;
    END;

    IF @action = 'DELETE'
    BEGIN
        BEGIN TRY
            BEGIN TRANSACTION;

            IF NOT EXISTS
                (SELECT 1 FROM dbo.DepartmentalReturnMaster WHERE RecordId = @p_record_id AND IsActive = 1)
                THROW 50004, 'Departmental return was not found.', 1;

            UPDATE dbo.DepartmentalReturnDetail
            SET IsActive = 0, ModifiedBy = @p_user, ModifiedAt = SYSUTCDATETIME()
            WHERE ReturnRecordId = @p_record_id AND IsActive = 1;

            UPDATE dbo.DepartmentalReturnMaster
            SET IsActive = 0, ModifiedBy = @p_user, ModifiedAt = SYSUTCDATETIME()
            WHERE RecordId = @p_record_id AND IsActive = 1;

            COMMIT TRANSACTION;
            SELECT N'Saved successfully.' AS Message, 1 AS isSuccess;
        END TRY
        BEGIN CATCH
            IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
            SELECT ERROR_MESSAGE() AS Message, 0 AS isSuccess;
        END CATCH;
        RETURN;
    END;

    SELECT N'Unsupported action.' AS Message, 0 AS isSuccess;
END;
GO
