
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		WYP
-- Create date: 2018/07/21
-- Description:	ALMS-損益表
-- EXEC SP_ALMS_RP03 @BEG_DT='20180701',@END_DT='20180731',@Type=''  
-- =============================================

IF exists(select name FROM sysobjects 
          where name = 'SP_ALMS_RP03' AND type = 'P')
drop procedure SP_ALMS_RP03
go

CREATE PROCEDURE SP_ALMS_RP03 
       @ACC_NO_Digit  varchar(01)='4',   /* ACC_NO位數 */
       @Type  varchar(10),   /* 查詢條件-類別A、B(A 稅:1、2, B 財:2、3) DA03A*/
       @BEG_DT  varchar(10),   /* 查詢條件-起始日期 20180101 */
       @END_DT  varchar(10)   /* 查詢條件-結束日期 20180331 */       
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIS_DT nvarchar(20) =  LEFT(@BEG_DT,4)+'0131' /* 取得查詢區間年分的一月 */
	DECLARE @Range1 int=1,@Range2 int=3

	if(@Type='A')
		Begin
		--稅:1、2
			SET @Range1 =1
			SET @Range2 =2
		End
	Else if(@Type='B')
		Begin
		--財:2、3
			SET @Range1 =2
			SET @Range2 =3
		End

	CREATE TABLE #DB
	(
		ACC_NO nvarchar(5),    /*科目編號*/
		ACC_NM nvarchar(50),   /*科目名稱*/
		ACC_TP varchar(2),     /*1: 借/ 2: 貸*/
		CUR_MY numeric(19,4),  /*本期金額*/
		TOT_MY numeric(19,4),  /*累計金額*/
	)
	-----------------------------------------第一部分-------------------------------------
	-- 計算 [本期金額] 20180728 計算 本期金額 修改公式 兩欄位相減
	INSERT #DB
	(ACC_NO,ACC_NM,ACC_TP,CUR_MY,TOT_MY)
	select left(B.ACC_NO,@ACC_NO_Digit),B.ACC_NM,B.DA01A_ID,
		case when B.DA01A_ID = 1 then isnull(SUM(A.DEB_MY - A.CRE_MY),0)
		else isnull(SUM(A.CRE_MY),0) end as 本期金額, 0
    FROM [ALMS].[dbo].[TR01A] as A
	left join BA01A as B on A.BA01A_ID = B.BA01A_ID
	where left(B.ACC_NO,1) in (4,5,6,7,8,9) and A.TRN_DT between @BEG_DT and @END_DT and A.DA03A_ID between @Range1 and @Range2
	Group by left(B.ACC_NO,@ACC_NO_Digit), left(A.TRN_DT,4),B.ACC_NM,B.DA01A_ID

	-- 更新 [累積金額]
	UPDATE #DB
	SET #DB.TOT_MY = B.TOT_MY
	FROM (
		-- 計算 [累積金額]
		select left(B.ACC_NO,@ACC_NO_Digit) as ACC_NO,
			case when B.DA01A_ID = 1 then isnull(SUM(A.DEB_MY),0)
			else isnull(SUM(A.CRE_MY),0) end as TOT_MY
		FROM [ALMS].[dbo].[TR01A] as A
		left join BA01A as B on A.BA01A_ID = B.BA01A_ID
		where left(B.ACC_NO,1) in (4,5,6,7,8,9) and A.TRN_DT between @FIS_DT and @END_DT and A.DA03A_ID between @Range1 and @Range2
		Group by left(B.ACC_NO,@ACC_NO_Digit), left(A.TRN_DT,4),B.ACC_NM,B.DA01A_ID
	) as B
	WHERE #DB.ACC_NO = B.ACC_NO
	select *from #DB
	-----------------------------------------第二部分-------------------------------------
	CREATE TABLE #DB2
	(
		ACC_NO nvarchar(5),    /*科目編號*/
		ACC_NM nvarchar(50),   /*科目名稱*/
		ACC_TP varchar(2),     /*1: 借/ 2: 貸*/
		TOT_M1 numeric(19,4),  /*本期金額加總*/
		TOT_M2 numeric(19,4),  /*累計金額加總*/
	)

	INSERT #DB2
	select '','本期損益',ACC_TP,SUM(CUR_MY) as Y1, SUM(TOT_MY) as Y2
	from #DB
	group by ACC_TP

	SELECT t1.ACC_NO,t1.ACC_NM,(t2.TOT_M1 - t1.TOT_M1) AS CUR_MY, (t2.TOT_M2 - t1.TOT_M2) AS TOT_MY
	FROM #DB2 t1 CROSS JOIN
	     #DB2 t2
	WHERE t1.ACC_TP = '1' AND t2.ACC_TP = '2';

	--select ACC_NO,ACC_NM,CUR_MY,TOT_MY from #DB
END
GO
