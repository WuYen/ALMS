
GO

/****** Object:  UserDefinedFunction [dbo].[Get_VOU_NO]    Script Date: 2018/7/8 ¤U¤È 10:39:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[Get_VOU_NO]()
RETURNS nvarchar(20)
AS
BEGIN
  DECLARE @OrderID nvarchar(20)
  DECLARE @DT nvarchar(20)
  --SELECT @DT = convert(varchar(10),DateAdd(HH,8,GETUTCDATE()),112)--convert(varchar(10),getdate(),112)
  SELECT @DT = CAST((CAST(DATEPART(YEAR,GETDATE()) AS NUMERIC(4))-1911) AS VARCHAR(3)) + REPLACE(STR(datepart(mm, getdate()), 2, 0), ' ', '0')
  SELECT @OrderID= @DT + right('00' + ltrim(isnull(max(cast(right(VOU_NO, 3) as int)),0)+1), 3)
   from TR01A where left(VOU_NO, 5) = @DT
  RETURN @OrderID
END
GO


-- SELECT dbo.[Get_VOU_NO]()


 --SELECT CAST((CAST(DATEPART(YEAR,GETDATE()) AS NUMERIC(4))-1911) AS VARCHAR(3)) + REPLACE(STR(datepart(mm, getdate()), 2, 0), ' ', '0')

 --select CAST(DATEPART(MM,GETDATE()) AS VARCHAR(2))
 --SELECT REPLACE(STR(datepart(mm, getdate()), 2, 0), ' ', '0')