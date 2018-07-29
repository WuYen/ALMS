
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		WYP
-- Create date: 2018/07/20
-- Description:	ALMS-試算表
-- EXEC SP_ALMS_RP02 @BEG_DT='20180701',@END_DT='20180731'
-- =============================================

IF exists(select name FROM sysobjects 
          where name = 'SP_ALMS_RP02' AND type = 'P')
drop procedure SP_ALMS_RP02
go

CREATE PROCEDURE SP_ALMS_RP02 
       @ACC_NO_Digit  varchar(01)='4',   /* ACC_NO位數 */
       @BEG_DT  varchar(10),   /* 查詢條件-起始日期 20180101 */
       @END_DT  varchar(10)   /* 查詢條件-結束日期 20180331 */       
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ACC_DT nvarchar(20) =  LEFT(@BEG_DT,4)+'.'+substring(@BEG_DT,5,2)+'-'+ LEFT(@END_DT,4)+'.'+substring(@END_DT,5,2) /* 試算表內的時間區間 */
	DECLARE @TEMP_DT datetime = DATEADD(MM,-1,convert(datetime,@BEG_DT,111))
	DECLARE @PRE_DT nvarchar(20) = LEFT(CONVERT(varchar, @TEMP_DT,112),6) /* 期初時間 */

	CREATE TABLE #DB
	(
		ACC_YY varchar(10),    /*会计年度 2015*/
		ACC_DT nvarchar(60),   /*会计期间*/
		ACC_NO nvarchar(5),    /*科目編號*/
		ACC_NM nvarchar(50),   /*科目名稱*/
		LEV_QT varchar(2),     /*級數*/
		BEG_DEB numeric(19,4),    /*期初借方*/
		BEG_CRE numeric(19,4),  /*期初贷方*/
		CUR_DEB numeric(19,4),  /*本期发生借方*/
		CUR_CRE numeric(19,4),  /*本期发生贷方*/
		END_DEB numeric(19,4),   /*期末借方*/
		END_CRE numeric(19,4)   /*期末贷方*/
	)

	-- 建立 [本月借貸]
	INSERT #DB
	(ACC_YY,ACC_DT,ACC_NO,ACC_NM,LEV_QT,BEG_DEB,BEG_CRE,CUR_DEB,CUR_CRE,END_DEB,END_CRE)
	select left(A.TRN_DT,4), @ACC_DT,left(B.ACC_NO,@ACC_NO_Digit),B.ACC_NM,@ACC_NO_Digit,0,0,isnull(SUM(A.DEB_MY),0) as 本期借,isnull(SUM(A.CRE_MY),0) as 本期貸,0,0
    FROM [ALMS].[dbo].[TR01A] as A
	left join BA01A as B on A.BA01A_ID = B.BA01A_ID
	where A.TRN_DT between @BEG_DT and @END_DT
	Group by left(B.ACC_NO,@ACC_NO_Digit), left(A.TRN_DT,4),B.ACC_NM

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

	-- 20180728 插入 [上期有期初,本月沒交易]
	INSERT #DB
	(ACC_YY,ACC_DT,ACC_NO,ACC_NM,LEV_QT,BEG_DEB,BEG_CRE,CUR_DEB,CUR_CRE,END_DEB,END_CRE)
	select left(A.TRN_DT,4), @ACC_DT,left(B.ACC_NO,@ACC_NO_Digit),B.ACC_NM,@ACC_NO_Digit,A.DEB_MY as 前借,A.CRE_MY as 前貸,0 as 本期借,0 as 本期貸,A.DEB_MY as 末借,A.CRE_MY as 末貸
    FROM [ALMS].[dbo].[TR01X] as A
	left join BA01A as B on A.ACC_NO = B.ACC_NO
	where A.TRN_DT = @PRE_DT	
	
	select ACC_YY,ACC_DT,ACC_NO,ACC_NM,LEV_QT,BEG_DEB,BEG_CRE,CUR_DEB,CUR_CRE,END_DEB,END_CRE from #DB order by ACC_NO
END
GO
