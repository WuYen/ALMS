-- ================================================
-- Template generated from Template Explorer using:
-- Create Trigger (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- See additional Create Trigger templates for more
-- examples of different Trigger statements.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- 編輯的時候  新增 驗證會出錯
-- =============================================
CREATE TRIGGER TR01A_TRG_INSERT 
   ON [dbo].[TR01A]
   Instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @err_msg varchar(100);
	declare @VOU_NO varchar(8);

	select @VOU_NO = VOU_NO from inserted group by VOU_NO
	if exists
	(
		select A.VOU_NO from TR01A as A with(nolock)
		where A.VOU_NO = @VOU_NO	
	)
	set @err_msg = '傳票號碼不可重複'

	if @err_msg <> ''
	begin
		set @err_msg = 'utg : (TR01A) '+ @err_msg  
		raiserror (@err_msg, 16, 1)  
		rollback
	end
	Else Insert into [dbo].[TR01A] 
	select [TRN_DT]
      ,[DA03A_ID]
      ,[VOU_NO]
      ,[BA02A_ID]
      ,[BA02B_ID]
      ,[BA03A_ID]
      ,[BA01A_ID]
      ,[SUM_RM]
      ,[DEB_MY]
      ,[CRE_MY]
      ,[CREATE_USER]
      ,[CREATE_DATE]
      ,[UPDATE_USER]
      ,[UPDATE_DATE]
      ,[ACT_YN] from inserted
END
GO
