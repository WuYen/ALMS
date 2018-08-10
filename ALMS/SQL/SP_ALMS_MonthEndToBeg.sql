
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		WYP
-- Create date: 2018/07/26
-- Description:	ALMS-試算表
-- EXEC SP_ALMS_MonthEndToBeg @BEG_DT='20180701'
-- =============================================

IF exists(select name FROM sysobjects 
          where name = 'SP_ALMS_MonthEndToBeg' AND type = 'P')
drop procedure SP_ALMS_MonthEndToBeg
go

CREATE PROCEDURE SP_ALMS_MonthEndToBeg 
       @ACC_NO_Digit  varchar(01)='7',   /* ACC_NO位數 */
       @BEG_DT  varchar(8)   /* 查詢條件-起始日期 20180701 */
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TEMP_DT datetime = DATEADD(MM,-1,convert(datetime,@BEG_DT,111))
	DECLARE @PRE_DT varchar(8) = LEFT(CONVERT(varchar, @TEMP_DT,112),6) /* 期初時間 */

	CREATE TABLE #DB
	(
		TRN_DT varchar(10),     /*時間*/
		ACC_NO nvarchar(7),     /*科目編號*/
		BEG_DEB numeric(19,4),  /*期初借方*/
		BEG_CRE numeric(19,4),  /*期初贷方*/
		CUR_DEB numeric(19,4),  /*本期发生借方*/
		CUR_CRE numeric(19,4),  /*本期发生贷方*/
		END_DEB numeric(19,4),  /*期末借方*/
		END_CRE numeric(19,4)   /*期末贷方*/
	)

	-- 建立 [本月借貸]
	INSERT #DB
	(TRN_DT,ACC_NO,BEG_DEB,BEG_CRE,CUR_DEB,CUR_CRE,END_DEB,END_CRE)
	select left(A.TRN_DT,6),left(B.ACC_NO,4),0,0,isnull(SUM(A.DEB_MY),0) as 本期借,isnull(SUM(A.CRE_MY),0) as 本期貸,0,0
    FROM [ALMS].[dbo].[TR01A] as A
	left join BA01A as B on A.BA01A_ID = B.BA01A_ID
	where left(A.TRN_DT,6) = left(@BEG_DT,6)
	Group by left(B.ACC_NO,4), left(A.TRN_DT,6)

	-- 更新 [期初借方]
	UPDATE #DB SET BEG_DEB = B.DEB_MY
	FROM TR01X as B WHERE left(#DB.ACC_NO,1) = '1' and B.ACC_NO = #DB.ACC_NO and B.TRN_DT = @PRE_DT

	-- 更新 [期初贷方]
	UPDATE #DB SET BEG_CRE = B.CRE_MY
	FROM TR01X as B WHERE left(#DB.ACC_NO,1) in('2','3') and B.ACC_NO = #DB.ACC_NO and B.TRN_DT = @PRE_DT

	-- 更新 [期末借方] = 期初借方+本期借方-本期貨方
	UPDATE #DB
	SET #DB.END_DEB = BEG_DEB+CUR_DEB-CUR_CRE
	FROM #DB
	WHERE left(#DB.ACC_NO,1) = '1'

	-- 更新 [期末贷方] = 期初貸方+本期貸方-本期借方
	UPDATE #DB
	SET #DB.END_CRE= BEG_CRE+CUR_CRE-CUR_DEB
	FROM #DB 
	WHERE left(#DB.ACC_NO,1) in('2','3')
	
	delete TR01X
	where TRN_DT = left(@BEG_DT,6)

	INSERT TR01X
	(TRN_DT,ACC_NO,DEB_MY,CRE_MY,CREATE_USER,CREATE_DATE)
	select A.TRN_DT,A.ACC_NO,A.END_DEB,A.END_CRE,'SYS',GETDATE()
    FROM #DB as A

	--select TRN_DT,ACC_NO,DEB_MY,CRE_MY,CREATE_USER,CREATE_DATE
	--FROM TR01X 
	--where TRN_DT = left(@BEG_DT,6)

	--上月有期末,本月沒交易,自動上月轉本月
	INSERT TR01X
	(TRN_DT,ACC_NO,DEB_MY,CRE_MY,CREATE_USER,CREATE_DATE)
	select left(@BEG_DT,6),ACC_NO,DEB_MY,CRE_MY,'SYS',GETDATE()
    FROM TR01X 
	where TRN_DT = left(@PRE_DT,6) and ACC_NO not In (select ACC_NO FROM TR01X where TRN_DT = left(@BEG_DT,6))

END
GO
