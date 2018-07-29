SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		WYP
-- Create date: 20180723
-- Description: �ˬdTR01A�̭��ϧ_���ϥΡA���ϥΪ����i�R��
-- =============================================
CREATE TRIGGER Trigger_BA02A_Delete
   ON  [dbo].[BA02A]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @err_msg varchar(100);

    set @err_msg = 
		case	
			when Exists(Select * From  dbo.TR01A A Where A.BA02A_ID IN (Select D.BA02A_ID FROM deleted D))
				then '��Ƥw�Q�ϥΤ��i�R��<br />'
			Else ''
		END
	if(@err_msg <> '')
	begin
		RaisError (@err_msg, 16, 1)  
		rollback
	end
END
GO
