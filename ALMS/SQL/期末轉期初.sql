		CREATE TABLE #DB
	(
		TRN_DT varchar(10),     /*時間*/
		ACC_NO nvarchar(5),     /*科目編號*/
		BEG_DEB numeric(19,4),  /*期初借方*/
		BEG_CRE numeric(19,4),  /*期初贷方*/
		CUR_DEB numeric(19,4),  /*本期发生借方*/
		CUR_CRE numeric(19,4),  /*本期发生贷方*/
		END_DEB numeric(19,4),  /*期末借方*/
		END_CRE numeric(19,4)   /*期末贷方*/
	)
	
	DECLARE @ACC_NO_Digit  varchar(01)='4'   /* ACC_NO位數 */
    DECLARE @BEG_DT  varchar(10) = '20180701'   /* 查詢條件-結算日期 20180701 */
	DECLARE @END_DT  varchar(10) = '20180731'

	--SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0))

	DECLARE @TEMP_DT datetime = DATEADD(MM,-1,convert(datetime,@BEG_DT,111))
	DECLARE @PRE_DT varchar(10) = LEFT(CONVERT(varchar, @TEMP_DT,112),6) /* 期初時間 20180601 */



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

	--truncate table #DB

	select TRN_DT,ACC_NO,BEG_DEB,BEG_CRE,CUR_DEB,CUR_CRE,END_DEB,END_CRE from #DB

	INSERT TR01X
	(TRN_DT,ACC_NO,DEB_MY,CRE_MY,CREATE_USER,CREATE_DATE)
	select A.TRN_DT,A.ACC_NO,A.END_DEB,A.END_CRE,'SYS',GETDATE()
    FROM #DB as A

	select TRN_DT,ACC_NO,DEB_MY,CRE_MY,CREATE_USER,CREATE_DATE
    FROM TR01X 
	where TRN_DT = left('20180701',6)

	INSERT TR01X
	(TRN_DT,ACC_NO,DEB_MY,CRE_MY,CREATE_USER,CREATE_DATE)
	select left('20180701',6),ACC_NO,DEB_MY,CRE_MY,'SYS',GETDATE()
    FROM TR01X 
	where TRN_DT = left('20180601',6) and ACC_NO not In (select ACC_NO FROM TR01X where TRN_DT = left('20180701',6))

	select TRN_DT,ACC_NO,DEB_MY,CRE_MY,CREATE_USER,CREATE_DATE
    FROM TR01X 
	where TRN_DT = left('20180601',6) and ACC_NO not In (select ACC_NO FROM TR01X where TRN_DT =  left('20180701',6))
